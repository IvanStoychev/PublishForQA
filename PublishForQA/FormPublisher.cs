using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace PublishForQA
{
    public partial class FormPublisher : Form
    {
        //This character will be used as a separator when writing the save file. We need it as a landmark
        //to be able to tell our position when loading the save file.
        const char Separator = '*';
        public static List<string> AccessDeniedFolders = new List<string>();
        public static List<TextBox> TextBoxesList = new List<TextBox>();

        public FormPublisher()
        {
            InitializeComponent();
            TextBoxesList.Add(tbECheckPath);
            TextBoxesList.Add(tbCorePath);
            TextBoxesList.Add(tbServicePath);
            TextBoxesList.Add(tbQAFolderPath);
            if (File.Exists("PublishForQA.txt"))
            {
                LoadFile("PublishForQA.txt");
            }
        }

        #region Events of controls
        public static void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ((FormPublisher)Form.ActiveForm).btnLocate.Menu.Close();
            ((FormPublisher)Form.ActiveForm).Locate(e.ClickedItem.ToString());
        }

        private void pbAccessDenied_Click(object sender, EventArgs e)
        {
            using (FormAccessDenied accessDenied = new FormAccessDenied(AccessDeniedFolders))
            {
                accessDenied.ShowDialog();
            }
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            //If an IllegalPath character or the Enter key would be entered in the
            //textboxes, we prevent it by setting the "Handled" property to "true".
            e.Handled =
                (
                    e.KeyChar == (char)Keys.Return ||
                    e.KeyChar == '"' ||
                    e.KeyChar == '/' ||
                    e.KeyChar == '?' ||
                    e.KeyChar == '|' ||
                    e.KeyChar == '*' ||
                    e.KeyChar == '<' ||
                    e.KeyChar == '>'
                );

            //There is a special case for the "Task Name" textbox
            if (sender.Equals(tbTaskName) && (e.KeyChar == '\\' || e.KeyChar == ':')) e.Handled = true;

            //If the pressed key is the "Enter" key we call the textbox "Leave" event.
            if (e.KeyChar == (char)Keys.Return) tb_Leave(sender, new EventArgs());
        }

        //This event is only used to emulate "Control + A" select all text behaviour
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (e.Control && e.KeyCode == Keys.A) tb.SelectAll();
        }

        private void tb_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.EndsWith(".")) tb.Text = tb.Text.TrimEnd('.');
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            CursorChange();

            //Four subsequent checks before beginning the copy operation.
            //Ordered in this way for readability.
            if (PathsAreLegal())
                if (HasBinDebug())
                    if (DirectoriesExist())
                        if(HasNetworkAccess())
                            CopyFilesAndDirectories();

            CursorChange();
        }

        private void pbHelp_Click(object sender, EventArgs e)
        {
            //We check if there already is an open FormHelp.
            //This is done by getting all open forms of type "FormHelp". Since at most there should be only one
            //we use "FirstOrDefault()" in this way we will either get the open one or null. If we get null then
            //there must be no open help forms (and we got "Default") so we open one.
            //ELSE we set the focus to the one that was returned by "First".
            if (Application.OpenForms.OfType<FormHelp>().FirstOrDefault() == null)
            {
                new FormHelp().Show();
            }
            else
            {
                Application.OpenForms.OfType<FormHelp>().First().Focus();
            }
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete("PublishForQA.txt");
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("The save file is locked by another process.\nSaving failed.", "Save failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (TextBox tb in TextBoxesList)
            {
                File.AppendAllText("PublishForQA.txt", tb.Name + Separator + " " + tb.Text + Environment.NewLine);
            }
        }

        private void pbLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFile(openFileDialog.FileName);
            }
        }
        #endregion

        /// <summary>
        /// Checks whether all paths have at most a single colon character.
        /// </summary>
        /// <returns>"True" if all paths contain no more than a single colon character per path, otherwise "False".</returns>
        private bool PathsAreLegal()
        {
            //This list will hold all text boxes whose paths contain more than one colon character.
            List<TextBox> pathIsIllegal = new List<TextBox>();
            //For each TextBox we check if the position of the last colon character is greater than 1.
            //If it is that means it is located further than where it should be for a drive letter
            //which in all likelyhood is wrong, so we add it to the list.
            foreach (var tb in TextBoxesList)
            {
                if (tb.Text.LastIndexOf(':') > 1) pathIsIllegal.Add(tb);
            }

            if (pathIsIllegal.Count == 1)
            {
                DialogResult fixPath = MessageBox.Show("The path of " + NameReplace(pathIsIllegal[0]) + " looks illegal as it contains a ':' character where it shouldn't and thus copying cannot continue.\nWould you like to fix it by removing all ':' characters but the first one and continue?", "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (fixPath == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    int firstColon = pathIsIllegal[0].Text.IndexOf(':');
                    pathIsIllegal[0].Text = pathIsIllegal[0].Text.Replace(":", "");
                    pathIsIllegal[0].Text = pathIsIllegal[0].Text.Insert(firstColon, ":");
                    return true;
                }
            }
            else if (pathIsIllegal.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths look illegal because they contain a ':' character where they shouldn't:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in pathIsIllegal)
                {
                    stringBuilder.AppendLine(NameReplace(tb));
                }
                stringBuilder.Append(Environment.NewLine + "Copying cannot proceed like this.\nWould you like to fix it by removing all ':' characters in each path but the first one and continue?");
                DialogResult fixPath = MessageBox.Show(stringBuilder.ToString(), "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (fixPath == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    foreach (var tb in pathIsIllegal)
                    {
                        int firstColon = tb.Text.IndexOf(':');
                        tb.Text = tb.Text.Replace(":", "");
                        tb.Text = tb.Text.Insert(firstColon, ":");
                    }
                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether all paths end with a "bin\Debug" folder and alerts the user with an OKCancel MessageBox if any do not
        /// </summary>
        /// <returns>"True" if all paths have a "bin\Debug" folder or the user chose to ignore any that do not. "False" if the
        /// user decides to halt operation after being alerted.</returns>
        /// <remarks>The MessageBox lists all TextBoxes that do not end with a "bin\Debug" folder</remarks>
        private bool HasBinDebug()
        {
            //And we check if the paths ends with in "bin\Debug" folder.
            List<TextBox> tbNoBinDebugList = new List<TextBox>();
            foreach (var tb in TextBoxesList)
            {
                //We skip the check for the QA Folder TextBox.
                if (tb == tbQAFolderPath) continue;
                //For clarity and "just in case", we add a slash at the end of paths that don't have one.
                if (!tb.Text.EndsWith("\\")) tb.Text = tb.Text + "\\";

                if (!tb.Text.ToLower().EndsWith("\\bin\\debug\\"))
                {
                    tbNoBinDebugList.Add(tb);
                }
            }

            //For user-friendlyness-ness-ness-ness we format the shown error in singular or plural case.
            if (tbNoBinDebugList.Count == 1)
            {
                DialogResult confirm = MessageBox.Show("The path of " + NameReplace(tbNoBinDebugList[0]) + " does not end with a \"bin\\Debug\" folder.\nAre you sure you wish to proceed?", "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.No)
                {
                    return false;
                }
            }
            else if (tbNoBinDebugList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths don't end with a \"bin\\Debug\" folder:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbNoBinDebugList)
                {
                    stringBuilder.AppendLine(NameReplace(tb));
                }
                stringBuilder.Append(Environment.NewLine + "Are you sure you wish to proceed?");
                DialogResult confirm = MessageBox.Show(stringBuilder.ToString(), "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Checks whether the listed directories in all TextBoxes exist and alerts the user and stops execution if any do not
        /// </summary>
        /// <returns>"True" if all directories exist, otherwise "False"</returns>
        private bool DirectoriesExist()
        {
            //This list will hold all text boxes whose listed directories do not exist.
            List<TextBox> doesNotExist = new List<TextBox>();
            //For each TextBox we check if its listed directory exists and add it to the list if it does not.
            foreach (var tb in TextBoxesList)
            {
                if (!Directory.Exists(tb.Text) || tb.Text == "")
                {
                    doesNotExist.Add(tb);
                }
            }

            //For user-friendlyness-ness-ness-ness we format the shown error in singular or plural case.
            if (doesNotExist.Count == 1)
            {
                MessageBox.Show("The directory for " + doesNotExist[0].Name.Replace("tb", "").Replace("Path", "") + " does not exist. Please check that the path is correct.", "Path error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (doesNotExist.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The directories for the following do not exist:" + Environment.NewLine + Environment.NewLine);
                foreach (var txtb in doesNotExist)
                {
                    stringBuilder.AppendLine(NameReplace(txtb));
                }
                stringBuilder.Append(Environment.NewLine + "Please check that the paths are correct.");
                MessageBox.Show(stringBuilder.ToString(), "Path error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether the user has write permissions for the network folder
        /// </summary>
        /// <returns>"True" if the user can write to the folder, otherwise "False"</returns>
        private bool HasNetworkAccess()
        {
            try
            {
                //This will raise an exception if the path is read only or the user
                //does not have access to view the permissions.
                Directory.GetAccessControl(tbQAFolderPath.Text);
                return true;
            }
            catch (UnauthorizedAccessException UAex)
            {
                MessageBox.Show("You are not authorized to access the network folder:\n" + UAex.Message, "Unauthorized Access Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (InvalidOperationException IOex)
            {
                MessageBox.Show("Invalid operation:\n" + IOex.Message, "Invalid Operation Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown exception occured", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Recreates the directory structure at the target location and copies all files from the source recursively.
        /// </summary>
        private void CopyFilesAndDirectories()
        {
            //We create an array of strings which will be the targets of the copy operation.
            //They consist of the user's pointed QA Folder and the name of each folder needed for QA.
            string[] destinationPaths =
                {
                tbQAFolderPath.Text + tbTaskName.Text + "\\E-Check\\",
                tbQAFolderPath.Text + tbTaskName.Text + "\\E-CheckCore\\",
                tbQAFolderPath.Text + tbTaskName.Text + "\\E-CheckService\\"
                };
            //We set the sources, corresponding to the target paths, from the respective TextBoxes.
            string[] sourcePaths =
                {
                tbECheckPath.Text,
                tbCorePath.Text,
                tbServicePath.Text
                };

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    //First we create the directory structure
                    foreach (string dirPath in Directory.GetDirectories(sourcePaths[i], "*", SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(sourcePaths[i], destinationPaths[i]));

                    //Then we copy all files, overwriting any existing ones
                    foreach (string filePath in Directory.GetFiles(sourcePaths[i], "*", SearchOption.AllDirectories))
                        File.Copy(filePath, filePath.Replace(sourcePaths[i], destinationPaths[i]), true);
                }
                //We previously already checked for network access but just in case something changes
                //while the copy operation is in progress we try to catch a UnauthorizedAccessException again.
                catch (System.UnauthorizedAccessException UAex)
                {
                    MessageBox.Show("You are not authorized to access the network folder:\n" + UAex.Message, "Unauthorized Access Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (IOException IOex)
                {
                    MessageBox.Show("IO exception occurred:\n" + IOex.Message, "IOException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show("Unknown exception occured", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }

            MessageBox.Show("Copy operation completed successfully!", "Operation finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Gets a list of all the folders of the fixed and removable drives on the system and searches through them for
        /// a folder named as the passed version parameter.
        /// </summary>
        /// <param name="version">The folder name of the E-Check version to search for</param>
        /// <remarks>The method also keeps a list of all folders that access was denied to</remarks>
        private void Locate(string version)
        {
            List<DriveInfo> drives = new List<DriveInfo>();
            List<string> ECheckresults = new List<string>();
            List<string> Coreresults = new List<string>();
            drives.AddRange(DriveInfo.GetDrives());
            drives = drives
                           .Where(x => x.DriveType == DriveType.Fixed || x.DriveType == DriveType.Removable)
                           .ToList();

            pbAccessDenied.Visible = false;
            CursorChange();
            AccessDeniedFolders.Clear();

            //For each Fixed or Removable storage drive on the system we search for folders
            //named after the selected version and "E-CheckCore".
            //We also create a list of all folders to which access was denied to.
            foreach (var drive in drives)
            {
                List<string> folders = new List<string>();
                folders.AddRange(Directory.GetDirectories(drive.Name));
                foreach (var folder in folders)
                {
                    try
                    {
                        ECheckresults.AddRange(Directory.GetDirectories(folder, version, SearchOption.AllDirectories));
                        Coreresults.AddRange(Directory.GetDirectories(folder, "E-CheckCore", SearchOption.AllDirectories));
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.UnauthorizedAccessException)
                        {
                            AccessDeniedFolders.Add(ex.Message.Replace(@"Access to the path '", "").Replace(@"' is denied.", ""));
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            //If there were any folders that access was denied to we show the
            //button which opens the dialog which lists them.
            if (AccessDeniedFolders.Count > 0)
            {
                AccessDeniedFolders = AccessDeniedFolders.Distinct().ToList();
                AccessDeniedFolders.Sort();
                pbAccessDenied.Visible = true;
            }
            else
            {
                pbAccessDenied.Visible = false;
            }

            CursorChange();

            List<string> eCheckPath = ECheckresults.Where(x => Directory.Exists(x + @"\master\WinClient\E-Check\bin\Debug\") && Directory.Exists(x + @"\master\AppServer\ServiceHostNew\ServiceHostNew\bin\Debug\")).ToList();
            List<string> corePath = Coreresults.Where(x => Directory.Exists(x + @"\E-CheckCore\E-CheckCoreConsoleHost\bin\Debug\")).ToList();

            //No results for either E-Check or E-CheckCore
            if (eCheckPath.Count < 1 && corePath.Count < 1)
            {
                MessageBox.Show("Neither " + version + " nor E-CheckCore were found", "No results", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //No results for E-Check
            else if (eCheckPath.Count > 0 && corePath.Count < 1)
            {
                MessageBox.Show(version + "was not found.", "Partial success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //No results for E-CheckCore
            else if (eCheckPath.Count < 1 && corePath.Count > 0)
            {
                MessageBox.Show("E-CheckCore was not found.", "Partial success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (eCheckPath.Count == 1)
            {
                tbECheckPath.Text = Path.Combine(eCheckPath[0], @"master\WinClient\E-Check\bin\Debug\");
                tbServicePath.Text = Path.Combine(eCheckPath[0], @"master\AppServer\ServiceHostNew\ServiceHostNew\bin\Debug\");
            }
            if (corePath.Count == 1)
            {
                tbCorePath.Text = Path.Combine(corePath[0], @"E-CheckCore\E-CheckCoreConsoleHost\bin\Debug\");
            }

            if (eCheckPath.Count > 1 || corePath.Count > 1)
            {
                using (FormTooManyResults formTooManyResults = new FormTooManyResults(eCheckPath, corePath))
                {
                    formTooManyResults.ShowDialog();
                }
            }
        }

        private void Browse(object sender, EventArgs e)
        {
            //First we get the TextBox, corresponding to the pressed button.
            //Then we set the selected path of the FolderBrowserDialog to the text of said TextBox.
            //If the TextBox has invalid text it will just default to "Desktop".
            //Lastly if the user clicked "OK" we set the TextBox text to be the selected path and add
            //a backslash to its end (by use of shorthand "if... then... else" statement) if it already doesn't have one.
            Control control = sender as Control;
            TextBox textBox = this.Controls.Find(control.Name.Replace("btn", "tb").Replace("Browse", "Path"), false).OfType<TextBox>().FirstOrDefault();
            folderBrowserDialog.SelectedPath = textBox.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) textBox.Text = folderBrowserDialog.SelectedPath.EndsWith("\\") ? folderBrowserDialog.SelectedPath : folderBrowserDialog.SelectedPath + "\\";
        }

        /// <summary>
        /// Reads the given file line by line and attempts to retrieve the values for all TextBoxes.
        /// It will display a MessageBox with all TextBoxes for whom it could not find a value.
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadFile(string filePath)
        {
            //We will use this list to tell if a value for a TextBox was missing in the save file.
            List<TextBox> notFoundBoxes = new List<TextBox>();
            notFoundBoxes.AddRange(TextBoxesList);
            string line;
            using (StreamReader file = new StreamReader(filePath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        //We get the substring from the beginning of the line to the first occurance of our separator. That
                        //should be the name of one of our TextBoxes. If we can match the substring to a TextBox that means
                        //we have a value for it and we take it out of the list "notFoundBoxes". Thus in the end we have a
                        //list of only the TextBoxes we couldn't find a value for.
                        TextBox tb = this.Controls.Find(line.Substring(0, line.IndexOf(Separator)), false).OfType<TextBox>().FirstOrDefault();
                        if (tb == null) continue;
                        tb.Text = line.Substring(line.IndexOf(Separator) + 2);
                        notFoundBoxes.Remove(tb);
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {

                    }
                }
                if (notFoundBoxes.Count == 1)
                {
                    MessageBox.Show("The path for " + notFoundBoxes[0].Name.Replace("tb", "").Replace("Path", " Path") + " could not be read from the file.", "Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (notFoundBoxes.Count > 1)
                {
                    StringBuilder stringBuilder = new StringBuilder("The paths for the following TextBoxes:" + Environment.NewLine + Environment.NewLine);
                    foreach (var tb in notFoundBoxes)
                    {
                        stringBuilder.AppendLine(NameReplace(tb));
                    }
                    stringBuilder.Append(Environment.NewLine + "could not be read from the save file.");
                    MessageBox.Show(stringBuilder.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void CursorChange()
        {
            if (Cursor == Cursors.Default)
            {
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Converts the name of a TextBox. Removes all instances of "tb" and "Path".
        /// </summary>
        /// <param name="tb">The TextBox whose name should be converted</param>
        /// <returns>A String instance of the TextBox name with no instances of "tb" and/or "Path"</returns>
        private string NameReplace(TextBox tb)
        {
            return tb.Name.Replace("tb", "").Replace("Path", "");
        }
    }
}