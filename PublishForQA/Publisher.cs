using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PublishForQA
{
    public static class Publisher
    {
        private static FormPublisher formPublisher = (FormPublisher)Form.ActiveForm;
        private static List<TextBox> allTextBoxes = formPublisher.AllTextBoxesList;
        private static List<TextBox> debugTextBoxes = formPublisher.DebugTextBoxesList;

        static Publisher()
        {
            
        }

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
                                 CopyFilesAndDirectories();
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
        public static bool NotEmpty()
        {
            //First we check if the "QA Folder" TextBox is empty.
            //Since it is mandatory we alert the user if it is.
            if (formPublisher.tbQAFolderPath.Text.Length < 1)
            {
                MessageBox.Show("No value provided for your QA folder.\nIt is mandatory, operation cannot continue.", "No QA Folder entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Then we add all TextBoxes with an empty Text property to a list
            //that will be used to display a warning and manipulate them further.
            List<TextBox> tbNoValueList = new List<TextBox>();
            foreach (var tb in debugTextBoxes)
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
                DialogResult confirm = MessageBox.Show(StringOperations.NameReplace(tbNoValueList[0]) + " is empty.\n\nDo you wish to proceed without it?", "Empty value", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    allTextBoxes = allTextBoxes.Except(tbNoValueList).ToList();
                    debugTextBoxes = debugTextBoxes.Except(tbNoValueList).ToList();
                    return true;
                }
            }
            else if (tbNoValueList.Count > 1)
            {
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
                    allTextBoxes = allTextBoxes.Except(tbNoValueList).ToList();
                    debugTextBoxes = debugTextBoxes.Except(tbNoValueList).ToList();
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
            foreach (var tb in allTextBoxes)
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
                DialogResult fixPath = MessageBox.Show("The path of " + StringOperations.NameReplace(tbIllegalColonList[0]) + " looks illegal as it contains a ':' character where it shouldn't and thus copying cannot continue.\nWould you like to fix it by removing all ':' characters but the first one and continue?", "Path warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                stringBuilder.Append(Environment.NewLine + "Copying cannot proceed like this.\nWould you like to fix it by removing all ':' characters in each path but the first one and continue?");
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
            foreach (var tb in debugTextBoxes)
            {
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
            foreach (var tb in allTextBoxes)
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
                    MessageBox.Show("The directory for " + StringOperations.NameReplace(tbDoesNotExistList[0]) + " does not exist.\nPlease check that the path is correct.", "Path error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                stringBuilder.Append(Environment.NewLine + "Please check that the paths are correct.");
                if (tbDoesNotExistList.Contains(formPublisher.tbQAFolderPath)) stringBuilder.AppendLine(Environment.NewLine + "The QA Folder can be automatically created but the other paths need to be corrected first.");
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

            foreach (var tb in allTextBoxes)
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

        /// <summary>
        /// Recreates the directory structure at the target location and copies all files from the source recursively.
        /// </summary>
        public static void CopyFilesAndDirectories()
        {
            foreach (var tb in debugTextBoxes)
            {
                //If there is a task name provided we add a backslash, otherwise the QA Folder path's
                //last backslash will suffice.
                string destinationPath = formPublisher.tbQAFolderPath.Text + ((formPublisher.tbTaskName.Text.Length > 0) ? formPublisher.tbTaskName.Text + "\\" : string.Empty);

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

                if (!CreateDirectoryStructure(tb.Text, destinationPath)) return;
                if (!CopyFiles(tb.Text, destinationPath)) return;
            }

            if (formPublisher.cbBatchFile.Checked == true)
            {
                string createBatchResult = AdditionalFunctionality.CreateBatchFile();
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
        /// Gets all the directories in a target path and recreates the same
        /// directory structure in the destination path.
        /// </summary>
        /// <param name="sourcePath">The path from which to read the directory structure.</param>
        /// <param name="destinationPath">The path where to recreate the directory structure.</param>
        /// <returns>"True" if the operation was successful, "false" if an exception was raised.</returns>
        public static bool CreateDirectoryStructure(string sourcePath, string destinationPath)
        {
            //These variables will hold the current source and target path of the "for" iteration.
            //They will be used to show more information in the exception catching.
            string sourceDir = FormPublisher.ErrorBeforeDirectoryLoop;
            string targetDir = FormPublisher.ErrorBeforeDirectoryLoop;
            try
            {
                //First we create the directory structure.
                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                {
                    sourceDir = dirPath;
                    targetDir = dirPath.Replace(sourcePath, destinationPath);
                    Directory.CreateDirectory(targetDir);
                }
                return true;
            }
            #region catch block
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.Directory("The caller does not have the required permission for \"" + targetDir + "\".", sourceDir, targetDir, ex), "Unauthorized Access Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.Directory("The path passed for directory creation is null.", sourceDir, targetDir, ex), "Argument Null Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.Directory("The path passed for directory creation is invalid.", sourceDir, targetDir, ex), "Argument Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (PathTooLongException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.Directory("Cannot create target directory, path is too long.", sourceDir, targetDir, ex), "Path Too Long Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.Directory("The path passed for directory creation could not be found.", sourceDir, targetDir, ex), "Directory Not Found Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (IOException ex)
            {
                //IO Exception can be either the passed path is a file or the network name is not known.
                //Since we have previous checks in place to make sure the path is a directory,
                //the second possible error is shown.
                MessageBox.Show(ExceptionMessageBuilder.Directory("The network name is not known.", sourceDir, targetDir, ex), "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.Directory("The path passed contains an illegal colon character.", sourceDir, targetDir, ex), "Not Supported Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.Directory("Unexpected exception occurred:" + Environment.NewLine + ex.Message, sourceDir, targetDir, ex), "Unexpected Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            #endregion
        }

        /// <summary>
        /// Copies all files from the target path to the destination path,
        /// overriding any existing ones.
        /// </summary>
        /// <param name="sourcePath">The path from which to copy all the files.</param>
        /// <param name="destinationPath">The path where to copy all the files.</param>
        /// <returns>"True" if the operation was successful, "false" if an exception was raised.</returns>
        public static bool CopyFiles(string sourcePath, string destinationPath)
        {
            //These variables will hold the current source and target path of the "for" iteration.
            //They will be used to show more information in the exception catching.
            string sourceFile = FormPublisher.ErrorBeforeFileLoop;
            string targetFileDir = FormPublisher.ErrorBeforeFileLoop;
            try
            {
                //We copy all files, overwriting any existing ones.
                foreach (string filePath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                {
                    sourceFile = filePath;
                    targetFileDir = filePath.Replace(sourcePath, destinationPath);
                    File.Copy(filePath, targetFileDir, true);
                }
                return true;
            }
            #region catch block
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.File("The caller does not have the required permission for \"" + targetFileDir + "\".", sourceFile, targetFileDir, ex), "Unauthorized Access Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.File("Either the source or destination file paths are null.", sourceFile, targetFileDir, ex), "Argument Null Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.File("Either the source or destination file paths are invalid.", sourceFile, targetFileDir, ex), "Argument Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (PathTooLongException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.File("Either the source or destination file paths are too long.", sourceFile, targetFileDir, ex), "Path Too Long Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.File("Either the source or destination file paths could not be found.", sourceFile, targetFileDir, ex), "Directory Not Found Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.File("Either the source or destination file paths are invalid.", sourceFile, targetFileDir, ex), "Not Supported Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.File("\"" + sourceFile + "\" was not found.", sourceFile, targetFileDir, ex), "File Not Found Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.File("An I/O error has occurred.", sourceFile, targetFileDir, ex), "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.File("An unexpected exception has occurred:" + Environment.NewLine + ex.Message, sourceFile, targetFileDir, ex), "Unexpected Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            #endregion
        }
    }
}
