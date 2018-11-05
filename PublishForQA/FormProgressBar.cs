using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PublishForQA
{
    public partial class FormProgressBar : Form
    {
        /// <summary>
        /// A reference to FormPublisher.
        /// </summary>
        static FormPublisher formPublisher = (FormPublisher)Form.ActiveForm;
        static FormProgressBar formProgressBar;
        /// <summary>
        /// A list of all E-Check debug folder text boxes on the form.
        /// </summary>
        static List<TextBox> debugTextBoxes = formPublisher.DebugTextBoxesList;
        static BackgroundWorker backgroundWorker;
        static DoWorkEventArgs WorkArgs;
        static int TotalOperationsCount;
        static int CurrentOpeartionCount;

        public FormProgressBar()
        {
            backgroundWorker = new BackgroundWorker();
            TotalOperationsCount = 0;
            CurrentOpeartionCount = 0;
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
            WorkArgs = e;
            pbMain.Maximum = GetOperationsCount();
            CopyFilesAndDirectories();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbMain.Value = e.ProgressPercentage;
            TaskbarProgress.SetState(formPublisher.Handle, TaskbarProgress.TaskbarStates.Indeterminate);
            TaskbarProgress.SetValue(formPublisher.Handle, pbMain.Value, pbMain.Maximum);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                TaskbarProgress.SetState(formPublisher.Handle, TaskbarProgress.TaskbarStates.Error);
                MessageBox.Show("An error occurred during copying:" + Environment.NewLine + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cleanup();
            }
            else if (e.Cancelled)
            {
                TaskbarProgress.SetState(formPublisher.Handle, TaskbarProgress.TaskbarStates.Paused);
                MessageBox.Show("Copy operation aborted.", "Abort", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cleanup();
            }
            else
            {
                TaskbarProgress.SetState(formPublisher.Handle, TaskbarProgress.TaskbarStates.Normal);
                MessageBox.Show("Copy operation completed successfully!", "Operation success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cleanup();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            TaskbarProgress.SetState(formPublisher.Handle, TaskbarProgress.TaskbarStates.Paused);
            TaskbarProgress.SetValue(formPublisher.Handle, 0, pbMain.Maximum);
            backgroundWorker.CancelAsync();
        }

        /// <summary>
        /// Checks if "CancellationPending" is true and if it is aborts the thread and sets
        /// the DoWorkEventArgs's Cancel property to true.
        /// </summary>
        static void CheckForCancel()
        {
            if (backgroundWorker.CancellationPending)
            {
                WorkArgs.Cancel = true;
                Thread.CurrentThread.Abort();
            }
        }

        /// <summary>
        /// Marks the BackgroundWorker to be disposed and closes the progress bar form.
        /// </summary>
        static void Cleanup()
        {
            TaskbarProgress.SetState(formPublisher.Handle, TaskbarProgress.TaskbarStates.NoProgress);
            backgroundWorker.Dispose();
            formProgressBar.Close();
        }

        /// <summary>
        /// Counts all the directories that need to be created and all the
        /// files that need to be copied from each designated folder.
        /// </summary>
        /// <returns></returns>
        public static int GetOperationsCount()
        {
            formProgressBar.lblCurrentOperation.Text = "Counting the total number of operations...";
            foreach (var tb in debugTextBoxes)
            {
                CheckForCancel();
                TotalOperationsCount += Directory.GetFiles(tb.Text, "*", SearchOption.AllDirectories).Length;
                CheckForCancel();
                TotalOperationsCount += Directory.GetDirectories(tb.Text, "*", SearchOption.AllDirectories).Length;
                formProgressBar.lblCurrentPath.Text = tb.Text;
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
                CheckForCancel();
                string destinationPath = AdditionalFunctionality.SetDestinationPath();

                // Sets the name of the destination folder, depending
                // on the TextBox being iterated over.
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

            // These variables will hold the current source and target path of the "for" iteration.
            // They will be used to show more information in the exception catching.
            string sourceDir = FormPublisher.ErrorBeforeDirectoryLoop;
            string targetDir = FormPublisher.ErrorBeforeDirectoryLoop;
            try
            {
                // First we create the directory structure.
                Directory.CreateDirectory(destinationPath);
                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                {
                    CheckForCancel();
                    sourceDir = dirPath;
                    targetDir = dirPath.Replace(sourcePath, destinationPath);
                    formProgressBar.lblCurrentPath.Text = targetDir;
                    Directory.CreateDirectory(targetDir);
                    CurrentOpeartionCount++;
                    backgroundWorker.ReportProgress(CurrentOpeartionCount);
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
                // IO Exception can be either the passed path is a file or the network name is not known.
                // Since we have previous checks in place to make sure the path is a directory,
                // the second possible error is shown.
                MessageBox.Show(ExceptionMessageBuilder.Directory("The network name is not known.", sourceDir, targetDir, ex), "IO Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show(ExceptionMessageBuilder.Directory("The path passed contains an illegal colon character.", sourceDir, targetDir, ex), "Not Supported Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
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
            // These variables will hold the current source and target path of the "for" iteration.
            // They will be used to show more information in the exception catching.
            // But first they are set to the string used to indicate an error before the loop.
            string sourceFile = FormPublisher.ErrorBeforeFileLoop;
            string targetFileDir = FormPublisher.ErrorBeforeFileLoop;
            try
            {
                // We copy all files, overwriting any existing ones.
                foreach (string filePath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                {
                    CheckForCancel();
                    sourceFile = filePath;
                    targetFileDir = filePath.Replace(sourcePath, destinationPath);
                    formProgressBar.lblCurrentPath.Text = filePath;
                    File.Copy(filePath, targetFileDir, true);
                    CurrentOpeartionCount++;
                    backgroundWorker.ReportProgress(CurrentOpeartionCount);
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
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
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
