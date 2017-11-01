using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
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
            if (sender.Equals(tbTaskName) && e.KeyChar == '\\') e.Handled = true;

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

            if (HasBinDebug())
                if (DoDirectoriesExist())
                    CopyFilesAndDirectories();

            CursorChange();
        }

        private void pbHelp_Click(object sender, EventArgs e)
        {
            
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

        /// <summary>
        /// Checks whether all paths end with a "bin\Debug" folder and alerts the user with an OKCancel MessageBox if any do not
        /// </summary>
        /// <returns>"True" if all paths have a "bin\Debug" folder or the user chose to ignore any that do not. "False" if the
        /// user decides to halt operation after being alerted.</returns>
        /// <remarks>The MessageBox lists all TextBoxes that do not end with a "bin\Debug" folder</remarks>
        private bool HasBinDebug()
        {
            //For clarity and "just in case", we add a slash at the end of paths that don't have one.
            //And we check if the paths ends with in "bin\Debug" folder.
            List<TextBox> tbNoBinDebugList = new List<TextBox>();
            foreach (var tb in TextBoxesList)
            {
                //We skip the check for the QA Folder TextBox.
                if (tb == tbQAFolderPath) continue;
                if (!tb.Text.EndsWith("\\")) tb.Text = tb.Text + "\\";
                //Considering the previous validation all paths should end in "\\" but
                //just in case we also check for "\\bin\\debug", as well.
                if (!tb.Text.ToLower().EndsWith("\\bin\\debug\\") && !tb.Text.ToLower().EndsWith("\\bin\\debug"))
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
        private bool DoDirectoriesExist()
        {
            List<TextBox> doesNotExist = new List<TextBox>();
            //For each TextBox we check if its listed directory exists and alert the user and stop execution if it does not.
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
                //First we create the directory structure
                foreach (string dirPath in Directory.GetDirectories(sourcePaths[i], "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourcePaths[i], destinationPaths[i]));

                //Then we copy all files, overwriting any existing ones
                foreach (string filePath in Directory.GetFiles(sourcePaths[i], "*", SearchOption.AllDirectories))
                    File.Copy(filePath, filePath.Replace(sourcePaths[i], destinationPaths[i]), true);
            }
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

    public class MenuButton : Button
    {
        [DefaultValue(null)]
        public ContextMenuStrip Menu { get; set; }

        [DefaultValue(false)]
        public bool ShowMenuUnderCursor { get; set; }

        public MenuButton()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(FormPublisher.contextMenuStrip_ItemClicked);
            Menu = contextMenuStrip;
            string result = string.Empty;
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("PublishForQA.Resources.E-CheckVersions.xml"))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            XmlReader reader = XmlReader.Create(new StringReader(result));
            var doc = XDocument.Load(reader);
            List<XElement> elements = doc.Root.Elements("Version").ToList();
            foreach (var element in elements)
            {
                contextMenuStrip.Items.Add(element.Value);
            }
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if (Menu != null && mevent.Button == MouseButtons.Left)
            {
                Point menuLocation;

                if (ShowMenuUnderCursor)
                {
                    menuLocation = mevent.Location;
                }
                else
                {
                    menuLocation = new Point(0, Height);
                }

                Menu.Show(this, menuLocation);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (Menu != null)
            {
                int arrowX = ClientRectangle.Width - 14;
                int arrowY = ClientRectangle.Height / 2 - 1;

                Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
                Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
                pevent.Graphics.FillPolygon(brush, arrows);
            }
        }
    }
}