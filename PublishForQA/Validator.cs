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
        /// A reference to FormPublisher.
        /// </summary>
        static FormPublisher formPublisher = (FormPublisher)Form.ActiveForm;
        /// <summary>
        /// A list of all E-Check debug folder text boxes on the form.
        /// </summary>
        static List<TextBox> debugTextBoxes = formPublisher.DebugTextBoxesList;
        static FormValidationErrors validationErrors = new FormValidationErrors();

        static void Validate(List<TextBox> textBoxes)
        {
            List<DirectoryInfo> directories = new List<DirectoryInfo>();

            foreach (TextBox textBox in textBoxes)
            {
                directories.Add(new DirectoryInfo(textBox.Text)); // [???] this method needs to be finished by making it validate all possible pre-execution validatable exceptions.
            }
        }

        /// <summary>
        /// Checks if the longest file system entry path, that would result from
        /// the copy operation, would be too long and thus cause a "PathTooLongException".
        /// </summary>
        /// <param name="directories">A list of DirecotryInfo objects supplied for the copy operation.</param>
        static void CheckPathTooLongException(List<DirectoryInfo> directories)
        {
            (string destPath, string origPath, DirectoryInfo sourceDir) longestPathResult = GetLongestPath(directories);

            if (longestPathResult.destPath.Length > 260)
            {
                string validationName = "Path is too long";
                StringBuilder validationDetails = new StringBuilder();
                validationDetails.AppendLine("The copy operation cannot be performed, because the source directories contain a file or folder, whose path will exceed the maximum ");

                ValidationCheck pathTooLongCheck = new ValidationCheck(validationName, validationDetails);
                // [???] this method needs to be finished by initialising data for the error/exception message.
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
