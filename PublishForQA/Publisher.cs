using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static PublishForQA.Enums;
using static PublishForQA.Globals;

namespace PublishForQA
{
    /// <summary>
    /// Class concerned with performing system validation and starting the copy operation.
    /// </summary>
    public static class Publisher
    {
        /// <summary>
        /// A reference to the main form.
        /// </summary>
        private static FormPublisher formPublisher = MainForm;

        /// <summary>
        /// A list for all TextBoxes that do not have a value.
        /// </summary>
        private static List<TextBox> tbNoValueList = new List<TextBox>();

        /// <summary>
        /// Publishes the currently chosen version for QA. It does so by first
        /// making all the verifications needed for correct copying and then
        /// copying all the files and directories to the given QA folder.
        /// </summary>
        public static void Publish()
        {
            //Subsequent checks before beginning the copy operation.
            //Ordered in this way for readability.
            if (NotEmpty())
                if (PathsAreLegal())
                    if (HasBinDebug())
                        if (DirectoriesExist())
                            if (HasNetworkAccess())
                            {
                                FormProgressBar formProgressBar = new FormProgressBar();
                                formProgressBar.ShowDialog();
                            }
        }

        /// <summary>
        /// Checks if any TextBox's value is empty and asks the user if he would
        /// like to omit it, if it is.
        /// </summary>
        /// <returns>
        /// "True" if all text boxes have values or the user decided to skip those
        /// that do not. "False" if there are empty values and the user does not wish to continue.
        /// </returns>
        /// <remarks>
        /// The QA folder is mandatory and cannot be omitted.
        /// </remarks>
        public static bool NotEmpty()
        {
            // First check if the "QA Folder" TextBox is empty.
            // Since it is mandatory - alert the user, if it is.
            if (formPublisher.tbQAFolderPath.Text.Length < 1)
            {
                MessageBox.Show("No value provided for your QA folder.\nIt is mandatory, operation cannot continue.", "No QA Folder entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Add all TextBoxes with an empty Text property to a list
            // that will be used to display a warning and manipulate them further.
            tbNoValueList = new List<TextBox>();
            foreach (var tb in DebugTextBoxesList)
            {
                if (tb.Text.Length < 1)
                {
                    tbNoValueList.Add(tb);
                }
            }

            // If there are no text boxes with no values - continue.
            if (tbNoValueList.Count == 0)
            {
                return true;
            }

            //For user-friendlyness-ness-ness-ness format the shown error in singular or plural case.
            if (tbNoValueList.Count == 1)
            {
                MessageBuilder.CreateMessage(StringOperations.NameReplace(tbNoValueList[0]) + " is empty.", MessageUserControlIcons.Warning, MessageUserControlButtons.None);
            }
            else if (tbNoValueList.Count > 1)
            {
                // TO DO [???]
                StringBuilder stringBuilder = new StringBuilder("The following text boxes are empty:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbNoValueList)
                {
                    stringBuilder.AppendLine(StringOperations.NameReplace(tb));
                }
                stringBuilder.Append(Environment.NewLine + "Do you wish to proceed without them?");
                DialogResult confirm = MessageBox.Show(stringBuilder.ToString(), "Empty value", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    AllTextBoxesList = AllTextBoxesList.Except(tbNoValueList).ToList();
                    DebugTextBoxesList = DebugTextBoxesList.Except(tbNoValueList).ToList();
                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether all paths have at most a single colon character or no more than one consecutive backslash
        /// character. If any path violates any of these conditions its TextBox is added to an appropriate list.
        /// Afterwards if the user chooses the program can fix any violations encountered.
        /// </summary>
        /// <returns>
        /// "True" if all paths contain no more than a single colon character per path, otherwise "False".
        /// </returns>
        public static bool PathsAreLegal()
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
            foreach (var tb in AllTextBoxesList)
            {
                //For clarity and "just in case", we add a slash at the end of paths that don't have one.
                if (!tb.Text.EndsWith("\\")) tb.Text = tb.Text + "\\";
                //If there is a colon character beyond index 1 of the string we add
                //the corresponding TextBox to the IllegalColonList list.
                if (tb.Text.LastIndexOf(':') > 1) tbIllegalColonList.Add(tb);
                //If there are two or more consecutive backslash characters beyond the start
                //of the string we add that TextBox to the IllegalBackslashList.
                if (Regex.IsMatch(tb.Text.Substring(1), @"[\\]{2,}")) tbIllegalBackslashList.Add(tb);
            }

            //If there are no text boxes with illegal paths we continue.
            if (tbIllegalColonList.Count == 0 && tbIllegalBackslashList.Count == 0)
            {
                return true;
            }

            if (tbIllegalColonList.Count == 1)
            {
                DialogResult fixPath = MessageBox.Show("The path of " + StringOperations.NameReplace(tbIllegalColonList[0]) + " looks illegal as it contains a ':' character where it shouldn't and thus copying cannot continue.\nWould you like to fix that and continue?", "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (fixPath == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    StringOperations.FixColons(tbIllegalColonList);
                }
            }
            else if (tbIllegalColonList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths look illegal because they contain a ':' character where they shouldn't:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbIllegalColonList)
                {
                    stringBuilder.AppendLine(StringOperations.NameReplace(tb));
                }
                stringBuilder.Append(Environment.NewLine + "Copying cannot proceed like this.\nWould you like to fix this problem in each path and continue?");
                DialogResult fixPath = MessageBox.Show(stringBuilder.ToString(), "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (fixPath == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    StringOperations.FixColons(tbIllegalColonList);
                }
            }

            if (tbIllegalBackslashList.Count == 1)
            {
                DialogResult fixPath = MessageBox.Show("The path of " + StringOperations.NameReplace(tbIllegalBackslashList[0]) + " looks illegal as it contains too many consecutive '\\' characters.\nWould you like to fix that by replacing them with a single '\\' character?" + "\n\nOperation will continue if either \"Yes\" or \"No\" are chosen.", "Path warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (fixPath == DialogResult.Cancel)
                {
                    return false;
                }
                else if (fixPath == DialogResult.Yes)
                {
                    StringOperations.FixBackslashes(tbIllegalBackslashList);
                }
            }
            else if (tbIllegalBackslashList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths look illegal because they contain too many consecutive '\\' characters:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbIllegalBackslashList)
                {
                    stringBuilder.AppendLine(StringOperations.NameReplace(tb));
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
                    StringOperations.FixBackslashes(tbIllegalBackslashList);
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
        public static bool HasBinDebug()
        {
            //And we check if the paths ends with in "bin\Debug" folder.
            List<TextBox> tbNoBinDebugList = new List<TextBox>();
            foreach (var tb in DebugTextBoxesList)
            {
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
                DialogResult confirm = MessageBox.Show("The path of " + StringOperations.NameReplace(tbNoBinDebugList[0]) + " does not end with a \"bin\\Debug\" folder.\nAre you sure you wish to proceed?", "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                    stringBuilder.AppendLine(StringOperations.NameReplace(tb));
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
        public static bool DirectoriesExist()
        {
            //This list will hold all text boxes whose listed directories do not exist.
            List<TextBox> tbDoesNotExistList = new List<TextBox>();

            //For each TextBox we check if its listed directory exists and add it to the list if it does not.
            foreach (var tb in AllTextBoxesList)
            {
                //A new task is started asynchronously that checks if the given directory exists.
                //If the task does not return a result after one second or returns that the
                //directory does not exist the TextBox with said directory path is added to the list.
                var task = new System.Threading.Tasks.Task<bool>(() => { return Directory.Exists(tb.Text); });
                task.Start();

                if (!(task.Wait(1000) && task.Result)) tbDoesNotExistList.Add(tb);
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
                if (tbDoesNotExistList[0] == formPublisher.tbQAFolderPath)
                {
                    DialogResult create = MessageBox.Show("The directory for " + StringOperations.NameReplace(tbDoesNotExistList[0]) + " does not exist.\nWould you like to create it?" + "\n\nOperation will continue if either \"Yes\" or \"No\" are chosen.", "Path error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                    if (create == DialogResult.Yes) //User chose to create the directory.
                    {
                        return AdditionalFunctionality.CreateQAFolder();
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
                    MessageBox.Show("The directory for " + StringOperations.NameReplace(tbDoesNotExistList[0]) + " does not exist.\nPlease, check that the path is correct.", "Path error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (tbDoesNotExistList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The directories for the following do not exist:" + Environment.NewLine + Environment.NewLine);
                foreach (var txtb in tbDoesNotExistList)
                {
                    stringBuilder.AppendLine(StringOperations.NameReplace(txtb));
                }
                stringBuilder.Append(Environment.NewLine + "Please, check that the paths are correct.");
                if (tbDoesNotExistList.Contains(formPublisher.tbQAFolderPath)) stringBuilder.AppendLine(Environment.NewLine + "The QA Folder can be automatically created but the other paths need to be corrected, first.");
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
        public static bool HasNetworkAccess()
        {
            List<TextBox> unauthorizedAccessExceptionList = new List<TextBox>();
            List<TextBox> invalidOperationExceptionList = new List<TextBox>();

            foreach (var tb in AllTextBoxesList)
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
                    MessageBox.Show("Unexpected exception occured when checking for access rights in \"" + tb.Text + "\":\n" + ex.Message + "\n\nOperation failed in " + System.Reflection.MethodBase.GetCurrentMethod().Name + " method.", "Critical error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
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
                    errorMessage.AppendLine("You are not authorized to access the folder for " + StringOperations.NameReplace(unauthorizedAccessExceptionList[0]) + Environment.NewLine);
                }
                else if (unauthorizedAccessExceptionList.Count > 1)
                {
                    errorMessage.AppendLine("You are not authorized to access the folders for:" + Environment.NewLine);
                    foreach (var tb in unauthorizedAccessExceptionList)
                    {
                        errorMessage.AppendLine(StringOperations.NameReplace(tb));
                    }
                    errorMessage.AppendLine();
                }

                if (invalidOperationExceptionList.Count == 1)
                {

                    errorMessage.AppendLine("Invalid operation occured when checking for access rights for " + StringOperations.NameReplace(invalidOperationExceptionList[0]));
                }
                else if (invalidOperationExceptionList.Count > 1)
                {
                    errorMessage.AppendLine("Invalid operation occured when checking for access rights for:" + Environment.NewLine);
                    foreach (var tb in invalidOperationExceptionList)
                    {
                        errorMessage.AppendLine(StringOperations.NameReplace(tb));
                    }
                    errorMessage.AppendLine();
                }

                MessageBox.Show(errorMessage.ToString(), "Network error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
    }
}
