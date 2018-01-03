using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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
        /// This is the string used to detect if an exception has occured before the
        /// directory creation for-loop.
        /// </summary>
        const string ErrorBeforeDirectoryLoop = "Exception occurred before the start of directory structure creation.";
        /// <summary>
        /// This is the string used to detect if an exception has occured before the
        /// file copying for-loop.
        /// </summary>
        const string ErrorBeforeFileLoop = "Exception occurred before the start of the file copy process.";
        /// <summary>
        /// A list of all text boxes on the form.
        /// </summary>
        public static List<TextBox> TextBoxesList = new List<TextBox>();

        public FormPublisher()
        {
            InitializeComponent();
            ListTextBoxes();

            List<string> txtFilesInDir = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt", SearchOption.TopDirectoryOnly).ToList();
            if (txtFilesInDir.Count == 1)
            {
                LoadFile(txtFilesInDir.First());
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

        //This event is only used to emulate "Control + A" select all text behaviour.
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
            ListTextBoxes();
            if (NotEmpty())
                if (PathsAreLegal())
                    if (HasBinDebug())
                        if (DirectoriesExist())
                            if (HasNetworkAccess())
                                CopyFilesAndDirectories();

            ListTextBoxes();
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
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (TextBox tb in TextBoxesList)
                        {
                            sw.WriteLine(tb.Name + Separator + " " + tb.Text);
                        }
                    }
                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("The name provided for the file was null." + Environment.NewLine + "Save operation failed.", "Null argument exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException)
                {
                    MessageBox.Show("The file is locked by another process." + Environment.NewLine + "Save operation failed.", "IO exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected exception has occurred:\n" + ex.Message + "\n\nSave operation failed.", "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private void pbLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFile(openFileDialog.FileName);
            }
        }

        private void pbLoadDropdown_Click(object sender, EventArgs e)
        {
            PictureBox owner = sender as PictureBox;
            List<string> saveFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).Take(10).ToList();
            ContextMenuStrip dropDown = new ContextMenuStrip();
            foreach (var file in saveFiles)
            {
                dropDown.Items.Add(file.Replace(".txt", string.Empty), null, ContextMenuStrip_Click);
            }
            dropDown.Show(owner, new System.Drawing.Point(0, owner.Height));
        }

        private void ContextMenuStrip_Click(object sender, EventArgs e)
        {
            LoadFile(Path.Combine(Directory.GetCurrentDirectory(), sender.ToString() + ".txt"));
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
                MessageBox.Show("An unexpected exception has occurred:\n" + ex.Message + "\n\nCopy to Clipboard operation failed.", "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            errorProvider.SetError(pbCopyToClipboard, errorText);
            System.Threading.Tasks.Task.Delay(3000).ContinueWith(t => errorProvider.Dispose());
        }
        
        private void pbBatchFile_Click(object sender, EventArgs e)
        {
            string createBatchResult = CreateBatchFile();
            if (createBatchResult == string.Empty)
            {
                MessageBox.Show("Batch file generated successfully.", "Batch file generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Batch file generation failed with the following error:" + Environment.NewLine + createBatchResult, "Batch file generation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /// <summary>
        /// Hardcodes all TextBoxes into a List<TextBox> for ease
        /// of operation later.
        /// </summary>
        private void ListTextBoxes()
        {
            TextBoxesList.Clear();
            TextBoxesList.Add(tbECheckPath);
            TextBoxesList.Add(tbCorePath);
            TextBoxesList.Add(tbServicePath);
            TextBoxesList.Add(tbQAFolderPath);
        }

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
            
            //Then we add all TextBoxes with an empty Text property to a list
            //that will be used to display a warning and manipulate them further.
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
                    TextBoxesList = TextBoxesList.Except(tbNoValueList).ToList();
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
            List<TextBox> tbIllegalColonList = new List<TextBox>();
            //This list will hold all text boxes whose paths contain more than one consecutive backslash character.
            List<TextBox> tbIllegalBackslashList = new List<TextBox>();
            //For each TextBox we check if the position of the last colon character is greater than 1.
            //If it is that means it is located further than where it should be for a drive letter
            //which in all likelyhood is wrong, so we add it to the list.
            //Similarly we use a RegEx to check if a path has more than one backslash character. If it does
            //we add it to the appropriate list.
            foreach (var tb in TextBoxesList)
            {
                if (tb.Text.LastIndexOf(':') > 1) tbIllegalColonList.Add(tb);
                if (Regex.IsMatch(tb.Text.Substring(1), @"[\\]{2,}")) tbIllegalBackslashList.Add(tb);
            }

            //If there are no text boxes with illegal paths we continue.
            if (tbIllegalColonList.Count == 0 && tbIllegalBackslashList.Count == 0)
            {
                return true;
            }

            if (tbIllegalColonList.Count == 1)
            {
                DialogResult fixPath = MessageBox.Show("The path of " + NameReplace(tbIllegalColonList[0]) + " looks illegal as it contains a ':' character where it shouldn't and thus copying cannot continue.\nWould you like to fix it by removing all ':' characters but the first one and continue?", "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (fixPath == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    FixColons(tbIllegalColonList);
                }
            }
            else if (tbIllegalColonList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths look illegal because they contain a ':' character where they shouldn't:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbIllegalColonList)
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
                    FixColons(tbIllegalColonList);
                }
            }

            if (tbIllegalBackslashList.Count == 1)
            {
                DialogResult fixPath = MessageBox.Show("The path of " + NameReplace(tbIllegalBackslashList[0]) + " looks illegal as it contains too many consecutive '\\' characters.\nWould you like to fix that by replacing them with a single '\\' character?" + "\n\nOperation will continue if either \"Yes\" or \"No\" are chosen.", "Path warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (fixPath == DialogResult.Cancel)
                {
                    return false;
                }
                else if (fixPath == DialogResult.Yes)
                {
                    FixBackslashes(tbIllegalBackslashList);
                }
            }
            else if (tbIllegalBackslashList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths look illegal because they contain too many consecutive '\\' characters:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbIllegalBackslashList)
                {
                    stringBuilder.AppendLine(NameReplace(tb));
                }
                stringBuilder.Append(Environment.NewLine + "Would you like to fix that by replacing them all with a single '\\' character?");
                stringBuilder.AppendLine(Environment.NewLine + Environment.NewLine + "Operation will continue if either \"Yes\" or \"No\" are chosen.");
                DialogResult fixPath = MessageBox.Show(stringBuilder.ToString(), "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (fixPath == DialogResult.Cancel)
                {
                    return false;
                }
                else if (fixPath == DialogResult.Yes)
                {
                    FixBackslashes(tbIllegalBackslashList);
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
                //For clarity and "just in case", we add a slash at the end of paths that don't have one.
                if (!tb.Text.EndsWith("\\")) tb.Text = tb.Text + "\\";
                
                //We skip the check for the QA Folder TextBox.
                if (tb == tbQAFolderPath) continue;

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
                if (!Directory.Exists(tb.Text))
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
                    else //User chose not to create the direcotry.
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
                if (tbDoesNotExistList.Contains(tbQAFolderPath)) stringBuilder.AppendLine(Environment.NewLine + "The QA Folder can be automatically created but the other paths need to be corrected first.");
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
            List<TextBox> unauthorizedAccessExceptionList = new List<TextBox>();
            List<TextBox> invalidOperationExceptionList = new List<TextBox>();

            foreach (var tb in TextBoxesList)
            {
                try
                {
                    //This will raise an exception if the path is read only or the user
                    //does not have access to view the permissions.
                    Directory.GetAccessControl(tb.Text);
                }
                catch (UnauthorizedAccessException)
                {
                    unauthorizedAccessExceptionList.Add(tb);
                }
                catch (InvalidOperationException)
                {
                    invalidOperationExceptionList.Add(tb);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected exception occured when checking for access rights in" + tb.Text + ":\n" + ex.Message + "\n\nOperation failed in " + System.Reflection.MethodBase.GetCurrentMethod().Name + " method.", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }

            if (unauthorizedAccessExceptionList.Count < 1 && invalidOperationExceptionList.Count < 1)
            {
                return true;
            }
            else
            {
                StringBuilder errorMessage = new StringBuilder("There were network errors:" + Environment.NewLine + "----------------------------" + Environment.NewLine);

                if (unauthorizedAccessExceptionList.Count == 1)
                {
                    errorMessage.AppendLine("You are not authorized to access the folder for " + NameReplace(unauthorizedAccessExceptionList[0]) + Environment.NewLine);
                }
                else if (unauthorizedAccessExceptionList.Count > 1)
                {
                    errorMessage.AppendLine("You are not authorized to access the folders for:" + Environment.NewLine);
                    foreach (var tb in unauthorizedAccessExceptionList)
                    {
                        errorMessage.AppendLine(NameReplace(tb));
                    }
                    errorMessage.AppendLine();
                }

                if (invalidOperationExceptionList.Count == 1)
                {

                    errorMessage.AppendLine("Invalid operation occured when checking for access rights for " + NameReplace(invalidOperationExceptionList[0]));
                }
                else if (invalidOperationExceptionList.Count > 1)
                {
                    errorMessage.AppendLine("Invalid operation occured when checking for access rights for:" + Environment.NewLine);
                    foreach (var tb in invalidOperationExceptionList)
                    {
                        errorMessage.AppendLine(NameReplace(tb));
                    }
                    errorMessage.AppendLine();
                }

                MessageBox.Show(errorMessage.ToString(), "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
            foreach (var tb in TextBoxesList)
            {
                //If there is a task name provided we add a backslash, otherwise the QA Folder path's
                //last backslash will suffice.
                string destinationPath = tbQAFolderPath.Text + ((tbTaskName.Text.Length > 0) ? tbTaskName.Text + "\\" : string.Empty);

                //We set the name of the destination folder, depending
                //on the TextBox we are iterating over.
                switch (tb.Name)
                {
                    case "tbECheckPath":
                        destinationPath += "E-Check\\";
                        break;
                    case "tbCorePath":
                        destinationPath += "E-CheckCore\\";
                        break;
                    case "tbServicePath":
                        destinationPath += "E-CheckService\\";
                        break;
                    case "tbQAFolderPath":
                        continue;
                    default:
                        break;
                }

                //These variables will hold the current source and target path of the "for" iteration.
                //They will be used to show more information in the exception catching.
                string sourceDir = ErrorBeforeDirectoryLoop;
                string targetDir = ErrorBeforeDirectoryLoop;
                try
                {
                    //First we create the directory structure.
                    foreach (string dirPath in Directory.GetDirectories(tb.Text, "*", SearchOption.AllDirectories))
                    {
                        sourceDir = dirPath;
                        targetDir = dirPath.Replace(tb.Text, destinationPath);
                        Directory.CreateDirectory(targetDir);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderDirectory("The caller does not have the required permission for \"" + targetDir + "\".", sourceDir, targetDir, ex), "Unauthorized Access Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderDirectory("The path passed for directory creation is null.", sourceDir, targetDir, ex), "Argument Null Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderDirectory("The path passed for directory creation is invalid.", sourceDir, targetDir, ex), "Argument Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (PathTooLongException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderDirectory("Cannot create target directory, path is too long.", sourceDir, targetDir, ex), "Path Too Long Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (DirectoryNotFoundException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderDirectory("The path passed for directory creation could not be found.", sourceDir, targetDir, ex), "Directory Not Found Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (IOException ex)
                {
                    //IO Exception can be either the passed path is a file or the network name is not known.
                    //Since we have previous checks in place to make sure the path is a directory,
                    //the second possible error is shown.
                    MessageBox.Show(ExceptionMessageBuilderDirectory("The network name is not known.", sourceDir, targetDir, ex), "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (NotSupportedException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderDirectory("The path passed contains an illegal colon character.", sourceDir, targetDir, ex), "Not Supported Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderDirectory("Unexpected exception occurred:" + Environment.NewLine + ex.Message, sourceDir, targetDir, ex), "Unexpected Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //These variables will hold the current source and target path of the "for" iteration.
                //They will be used to show more information in the exception catching.
                string sourceFile = ErrorBeforeFileLoop;
                string targetFileDir = ErrorBeforeFileLoop;
                try
                {
                    //We copy all files, overwriting any existing ones.
                    foreach (string filePath in Directory.GetFiles(tb.Text, "*", SearchOption.AllDirectories))
                    {
                        sourceFile = filePath;
                        targetFileDir = filePath.Replace(tb.Text, destinationPath);
                        File.Copy(filePath, targetFileDir, true);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderFile("The caller does not have the required permission for \"" + targetFileDir + "\".", sourceFile, targetFileDir, ex), "Unauthorized Access Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderFile("Either the source or destination file paths are null.", sourceFile, targetFileDir, ex), "Argument Null Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderFile("Either the source or destination file paths are invalid.", sourceFile, targetFileDir, ex), "Argument Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (PathTooLongException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderFile("Either the source or destination file paths are too long.", sourceFile, targetFileDir, ex), "Path Too Long Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (DirectoryNotFoundException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderFile("Either the source or destination file paths could not be found.", sourceFile, targetFileDir, ex), "Directory Not Found Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (NotSupportedException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderFile("Either the source or destination file paths are invalid.", sourceFile, targetFileDir, ex), "Not Supported Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderFile("\"" + sourceFile + "\" was not found.", sourceFile, targetFileDir, ex), "File Not Found Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderFile("An I/O error has occurred.", sourceFile, targetFileDir, ex), "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ExceptionMessageBuilderFile("An unexpected exception has occurred:" + Environment.NewLine + ex.Message, sourceFile, targetFileDir, ex), "Unexpected Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (cbBatchFile.Checked == true)
            {
                string createBatchResult = CreateBatchFile();
                if (createBatchResult == string.Empty)
                {
                    MessageBox.Show("Copy operation and batch file generation completed successfully!", "Operation success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Copy operation completed successfully, but batch file generation failed with the following error:" + Environment.NewLine + createBatchResult, "Operation partial success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Copy operation completed successfully!", "Operation success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Creates a StringBuilder and formats a more user-friendly message to display
        /// to the user when an exception occurrs in the directory structure creation method.
        /// </summary>
        /// <param name="message">A custom message to display.</param>
        /// <param name="sourceDir">The path to the source directory.</param>
        /// <param name="targetDir">The path to the target directory.</param>
        /// <param name="exception">The exception thrown.</param>
        /// <returns>The formatted message.</returns>
        private string ExceptionMessageBuilderDirectory(string message, string sourceDir, string targetDir, Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            if (sourceDir == ErrorBeforeDirectoryLoop || targetDir == ErrorBeforeDirectoryLoop)
            {
                sb.AppendLine(ErrorBeforeDirectoryLoop);
                sb.AppendLine("Exception message:");
                sb.Append(exception.Message);
            }
            else
            {
                sb.AppendLine("An exception occurred while creating directory structure." + Environment.NewLine);
                sb.AppendLine(message + Environment.NewLine);
                sb.AppendLine("Additional information:");
                sb.AppendLine("Source directory: " + sourceDir);
                sb.Append("Target directory: " + targetDir);
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// Creates a StringBuilder and formats a more user-friendly message to display
        /// to the user when an exception occurrs in the file copy method.
        /// </summary>
        /// <param name="message">A custom message to display.</param>
        /// <param name="sourceFile">The path to the source file.</param>
        /// <param name="targetFileDir">The path to the target directory.</param>
        /// <param name="exception">The exception thrown.</param>
        /// <returns>The formatted message.</returns>
        private string ExceptionMessageBuilderFile(string message, string sourceFile, string targetFileDir, Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            if (sourceFile == ErrorBeforeFileLoop || targetFileDir == ErrorBeforeFileLoop)
            {
                sb.AppendLine(ErrorBeforeFileLoop);
                sb.AppendLine("Exception message:");
                sb.Append(exception.Message);
            }
            else
            {
                sb.AppendLine("An exception occurred while copying files." + Environment.NewLine);
                sb.AppendLine(message + Environment.NewLine);
                sb.AppendLine("Additional information:");
                sb.AppendLine("Source file path: " + sourceFile);
                sb.Append("Target file path: " + targetFileDir);
            }

            return sb.ToString();
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
            TextBox textBox = tlpMain.Controls.Find(control.Name.Replace("btn", "tb").Replace("Browse", "Path"), false).OfType<TextBox>().FirstOrDefault();
            folderBrowserDialog.SelectedPath = textBox.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) textBox.Text = folderBrowserDialog.SelectedPath.EndsWith("\\") ? folderBrowserDialog.SelectedPath : folderBrowserDialog.SelectedPath + "\\";
        }

        /// <summary>
        /// Reads the given file line by line and attempts to retrieve the values for all TextBoxes.
        /// It will display a MessageBox with all TextBoxes for which it could not find a value.
        /// </summary>
        /// <param name="filePath">The full path to the save file.</param>
        private void LoadFile(string filePath)
        {
            //We will use this list to tell if a value for a TextBox was missing in the save file.
            List<TextBox> notFoundBoxes = new List<TextBox>();
            notFoundBoxes.AddRange(TextBoxesList);
            string line;
            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        try
                        {
                            //We get the substring from the beginning of the line to the first occurance of our separator. That
                            //should be the name of one of our TextBoxes. If we can match the substring to a TextBox that means
                            //we have a value for it and we take it out of the list "notFoundBoxes".
                            //Thus in the end we have a list of only the TextBoxes we couldn't find a value for.
                            TextBox tb = tlpMain.Controls.Find(line.Substring(0, line.IndexOf(Separator)), false).OfType<TextBox>().FirstOrDefault();
                            if (tb == null) continue;
                            tb.Text = line.Substring(line.IndexOf(Separator) + 2);
                            notFoundBoxes.Remove(tb);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An unexpected exception has occurred while reading the save file:\n" + ex.Message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw;
                        }
                    }

                    //For user-friendlyness-ness-ness-ness we format the shown error in singular or plural case.
                    if (notFoundBoxes.Count == 1)
                    {
                        MessageBox.Show("The path for " + NameReplace(notFoundBoxes[0]) + " could not be found in the file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (notFoundBoxes.Count > 1)
                    {
                        StringBuilder stringBuilder = new StringBuilder("The paths for the following TextBoxes:" + Environment.NewLine + Environment.NewLine);
                        foreach (var tb in notFoundBoxes)
                        {
                            stringBuilder.AppendLine(NameReplace(tb));
                        }
                        stringBuilder.Append(Environment.NewLine + "could not be found in the save file.");
                        MessageBox.Show(stringBuilder.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("A null argument has been passed to the load method.", "Null argument exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected exception has occurred while trying to fix illegal colon characters:\n" + ex.Message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
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

        /// <summary>
        /// Iterates through a list of TextBoxes' text properties and removes all colon
        /// characters, except the first one.
        /// </summary>
        /// <param name="list">A list of TextBoxes whose text properties should be fixed.</param>
        private void FixColons(List<TextBox> list)
        {
            try
            {
                foreach (var tb in list)
                {
                    int firstColon = tb.Text.IndexOf(':');
                    tb.Text = tb.Text.Replace(":", "");
                    tb.Text = tb.Text.Insert(firstColon, ":");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected exception has occurred while trying to fix illegal colon characters:\n" + ex.Message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Iterates through a list of TextBoxes' text properties and replaces all occurances of more
        /// than one consecutive backslash character with just a single backslash character.
        /// </summary>
        /// <param name="list">A list of TextBoxes whose text properties should be fixed.</param>
        private void FixBackslashes(List<TextBox> list)
        {
            Regex regex = new Regex(@"[\\]{2,}");
            try
            {
                foreach (var tb in list)
                {
                    tb.Text = tb.Text.Substring(0,1) + regex.Replace(tb.Text.Substring(1), "\\");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected exception has occurred while trying to fix repeated backslash characters:\n" + ex.Message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Creates a *.cmd file that will start all needed services
        /// and the GUI for E-Check.
        /// </summary>
        /// <returns>An empty string if generation was successful
        /// or the error message with which it failed.</returns>
        private string CreateBatchFile()
        {
            string targetPath = tbQAFolderPath.Text + tbTaskName.Text;
            try
            {
                using (StreamWriter sw = new StreamWriter(targetPath + "\\Start E-Check.ps1"))
                {
                    sw.WriteLine("start \"" + targetPath + "\\E-CheckCore\\E-CheckCoreConsoleHost.exe" + '\"');
                    sw.WriteLine("timeout 10");
                    sw.WriteLine("start \"" + targetPath + "\\E-CheckService\\ServiceHostNew.exe" + '\"');
                    sw.WriteLine("timeout 3");
                    sw.WriteLine("start \"" + targetPath + "\\E-Check\\ECheckERP.exe" + '\"');
                }
                return string.Empty;
            }
            catch (DirectoryNotFoundException)
            {
                return "The path specified for the batch file \"" + targetPath + "\" could not be found.";
            }
            catch (ArgumentNullException)
            {
                return "The path specified for the batch file is null.";
            }
            catch (ArgumentException)
            {
                return "The path specified for the batch file is invalid.";
            }
            catch (PathTooLongException)
            {
                return "The path specified for the batch file is too long.";
            }
            catch (IOException)
            {
                return "The path specified for the batch file includes an incorrect or invalid syntax.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}