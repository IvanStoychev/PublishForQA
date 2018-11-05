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
            if (GetLongestPath(directories).Length > 260)
            {
                // [???] this method needs to be finished by initialising data for the error/exception message.
            }
        }

        /// <summary>
        /// Finds the longest file system entry path in the given DirectoryInfo array
        /// by searching all levels of the hierarchy.
        /// </summary>
        /// <param name="directories"></param>
        /// <returns></returns>
        static string GetLongestPath(List<DirectoryInfo> directories)
        {
            string longestPath = string.Empty;
            string destinationPath = AdditionalFunctionality.SetDestinationPath() + "E-CheckService\\";

            foreach (var dir in directories)
            {
                string path = dir.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).OrderByDescending(d => d.FullName).FirstOrDefault().FullName.Replace(dir.FullName, destinationPath);
                if (path.Length > longestPath.Length) longestPath = path;
            }

            return longestPath;
        }
    }
}
