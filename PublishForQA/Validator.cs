using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static PublishForQA.Globals;

namespace PublishForQA
{
    static class Validator
    {
        /// <summary>
        /// A reference to the main form.
        /// </summary>
        static FormPublisher formPublisher = MainForm;
        static FormValidationErrors validationErrorsForm = new FormValidationErrors();
        /// <summary>
        /// The main TableLayoutPanel of the FormValidationErrors.
        /// </summary>
        static TableLayoutPanel tlpMain = (TableLayoutPanel)validationErrorsForm.Controls[0];

        // The following exceptions should be implemented as validation checks.
        #region To Implement
        //catch (ArgumentException ex)
        //{
        //    MessageBox.Show(ExceptionMessageBuilder.Directory("The path passed for directory creation is invalid.", sourceDir, targetDir, ex), "Argument Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return false;
        //}
        //catch (DirectoryNotFoundException ex)
        //{
        //    MessageBox.Show(ExceptionMessageBuilder.Directory("The path passed for directory creation could not be found.", sourceDir, targetDir, ex), "Directory Not Found Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return false;
        //}
        //catch (FileNotFoundException ex)
        //{
        //    MessageBox.Show(ExceptionMessageBuilder.File("\"" + sourceFile + "\" was not found.", sourceFile, targetFileDir, ex), "File Not Found Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return false;
        //}
        //catch (IOException ex)
        //{
        //    // IO Exception can be either the passed path is a file or the network name is not known.
        //    // Since we have previous checks in place to make sure the path is a directory,
        //    // the second possible error is shown.
        //    MessageBox.Show(ExceptionMessageBuilder.Directory("The network name is not known.", sourceDir, targetDir, ex), "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return false;
        //}
        //catch (NotSupportedException ex)
        //{
        //    MessageBox.Show(ExceptionMessageBuilder.Directory("The path passed contains an illegal colon character.", sourceDir, targetDir, ex), "Not Supported Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return false;
        //}
    #endregion

        public static void Validate()
        {
            CheckPermission(MainForm.tbQAFolderPath.Text);
            CheckPathTooLongException();

            tlpMain.Controls.Add(new Panel()); // A control used for padding, so the last ControlValidationCheck can collapse it's details.
            validationErrorsForm.ShowDialog();
        }

        /// <summary>
        /// Checks if the program would be authorized to copy files to the destination.
        /// </summary>
        /// <param name="directory">The destination directory of the copy operation.</param>
        static void CheckPermission(string directory)
        {
            // A check if the directory path ends with a '\'.
            string path = directory + (directory.EndsWith(@"\") ? "temp" : @"\temp");

            try
            {
                File.WriteAllBytes(path, new byte[0]);
                File.Delete(path);
            }
            catch (UnauthorizedAccessException)
            {
                string validationName = "Insufficient rights";
                StringBuilder validationDetails = new StringBuilder();

                validationDetails.AppendLine($"The program does not have sufficient rights to create files in the directory \"{directory}\".");
                validationDetails.AppendLine();
                validationDetails.AppendLine("This may be caused by a lack of write rights of your account, group, machine or a combination of those.");

                AddControlToForm(validationName, validationDetails.ToString(), ErrorIcon.Error);
            }
            catch (Exception ex)
            {
                string validationName = "Unexpected exception";
                StringBuilder validationDetails = new StringBuilder();

                validationDetails.AppendLine($"The program failed to verify if there are sufficient rights to copy files to \"{directory}\".");
                validationDetails.AppendLine();
                validationDetails.AppendLine("An unexpected exception was encountered:");
                validationDetails.AppendLine();
                validationDetails.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    validationDetails.AppendLine();
                    validationDetails.AppendLine("Inner exception:");
                    validationDetails.AppendLine(ex.InnerException.Message);
                }

                AddControlToForm(validationName, validationDetails.ToString(), ErrorIcon.Error);
            }
        }

        static void CheckDirectoryPaths(List<DirectoryInfo> directories)
        {
            foreach (var dir in directories)
            {

            }
        }

        /// <summary>
        /// Checks if the longest file system entry path, that would result from
        /// the copy operation, would be too long and thus cause a "PathTooLongException".
        /// </summary>
        /// <param name="directories">A list of DirecotryInfo objects supplied for the copy operation.</param>
        static void CheckPathTooLongException()
        {
            (string destPath, string origPath) longestPathResult = GetLongestPath();

            if (longestPathResult.destPath.Length > 259)
            {
                string validationName = null;
                StringBuilder validationDetails = new StringBuilder();

                validationName = "Path is too long";

                if (Directory.Exists(longestPathResult.origPath))
                {
                    validationDetails.AppendLine("The copy operation cannot be performed, because the source directories contain a folder, whose path will exceed the maximum length allowed by the system when copied.");
                    validationDetails.AppendLine();
                    validationDetails.AppendLine("The folder in question is:");
                }
                else if (File.Exists(longestPathResult.origPath))
                {
                    validationDetails.AppendLine("The copy operation cannot be performed, because the source directories contain a file, whose path will exceed the maximum length allowed by the system when copied.");
                    validationDetails.AppendLine();
                    validationDetails.AppendLine("The file in question is:");
                }
                else
                {
                    // This should never happen. If it does - something has gone horribly wrong and the operation must be halted in such a rude manner.
                    throw new Exception("An unexprected exception occurred, because the \"CheckPathTooLongException\" validation found a path that is too long but it could not determine if it is a file or directory.", new Exception($"Path that caused the exception: {longestPathResult.origPath}"));
                }

                validationDetails.AppendLine(longestPathResult.origPath);
                validationDetails.AppendLine();
                validationDetails.AppendLine("It will cause a \"PathTooLongException\" when copied to:");
                validationDetails.AppendLine(longestPathResult.destPath);

                AddControlToForm(validationName, validationDetails.ToString(), ErrorIcon.Error);
            }
        }

        /// <summary>
        /// Finds the longest file system entry path in the given DirectoryInfo array
        /// by searching all levels of the hierarchy.
        /// </summary>
        /// <param name="directories"></param>
        /// <returns></returns>
        static (string destPath, string origPath) GetLongestPath()
        {
            string longestPath = string.Empty;
            string origPath = string.Empty;

            // "E-CheckService\\" is added to the destination path, because it is the longest
            // of the directory names that will be used/created.
            string destinationPath = AdditionalFunctionality.SetDestinationPath() + "E-CheckService\\";

            foreach (TextBox tb in DebugTextBoxesList)
            {
                string path = Directory.EnumerateFileSystemEntries(tb.Text).OrderByDescending(d => d.Length).FirstOrDefault();
                string destPath = path.Replace(tb.Text + "\\", destinationPath);

                if (destPath.Length > longestPath.Length)
                {
                    longestPath = destPath;
                    origPath = path;
                }
            }

            return (longestPath, origPath);
        }

        /// <summary>
        /// Adds a "ControlValidationCheck" with the given parameters to the main "TableLayoutPanel" of the "FormValidationErrors".
        /// </summary>
        /// <param name="validationName">The name or title of the failed validation.</param>
        /// <param name="validationDetails">The details on why the validation failed.</param>
        /// <param name="errorIcon">The type of icon to dispay.</param>
        static void AddControlToForm(string validationName, string validationDetails, ErrorIcon errorIcon)
        {
            ControlValidationCheck pathTooLong = new ControlValidationCheck(validationName, validationDetails, errorIcon);
            tlpMain.Controls.Add(pathTooLong);
        }
    }
}
