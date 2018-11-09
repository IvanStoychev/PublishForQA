using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PublishForQA
{
    public static class AdditionalFunctionality
    {
        public static FormPublisher MainForm;

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
                Directory.CreateDirectory(MainForm.tbQAFolderPath.Text);
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
            eCheckDirectories.AddRange(allDirectories.Where(x => x.EndsWith("\\E-Check\\" + version)));
            coreDirectories.AddRange(allDirectories.Where(x => x.EndsWith("\\E-CheckCore")));

            //We check each subfolder of each directory found for the existance
            //of the mandatory "WinClient" and "AppServer" debug directories.
            foreach (var dir in eCheckDirectories)
            {
                eCheckResults.AddRange(Directory.EnumerateDirectories(dir).Where(x => Directory.Exists(x + @"\WinClient\E-Check\bin\Debug") && Directory.Exists(x + @"\AppServer\ServiceHostNew\ServiceHostNew\bin\Debug")).ToList());
            }
            foreach (var dir in coreDirectories)
            {
                coreResults.AddRange(Directory.EnumerateDirectories(dir).Where(x => Directory.Exists(x + @"\E-CheckCoreConsoleHost\bin\Debug")));
            }

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

        /// <summary>
        /// Returns the destination path for copying files.
        /// </summary>
        /// <returns>
        /// A string representing the destination for copying files,
        /// taking into account if a "Task Name" is provided.
        /// </returns>
        public static string SetDestinationPath()
        {
            // If there is a task name provided a backslash is added,
            // otherwise the QA Folder path's last backslash will suffice.
            if (MainForm.tbTaskName.Text.Length > 0)
            {
                return MainForm.tbQAFolderPath.Text + MainForm.tbTaskName.Text + "\\";
            }
            else
            {
                return MainForm.tbQAFolderPath.Text;
            }
        }
    }
}
