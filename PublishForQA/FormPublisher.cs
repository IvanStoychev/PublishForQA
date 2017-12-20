using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.ComponentModel;

namespace PublishForQA
{
    public partial class FormPublisher : Form
    {
        /// <summary>
        /// This character will be used as a separator when writing the save file. We need it as a landmark
        /// to be able to tell our position when loading the save file.
        /// </summary>
        const char Separator = '*';
        /// <summary>
        /// A list of all text boxes on the form.
        /// </summary>
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

            //Subsequent checks before beginning the copy operation.
            //Ordered in this way for readability.
            if (NotEmpty())
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
            catch (IOException)
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

        private void pbCopyToClipboard_Click(object sender, EventArgs e)
        {
            string errorText;
            ErrorProvider errorProvider = new ErrorProvider(Application.OpenForms.OfType<FormPublisher>().FirstOrDefault());

            try
            {
                if (!tbQAFolderPath.Text.EndsWith("\\") && tbQAFolderPath.Text != "") tbQAFolderPath.Text = tbQAFolderPath.Text + "\\";
                Clipboard.SetText(Path.Combine(tbQAFolderPath.Text + tbTaskName.Text));
                errorText = "QA folder path copied to clipboard.";
                errorProvider.Icon = Properties.Resources.Success;
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            }
            catch (ArgumentNullException)
            {
                Clipboard.Clear();
                errorText = "No QA folder and task name!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unknown exception has occurred:\n" + ex.Message + "\n\nCopy to Clipboard operation failed.", "Unknown exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            errorProvider.SetError(pbCopyToClipboard, errorText);
            System.Threading.Tasks.Task.Delay(3000).ContinueWith(t => errorProvider.Dispose());
        }
        #endregion

        /// <summary>
        /// Checks if any TextBox's value is empty and asks the user if he would
        /// like to omit it if it is.
        /// </summary>
        /// <returns>
        /// "True" if all text boxes have values or the user decided to skip those
        /// that do not. "False" if there are empty values and the user does not wish to continue.
        /// </returns>
        /// <remarks>
        /// The QA folder is mandatory and cannot be omitted.
        /// </remarks>
        private bool NotEmpty()
        {
            //First we check if the "QA Folder" TextBox is empty.
            //Since it is mandatory we alert the user if it is.
            if (tbQAFolderPath.Text.Length < 1)
            {
                MessageBox.Show("No value provided for your QA folder.\nIt is mandatory, operation cannot continue.", "No QA Folder entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            List<TextBox> tbNoValueList = new List<TextBox>();
            foreach (var tb in TextBoxesList)
            {
                if (tb.Text.Length < 1)
                {
                    tbNoValueList.Add(tb);
                }
            }

            //If there are no text boxes with no values we continue.
            if (tbNoValueList.Count == 0)
            {
                return true;
            }

            //For user-friendlyness-ness-ness-ness we format the shown error in singular or plural case.
            if (tbNoValueList.Count == 1)
            {
                DialogResult confirm = MessageBox.Show(NameReplace(tbNoValueList[0]) + " is empty.\n\nDo you wish to proceed without it?", "Empty value", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    TextBoxesList.Remove(tbNoValueList[0]);
                    return true;
                }
            }
            else if (tbNoValueList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following text boxes are empty:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbNoValueList)
                {
                    stringBuilder.AppendLine(NameReplace(tb));
                }
                stringBuilder.Append(Environment.NewLine + "Do you wish to proceed without them?");
                DialogResult confirm = MessageBox.Show(stringBuilder.ToString(), "Empty value", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    foreach (var tb in tbNoValueList)
                    {
                        TextBoxesList.Remove(tb);
                    }
                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether all paths have at most a single colon character.
        /// </summary>
        /// <returns>
        /// "True" if all paths contain no more than a single colon character per path, otherwise "False".
        /// </returns>
        private bool PathsAreLegal()
        {
            //This list will hold all text boxes whose paths contain more than one colon character.
            List<TextBox> tbIllegalPathList = new List<TextBox>();
            //For each TextBox we check if the position of the last colon character is greater than 1.
            //If it is that means it is located further than where it should be for a drive letter
            //which in all likelyhood is wrong, so we add it to the list.
            foreach (var tb in TextBoxesList)
            {
                if (tb.Text.LastIndexOf(':') > 1) tbIllegalPathList.Add(tb);
            }

            //If there are no text boxes with illegal paths we continue.
            if (tbIllegalPathList.Count == 0)
            {
                return true;
            }

            if (tbIllegalPathList.Count == 1)
            {
                DialogResult fixPath = MessageBox.Show("The path of " + NameReplace(tbIllegalPathList[0]) + " looks illegal as it contains a ':' character where it shouldn't and thus copying cannot continue.\nWould you like to fix it by removing all ':' characters but the first one and continue?", "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (fixPath == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    int firstColon = tbIllegalPathList[0].Text.IndexOf(':');
                    tbIllegalPathList[0].Text = tbIllegalPathList[0].Text.Replace(":", "");
                    tbIllegalPathList[0].Text = tbIllegalPathList[0].Text.Insert(firstColon, ":");
                    return true;
                }
            }
            else if (tbIllegalPathList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths look illegal because they contain a ':' character where they shouldn't:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbIllegalPathList)
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
                    foreach (var tb in tbIllegalPathList)
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
        /// Checks whether all paths end with a "bin\Debug" folder and alerts the user with an OKCancel MessageBox if any do not.
        /// </summary>
        /// <returns>
        /// "True" if all paths have a "bin\Debug" folder or the user chose to ignore any that do not. "False" if the
        /// user decides to halt operation after being alerted.
        /// </returns>
        /// <remarks>
        /// The MessageBox lists all TextBoxes that do not end with a "bin\Debug" folder.
        /// </remarks>
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

            //If all text boxes end in "bin\Debug" we continue.
            if (tbNoBinDebugList.Count == 0)
            {
                return true;
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
        /// Checks whether the listed directories in all TextBoxes exist and alerts the user and stops execution if any do not.
        /// </summary>
        /// <returns>
        /// "True" if all directories exist, otherwise "False".
        /// </returns>
        private bool DirectoriesExist()
        {
            //This list will hold all text boxes whose listed directories do not exist.
            List<TextBox> tbDoesNotExistList = new List<TextBox>();
            //For each TextBox we check if its listed directory exists and add it to the list if it does not.
            foreach (var tb in TextBoxesList)
            {
                if (!Directory.Exists(tb.Text) || tb.Text == "")
                {
                    tbDoesNotExistList.Add(tb);
                }
            }

            //If all directories exist we continue.
            if (tbDoesNotExistList.Count == 0)
            {
                return true;
            }

            //For user-friendlyness-ness-ness-ness we format the shown error in singular or plural case.
            if (tbDoesNotExistList.Count == 1)
            {
                //If the folder that does not exist is the QA one we prompt the user to create it.
                if (tbDoesNotExistList[0] == tbQAFolderPath)
                {
                    DialogResult create = MessageBox.Show("The directory for " + NameReplace(tbDoesNotExistList[0]) + " does not exist.\nWould you like to create it?" + "\n\nOperation will continue if either \"Yes\" or \"No\" are chosen.", "Path error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                    if (create == DialogResult.Yes) //User chose to create the directory.
                    {
                        return CreateQAFolder();
                    }
                    else if (create == DialogResult.Cancel) //User chose to abort the operation.
                    {
                        return false;
                    }
                    else //User chose to not create the direcotry.
                    {
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("The directory for " + NameReplace(tbDoesNotExistList[0]) + " does not exist.\nPlease check that the path is correct.", "Path error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (tbDoesNotExistList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The directories for the following do not exist:" + Environment.NewLine + Environment.NewLine);
                foreach (var txtb in tbDoesNotExistList)
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
        /// Checks whether the user has write permissions for the network folder.
        /// </summary>
        /// <returns>
        /// "True" if the user can write to the folder, otherwise "False".
        /// </returns>
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
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected exception occured when checking for access rights in QA Folder:\n" + ex.Message + "\n\nOperation failed in " + System.Reflection.MethodBase.GetCurrentMethod().Name + " method.", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Attempts to create a folder at the designated QA Folder path.
        /// </summary>
        /// <returns>
        /// "True" if creation was successful, "False" otherwise.
        /// </returns>
        private bool CreateQAFolder()
        {
            try
            {
                Directory.CreateDirectory(tbQAFolderPath.Text);
                return true;
            }
            catch (PathTooLongException)
            {
                MessageBox.Show("The path entered for the QA Folder is too long.\nPaths must be less than 248 characters and file names must be less than 260 characters.\n\nOperation failed.", "Path Too Long Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("You do not have sufficient permissions in the location you want the folder to be created in.\n\nOperation failed.", "Unauthorized Access Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (IOException IOex)
            {
                MessageBox.Show("An IO exception has occurred:\n" + IOex.Message + "\n\nOperation failed.", "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected exception has occurred:\n" + ex.Message + "\n\nOperation failed in " + System.Reflection.MethodBase.GetCurrentMethod().Name + " method.", "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
                    //First we create the directory structure.
                    foreach (string dirPath in Directory.GetDirectories(sourcePaths[i], "*", SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(sourcePaths[i], destinationPaths[i]));

                    //Then we copy all files, overwriting any existing ones.
                    foreach (string filePath in Directory.GetFiles(sourcePaths[i], "*", SearchOption.AllDirectories))
                        File.Copy(filePath, filePath.Replace(sourcePaths[i], destinationPaths[i]), true);
                }
                //We previously already checked for network access but just in case something changes
                //while the copy operation is in progress we try to catch a UnauthorizedAccessException again.
                catch (UnauthorizedAccessException UAex)
                {
                    MessageBox.Show("You are not authorized to access the network folder:\n" + UAex.Message + "\n\nCopy operation failed.", "Unauthorized Access Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (PathTooLongException)
                {
                    MessageBox.Show("The path \"" + destinationPaths[i] + "\" is too long.\nPaths must be less than 248 characters and file names must be less than 260 characters.\n\nCopy operation failed.", "Path Too Long Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (IOException IOex)
                {
                    MessageBox.Show("IO exception occurred:\n" + IOex.Message + "\n\nCopy operation failed.", "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected exception occured:\n" + ex.Message + "\n\nCopy operation failed.", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            List<string> allDirectories = new List<string>();
            List<string> eCheckDirectories = new List<string>();
            List<string> coreDirectories = new List<string>();
            List<string> eCheckResults = new List<string>();
            List<string> coreResults = new List<string>();

            //We get all fixed and removable drives on the system.
            drives.AddRange(DriveInfo.GetDrives()
                           .Where(x => x.DriveType == DriveType.Fixed || x.DriveType == DriveType.Removable)
                           .ToList());

            CursorChange();

            //For each Fixed or Removable storage drive on the system we get all folders in that drive.
            foreach (var drive in drives)
            {
                try
                {
                    allDirectories.AddRange(FolderEnumerator.EnumerateFoldersRecursively(drive.Name).ToList());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected exception has occurred:\n" + ex.Message + "\n\nOperation failed in " + System.Reflection.MethodBase.GetCurrentMethod().Name + " method.", "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }

            //We filter all the directories by the version name and "E-CheckCore".
            eCheckDirectories.AddRange(allDirectories.Where(x => x.Contains("\\E-Check\\" + version + "\\")));
            coreDirectories.AddRange(allDirectories.Where(x => x.Contains("\\E-CheckCore\\")));

            //We get the filtered directories for which exist the vital debug folders.
            eCheckResults.AddRange(eCheckDirectories.Where(x => Directory.Exists(x + @"\WinClient\E-Check\bin\Debug\") && Directory.Exists(x + @"\AppServer\ServiceHostNew\ServiceHostNew\bin\Debug\")).ToList());
            coreResults.AddRange(coreDirectories.Where(x => Directory.Exists(x + @"\E-CheckCoreConsoleHost\bin\Debug\")).ToList());

            CursorChange();

            //If no results for either E-Check or E-CheckCore:
            if (eCheckResults.Count < 1 && coreResults.Count < 1)
            {
                MessageBox.Show("Neither " + version + " nor E-CheckCore were found.", "No results", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //otherwise:
            using (FormResults formResults = new FormResults(eCheckResults, coreResults, version))
            {
                formResults.ShowDialog();
            }
            return;
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
        /// It will display a MessageBox with all TextBoxes for which it could not find a value.
        /// </summary>
        /// <param name="filePath">The path to the save file.</param>
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("An unknown exception has occurred while reading the save file:\n" + ex.Message, "Unknown exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }
                if (notFoundBoxes.Count == 1)
                {
                    MessageBox.Show("The path for " + NameReplace(notFoundBoxes[0]) + " could not be read from the file.", "Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        /// <summary>
        /// Changes the mouse cursor between "WaitCursor" and "Default".
        /// </summary>
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
        /// <param name="tb">The TextBox whose name should be converted.</param>
        /// <returns>
        /// A String instance of the TextBox name with no instances of "tb" and/or "Path".
        /// </returns>
        private string NameReplace(TextBox tb)
        {
            return tb.Name.Replace("tb", "").Replace("Path", "");
        }
    }
}