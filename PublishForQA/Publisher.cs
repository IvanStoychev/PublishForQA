using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static PublishForQA.Enums;
using static PublishForQA.Globals;

namespace PublishForQA
{
    /// <summary>
    /// Class concerned with performing system validation and starting the copy operation.
    /// </summary>
    public static class Publisher
    {
        /// <summary>
        /// A reference to the main form.
        /// </summary>
        private static FormPublisher formPublisher = MainForm;

        /// <summary>
        /// A list for all "debug" TextBoxes that need validation.
        /// </summary>
        private static List<TextBox> validatingDebugTB = new List<TextBox>();

        /// <summary>
        /// A list for all TextBoxes that need validation.
        /// </summary>
        private static List<TextBox> validatingAllTB = new List<TextBox>();

        /// <summary>
        /// Publishes the currently chosen version for QA. It does so by first
        /// making all the verifications needed for correct copying and then
        /// copying all the files and directories to the given QA folder.
        /// </summary>
        public static void Publish()
        {
            validatingAllTB = AllTextBoxesList.ToList();
            validatingDebugTB = DebugTextBoxesList.ToList();

            if (NotEmpty() && PathsAreLegal() && HasBinDebug() && DirectoriesExist() && HasNetworkAccess())
            {
                FormProgressBar formProgressBar = new FormProgressBar();
                formProgressBar.ShowDialog();
            }
            else
            {
                MessageBuilder.ShowFormWarningErrors();
            }
        }

