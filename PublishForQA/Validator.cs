using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PublishForQA
{
    static class Validator
    {
        /// <summary>
        /// A reference to the main form.
        /// </summary>
        static FormPublisher formPublisher = Globals.MainForm;
        /// <summary>
        /// A list of all E-Check debug folder text boxes on the form.
        /// </summary>
        static List<TextBox> debugTextBoxes = Globals.DebugTextBoxesList;
        static FormValidationErrors validationErrorsForm = new FormValidationErrors();
        static TableLayoutPanel tlpMain = (TableLayoutPanel)validationErrorsForm.Controls[0];

        public static void Validate(List<TextBox> textBoxes)
        {
            List<DirectoryInfo> directories = new List<DirectoryInfo>();

            foreach (TextBox textBox in textBoxes)
            {
                directories.Add(new DirectoryInfo(textBox.Text)); // [???] this method needs to be finished by making it validate all possible pre-execution validatable exceptions.
            }

            CheckPathTooLongException(directories);

            validationErrorsForm.ShowDialog();
        }

        /// <summary>
        /// Checks if the longest file system entry path, that would result from
        /// the copy operation, would be too long and thus cause a "PathTooLongException".
        /// </summary>
        /// <param name="directories">A list of DirecotryInfo objects supplied for the copy operation.</param>
        static void CheckPathTooLongException(List<DirectoryInfo> directories)
        {
            (string destPath, string origPath, DirectoryInfo sourceDir) longestPathResult = GetLongestPath(directories);

            if (true)//longestPathResult.destPath.Length > 259) [???]
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

                ValidationCheck pathTooLong = new ValidationCheck(validationName, validationDetails.ToString());
                pathTooLong.Dock = DockStyle.None;
                tlpMain.Controls.Add(pathTooLong);
            }
        }

        /// <summary>
        /// Finds the longest file system entry path in the given DirectoryInfo array
        /// by searching all levels of the hierarchy.
        /// </summary>
        /// <param name="directories"></param>
        /// <returns></returns>
        static (string destPath, string origPath, DirectoryInfo sourceDir) GetLongestPath(List<DirectoryInfo> directories)
        {
            string longestPath = string.Empty;
            string origPath = string.Empty;
            DirectoryInfo sourceDir = null;

            // "E-CheckService\\" is added to the destination path, because it is the longest
            // of the directory names that will be used/created.
            string destinationPath = AdditionalFunctionality.SetDestinationPath() + "E-CheckService\\";

            foreach (var dir in directories)
            {
                string path = dir.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).OrderByDescending(d => d.FullName).FirstOrDefault().FullName;
                string destPath = path.Replace(dir.FullName, destinationPath);

                if (destPath.Length > longestPath.Length)
                {
                    longestPath = destPath;
                    origPath = path;
                    sourceDir = dir;
                }
            }

            return (longestPath, origPath, sourceDir);
        }
    }
}
