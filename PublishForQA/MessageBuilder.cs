using System;
using System.Text;

namespace PublishForQA
{
    /// <summary>
    /// Builds messages intended to be displayed in
    /// the case of an exception or error.
    /// </summary>
    public static class MessageBuilder
    {
        /// <summary>
        /// Displays a user-friendly message when an exception
        /// occurrs in the directory structure creation method.
        /// </summary>
        /// <param name="message">A custom message to display.</param>
        /// <param name="sourceDir">The path of the source directory.</param>
        /// <param name="targetDir">The path of the target directory.</param>
        /// <param name="exception">The exception thrown.</param>
        /// <returns>The formatted message.</returns>
        public static string DirectoryStructureException(string message, string sourceDir, string targetDir, Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            if (sourceDir == FormPublisher.ErrorBeforeDirectoryLoop || targetDir == FormPublisher.ErrorBeforeDirectoryLoop)
            {
                sb.AppendLine(FormPublisher.ErrorBeforeDirectoryLoop);
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
        /// Displays a user-friendly message when an
        /// exception occurrs in the file copy method.
        /// </summary>
        /// <param name="message">A custom message to display.</param>
        /// <param name="sourceFile">The path to the source file.</param>
        /// <param name="targetFileDir">The path to the target directory.</param>
        /// <param name="exception">The exception thrown.</param>
        /// <returns>The formatted message.</returns>
        public static string FileCopyException(string message, string sourceFile, string targetFileDir, Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            if (sourceFile == FormPublisher.ErrorBeforeFileLoop || targetFileDir == FormPublisher.ErrorBeforeFileLoop)
            {
                sb.AppendLine(FormPublisher.ErrorBeforeFileLoop);
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
    }
}