        /// <summary>
        /// Checks if any TextBox's value is empty and asks the user if he would
        /// like to omit it, if it is.
        /// </summary>
        /// <returns>
        /// "True" if all text boxes have values or the user decided to skip those
        /// that do not. "False" if there are empty values and the user does not wish to continue.
        /// </returns>
        /// <remarks>
        /// The QA folder is mandatory and cannot be omitted.
        /// </remarks>
        private static bool NotEmpty()
        {
            #region message parameters
            MessageUserControlIcons messageIcon = MessageUserControlIcons.Warning;
            MessageUserControlButtons messageButtons = MessageUserControlButtons.None;
            Func<bool> fixFunc = null;
            string message = null;
            #endregion

            // A list of all empty TextBoxes.
            List<TextBox> tbNoValueList = new List<TextBox>();

            // First check if the "QA Folder" TextBox is empty.
            // Since it is mandatory - alert the user, if it is.
            if (formPublisher.tbQAFolderPath.Text.Length < 1)
            {
                MessageBox.Show("No value provided for your QA folder.\nIt is mandatory, operation cannot continue.", "No QA Folder entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Add all TextBoxes with an empty Text property to a list
            // that will be used to display a warning and manipulate them further.
            foreach (var tb in validatingDebugTB)
            {
                if (tb.Text.Length < 1)
                {
                    tbNoValueList.Add(tb);
                }
            }

            // If there are no text boxes with no values - continue.
            if (tbNoValueList.Count == 0) return true;

            // If all "Debug" TextBoxes are empty there is nothing to copy - operation cannot continue.
            if (validatingDebugTB.SequenceEqual(tbNoValueList))
            {
                MessageBox.Show("All \"Debug\" text boxes are empty,\nno operation can be performed.", "No input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // For user-friendlyness-ness-ness-ness format the shown error in singular or plural case.
            if (tbNoValueList.Count == 1)
            {
                message = StringOperations.NameReplace(tbNoValueList[0]) + " is empty.";
                MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
            }
            else if (tbNoValueList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following text boxes are empty:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbNoValueList)
                {
                    stringBuilder.AppendLine(StringOperations.NameReplace(tb));
                }
                stringBuilder.Append(Environment.NewLine + "If you continue they will be ignored.");
                message = stringBuilder.ToString();
                MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
            }

            ExceptLists(tbNoValueList);
            return true;
        }

        /// <summary>
        /// Checks whether all paths have at most a single colon character or no more than one consecutive backslash
        /// character. If any path violates any of these conditions its TextBox is added to an appropriate list.
        /// Afterwards if the user chooses the program can fix any violations encountered.
        /// </summary>
        /// <returns>
        /// "True" if all paths contain no more than a single colon character per path, otherwise "False".
        /// </returns>
        private static bool PathsAreLegal()
        {
            #region message parameters
            MessageUserControlIcons messageIcon = MessageUserControlIcons.Error;
            MessageUserControlButtons messageButtons = MessageUserControlButtons.Fix;
            Func<bool> fixFunc = null;
            string message = null;
            #endregion

            // This list will hold all text boxes whose paths contain more than one colon character.
            List<TextBox> tbIllegalColonList = new List<TextBox>();
            // This list will hold all text boxes whose paths contain more than one consecutive backslash character.
            List<TextBox> tbIllegalBackslashList = new List<TextBox>();

            foreach (var tb in validatingAllTB)
            {
                // If there is a colon character beyond index 1 of the string we add
                // the corresponding TextBox to the IllegalColonList list.
                if (tb.Text.LastIndexOf(':') > 1) tbIllegalColonList.Add(tb);

                // If there are two or more consecutive backslash characters beyond the start
                // of the string we add that TextBox to the IllegalBackslashList.
                if (Regex.IsMatch(tb.Text.Substring(1), @"[\\]{2,}")) tbIllegalBackslashList.Add(tb);
            }

            // If there are no text boxes with illegal paths - continue.
            if (tbIllegalColonList.Count == 0 && tbIllegalBackslashList.Count == 0) return true;

            fixFunc = new Func<bool>(() => StringOperations.FixColons(tbIllegalColonList));
            if (tbIllegalColonList.Count == 1)
            {
                message = "The path of " + StringOperations.NameReplace(tbIllegalColonList[0]) + " looks illegal as it contains a ':' character where it shouldn't and thus copying cannot continue.\nWould you like to fix that and continue?";
                MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
            }
            else if (tbIllegalColonList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths look illegal because they contain a ':' character where they shouldn't:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbIllegalColonList)
                {
                    stringBuilder.AppendLine(StringOperations.NameReplace(tb));
                }
                stringBuilder.Append(Environment.NewLine + "Copying cannot proceed like this.\nWould you like to fix this problem in each path and continue?");
                message = stringBuilder.ToString();
                MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
            }

            fixFunc = new Func<bool>(() => StringOperations.FixBackslashes(tbIllegalBackslashList));
            if (tbIllegalBackslashList.Count == 1)
            {
                message = "The path of " + StringOperations.NameReplace(tbIllegalBackslashList[0]) + " looks illegal as it contains too many consecutive '\\' characters.";
                MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
            }
            else if (tbIllegalBackslashList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths look illegal because they contain too many consecutive '\\' characters:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbIllegalBackslashList)
                {
                    stringBuilder.AppendLine(StringOperations.NameReplace(tb));
                }
                message = stringBuilder.ToString();
                MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
            }

            ExceptLists(tbIllegalColonList);
            ExceptLists(tbIllegalBackslashList);
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
        private static bool HasBinDebug()
        {
            #region message parameters
            MessageUserControlIcons messageIcon = MessageUserControlIcons.Warning;
            MessageUserControlButtons messageButtons = MessageUserControlButtons.None;
            Func<bool> fixFunc = null;
            string message = null;
            #endregion

            // A list of TextBoxes, whose paths do not end with "bin\Debug".
            List<TextBox> tbNoBinDebugList = new List<TextBox>();

            foreach (var tb in validatingDebugTB)
            {
                if (!tb.Text.ToLower().EndsWith("\\bin\\debug\\"))
                {
                    tbNoBinDebugList.Add(tb);
                }
            }

            //If all text boxes end in "bin\Debug" we continue.
            if (tbNoBinDebugList.Count == 0) return true;

            //For user-friendlyness-ness-ness-ness we format the shown error in singular or plural case.
            if (tbNoBinDebugList.Count == 1)
            {
                message = "The path of " + StringOperations.NameReplace(tbNoBinDebugList[0]) + " does not end with a \"bin\\Debug\" folder.";
                MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
            }
            else if (tbNoBinDebugList.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths don't end with a \"bin\\Debug\" folder:" + Environment.NewLine + Environment.NewLine);
                foreach (var tb in tbNoBinDebugList)
                {
                    stringBuilder.AppendLine(StringOperations.NameReplace(tb));
                }
                message = stringBuilder.ToString();
                MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
            }

            return true;
        }

        /// <summary>
        /// Checks whether the listed directories in all TextBoxes exist and alerts the user and stops execution if any do not.
        /// </summary>
        /// <returns>
        /// "True" if all directories exist, otherwise "False".
        /// </returns>
        private static bool DirectoriesExist()
        {
            #region message parameters
            MessageUserControlIcons messageIcon = MessageUserControlIcons.Error;
            MessageUserControlButtons messageButtons = MessageUserControlButtons.Fix;
            Func<bool> fixFunc = null;
            string message = null;
            #endregion

            // This list will hold all text boxes whose listed directories do not exist.
            List<TextBox> tbDoesNotExistList = new List<TextBox>();

            // For each TextBox - check if its listed directory exists and add it to the list if it does not.
            foreach (var tb in validatingAllTB)
            {
                // A new task is started asynchronously that checks if the given directory exists.
                // If the task does not return a result in the given timeout or returns that the
                // directory does not exist the TextBox with said directory path is added to the list.
                var task = new System.Threading.Tasks.Task<bool>(() => { return Directory.Exists(tb.Text); });
                task.Start();

                if (!(task.Wait(500) && task.Result)) tbDoesNotExistList.Add(tb);
            }

            // If all directories exist - continue.
            if (tbDoesNotExistList.Count == 0) return true;

            // For user-friendlyness-ness-ness-ness we format the shown error in singular or plural case.
            if (tbDoesNotExistList.Count == 1)
            {
                // If the folder that does not exist is the QA one we prompt the user to create it.
                if (tbDoesNotExistList[0] == formPublisher.tbQAFolderPath)
                {
                    message = "The directory for " + StringOperations.NameReplace(tbDoesNotExistList[0]) + " does not exist.\n\nPressing the \"Fix\" button will attempt to create it.";
                    fixFunc = new Func<bool>(() => AdditionalFunctionality.CreateQAFolder());
                    MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
                }
                else
                {
                    message = "The directory for " + StringOperations.NameReplace(tbDoesNotExistList[0]) + " does not exist.\n\nPlease, check that the path is correct.";
                    MessageBuilder.CreateMessage(message, messageIcon, MessageUserControlButtons.None, fixFunc);
                    return false;
                }
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder("The directories for the following do not exist:" + Environment.NewLine + Environment.NewLine);
                foreach (var txtb in tbDoesNotExistList)
                {
                    stringBuilder.AppendLine(StringOperations.NameReplace(txtb));
                }
                stringBuilder.Append(Environment.NewLine + "Please, check that the paths are correct.");

                if (tbDoesNotExistList.Contains(formPublisher.tbQAFolderPath))
                {
                    stringBuilder.AppendLine("The QA Folder can be created by clicking the \"Fix\" button, but the other paths need to be corrected.");
                    message = stringBuilder.ToString();
                    fixFunc = new Func<bool>(() => AdditionalFunctionality.CreateQAFolder());
                    MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
                }
                else
                {
                    message = stringBuilder.ToString();
                    MessageBuilder.CreateMessage(message, messageIcon, MessageUserControlButtons.None, fixFunc);
                }

                return false;
            }

            ExceptLists(tbDoesNotExistList);
            return true;
        }

        /// <summary>
        /// Checks whether the user has write permissions for the network folder.
        /// </summary>
        /// <returns>
        /// "True" if the user can write to the folder, otherwise "False".
        /// </returns>
        private static bool HasNetworkAccess()
        {
            #region message parameters
            MessageUserControlIcons messageIcon = MessageUserControlIcons.Error;
            MessageUserControlButtons messageButtons = MessageUserControlButtons.None;
            Func<bool> fixFunc = null;
            string message = null;
            #endregion

            // A list containing all TextBoxes whose path raises an UnauthorizedAccessException when accessed.
            List<TextBox> unauthorizedAccessExceptionList = new List<TextBox>();
            // A list containing all TextBoxes whose path raises an InvalidOperationException when accessed.
            List<TextBox> invalidOperationExceptionList = new List<TextBox>();

            foreach (var tb in validatingAllTB)
            {
                try
                {
                    // This will raise an exception if the path is read only or the user
                    // does not have access to view the permissions.
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

            if (unauthorizedAccessExceptionList.Count == 0 && invalidOperationExceptionList.Count == 0)
            {
                return true;
            }
            else
            {
                StringBuilder errorMessage = new StringBuilder("There were network errors:" + Environment.NewLine + Environment.NewLine);

                if (unauthorizedAccessExceptionList.Count == 1)
                {
                    errorMessage.AppendLine("You are not authorized to access the folder for " + StringOperations.NameReplace(unauthorizedAccessExceptionList[0]) + Environment.NewLine);
                }
                // unauthorizedAccessExceptionList.Count can still be 0, the passed "if" check only triggers if BOTH lists are empty.
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

                message = errorMessage.ToString();
                MessageBuilder.CreateMessage(message, messageIcon, messageButtons, fixFunc);
                return false;
            }
        }

        /// <summary>
        /// Remove elements from the given list from the "validatingDebugTB"
        /// and "validatingAllTB" lists.
        /// </summary>
        /// <param name="localList">A local variable List, containing TextBoxes that needn't be validated further.</param>
        private static void ExceptLists(List<TextBox> localList)
        {
            validatingDebugTB = validatingDebugTB.Except(localList).ToList();
            validatingAllTB = validatingAllTB.Except(localList).ToList();
        }
    }
}
