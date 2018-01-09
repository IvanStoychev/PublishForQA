using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PublishForQA
{
    public static class AdditionalFunctionality
    {
        public static FormPublisher FormPublisher { get; set; }

        static AdditionalFunctionality()
        {
            FormPublisher = (FormPublisher)Form.ActiveForm;
        }

        /// <summary>
        /// Attempts to create a folder at the designated QA Folder path.
        /// </summary>
        /// <returns>
        /// "True" if creation was successful, "False" otherwise.
        /// </returns>
        public static bool CreateQAFolder()
        {
            try
            {
                Directory.CreateDirectory(FormPublisher.tbQAFolderPath.Text);
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
        /// Creates a *.cmd file that will start all needed services
        /// and the GUI for E-Check.
        /// </summary>
        /// <returns>An empty string if generation was successful
        /// or the error message with which it failed.</returns>
        public static string CreateBatchFile()
        {
            string targetPath = FormPublisher.tbQAFolderPath.Text + FormPublisher.tbTaskName.Text;
            try
            {
                using (StreamWriter sw = new StreamWriter(targetPath + "\\Start E-Check.cmd"))
                {
                    sw.WriteLine("start \"\" \"" + ".\\E-CheckCore\\E-CheckCoreConsoleHost.exe" + '\"');
                    sw.WriteLine("timeout 10");
                    sw.WriteLine("start \"\" \"" + ".\\E-CheckService\\ServiceHostNew.exe" + '\"');
                    sw.WriteLine("timeout 3");
                    sw.WriteLine("start \"\" \"" + ".\\E-Check\\ECheckERP.exe" + '\"');
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

        /// <summary>
        /// Gets a list of all the folders of the fixed and removable drives on the system and searches through them for
        /// a folder named as the passed version parameter.
        /// </summary>
        /// <param name="version">The folder name of the E-Check version to search for</param>
        /// <remarks>The method also keeps a list of all folders that access was denied to</remarks>
        public static void Locate(string version)
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
                    return;
                }
            }

            //We filter all the directories by the version name and "E-CheckCore".
            eCheckDirectories.AddRange(allDirectories.Where(x => x.Contains("\\E-Check\\" + version + "\\")));
            coreDirectories.AddRange(allDirectories.Where(x => x.Contains("\\E-CheckCore\\")));

            //We get the filtered directories for which exist the vital debug folders.
            eCheckResults.AddRange(eCheckDirectories.Where(x => Directory.Exists(x + @"\WinClient\E-Check\bin\Debug\") && Directory.Exists(x + @"\AppServer\ServiceHostNew\ServiceHostNew\bin\Debug\")).ToList());
            coreResults.AddRange(coreDirectories.Where(x => Directory.Exists(x + @"\E-CheckCoreConsoleHost\bin\Debug\")).ToList());

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
    }
}
