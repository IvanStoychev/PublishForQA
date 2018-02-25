using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace PublishForQA
{
    public partial class FormProgressBar : Form
    {
        private static FormPublisher formPublisher = (FormPublisher)Form.ActiveForm;
        private static FormProgressBar formProgressBar;
        private static List<TextBox> debugTextBoxes = formPublisher.DebugTextBoxesList;
        private static BackgroundWorker backgroundWorker = new BackgroundWorker();
        private static int TotalOperationsCount;
        private static int DirectoryOpeartionCount;
        private static int FileOpeartionCount;

        public FormProgressBar()
        {
            TotalOperationsCount = 0;
            DirectoryOpeartionCount = 0;
            FileOpeartionCount = 0;
            InitializeComponent();
            formProgressBar = this;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            pbMain.Maximum = GetOperationsCount();
            CopyFilesAndDirectories();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbMain.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.Close();
            if (e.Error != null)
            {
                MessageBox.Show("An error occurred during copying:" + Environment.NewLine + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Copy operation aborted.", "Abort", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Copy operation completed successfully!", "Operation success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //backgroundWorker.CancelAsync();
            //backgroundWorker.Dispose();
        }

        public static int GetOperationsCount()
        {
            foreach (var tb in debugTextBoxes)
            {
                TotalOperationsCount += Directory.GetFiles(tb.Text, "*", SearchOption.AllDirectories).Length;
                TotalOperationsCount += Directory.GetDirectories(tb.Text, "*", SearchOption.AllDirectories).Length;
            }

            return TotalOperationsCount;
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
                    default:
                        break;
                }

                if (!CreateDirectoryStructure(tb.Text, destinationPath)) return;
                if (!CopyFiles(tb.Text, destinationPath)) return;
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
            formProgressBar.lblCurrentOperation.Text = "Creating directory structure...";
            //These variables will hold the current source and target path of the "for" iteration.
            //They will be used to show more information in the exception catching.
            string sourceDir = FormPublisher.ErrorBeforeDirectoryLoop;
            string targetDir = FormPublisher.ErrorBeforeDirectoryLoop;
            try
            {
                //First we create the directory structure.
                Directory.CreateDirectory(destinationPath);
                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                {
                    sourceDir = dirPath;
                    targetDir = dirPath.Replace(sourcePath, destinationPath);
                    formProgressBar.lblCurrentPath.Text = targetDir;
                    Directory.CreateDirectory(targetDir);
                    DirectoryOpeartionCount++;
                    backgroundWorker.ReportProgress(DirectoryOpeartionCount);
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
            formProgressBar.lblCurrentOperation.Text = "Copying files...";
            //These variables will hold the current source and target path of the "for" iteration.
            //They will be used to show more information in the exception catching.
            //But first they are set to the string used to indicate an error before the loop.
            string sourceFile = FormPublisher.ErrorBeforeFileLoop;
            string targetFileDir = FormPublisher.ErrorBeforeFileLoop;
            try
            {
                //We copy all files, overwriting any existing ones.
                foreach (string filePath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                {
                    sourceFile = filePath;
                    targetFileDir = filePath.Replace(sourcePath, destinationPath);
                    formProgressBar.lblCurrentPath.Text = filePath;
                    File.Copy(filePath, targetFileDir, true);
                    FileOpeartionCount++;
                    backgroundWorker.ReportProgress(DirectoryOpeartionCount + FileOpeartionCount);
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
