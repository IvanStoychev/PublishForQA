using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PublishForQA
{
    public partial class FormPublisher : Form
    {
        /// <summary>
        /// This character will be used as a separator when writing the save file. We need it as a landmark
        /// to be able to tell our position when loading the save file.
        /// </summary>
        const char Separator = '*';
        /// <summary>
        /// This string will be used to detect if an exception has occured
        /// before the directory creation for-loop.
        /// </summary>
        public const string ErrorBeforeDirectoryLoop = "Exception occurred before the start of directory structure creation.";
        /// <summary>
        /// This string will be used to detect if an exception has occured
        /// before the file copying for-loop.
        /// </summary>
        public const string ErrorBeforeFileLoop = "Exception occurred before the start of the file copy process.";
        /// <summary>
        /// A list of all text boxes on the form.
        /// </summary>
        public List<TextBox> AllTextBoxesList = new List<TextBox>();
        /// <summary>
        /// A list of all E-Check debug folder text boxes on the form.
        /// </summary>
        public List<TextBox> DebugTextBoxesList = new List<TextBox>();

        public FormPublisher()
        {
            InitializeComponent();
            ListTextBoxes();

            //If there is only a single *.txt file in the current directory, it tries to load it.
            List<string> txtFilesInDir = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt", SearchOption.TopDirectoryOnly).ToList();
            if (txtFilesInDir.Count == 1)
            {
                LoadFile(txtFilesInDir.First());
            }
        }

        #region Events of controls
        public static void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            FormPublisher formPublisher = ((FormPublisher)Form.ActiveForm);
            formPublisher.CursorChange();
            formPublisher.btnLocate.Menu.Close();
            AdditionalFunctionality.Locate(e.ClickedItem.ToString());
            formPublisher.CursorChange();
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            //If an IllegalPath character or the Enter key would be entered in the
            //textboxes, we prevent it by setting the "Handled" property to "true".
            e.Handled =
                (
                    e.KeyChar == (char)Keys.Return ||
                    e.KeyChar == '"' ||
                    e.KeyChar == '/' ||
                    e.KeyChar == '?' ||
                    e.KeyChar == '|' ||
                    e.KeyChar == '*' ||
                    e.KeyChar == '<' ||
                    e.KeyChar == '>'
                );

            //There is a special case for the "Task Name" textbox
            if (sender.Equals(tbTaskName) && (e.KeyChar == '\\' || e.KeyChar == ':')) e.Handled = true;

            //If the pressed key is the "Enter" key we call the textbox "Leave" event.
            if (e.KeyChar == (char)Keys.Return) tb_Leave(sender, new EventArgs());
        }

        //This event is only used to emulate "Control + A" select all text behaviour.
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (e.Control && e.KeyCode == Keys.A) tb.SelectAll();
        }

        private void tb_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.EndsWith(".")) tb.Text = tb.Text.TrimEnd('.');
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            CursorChange();
            ListTextBoxes();

            Publisher.Publish();

            ListTextBoxes();
            CursorChange();
        }

        private void pbHelp_Click(object sender, EventArgs e)
        {
            //We check if there already is an open FormHelp.
            //This is done by getting all open forms of type "FormHelp". Since at most there should be only one
            //we use "FirstOrDefault()" in this way we will either get the open one or null. If we get null then
            //there must be no open help forms (and we got "Default") so we open one.
            //ELSE we set the focus to the one that was returned by "First".
            if (Application.OpenForms.OfType<FormHelp>().FirstOrDefault() == null)
            {
                new FormHelp().Show();
            }
            else
            {
                Application.OpenForms.OfType<FormHelp>().First().Focus();
            }
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (TextBox tb in AllTextBoxesList)
                        {
                            sw.WriteLine(tb.Name + Separator + " " + tb.Text);
                        }
                    }
                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("The name provided for the file was null." + Environment.NewLine + "Save operation failed.", "Null argument exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException)
                {
                    MessageBox.Show("The file is locked by another process." + Environment.NewLine + "Save operation failed.", "IO exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected exception has occurred:\n" + ex.Message + "\n\nSave operation failed.", "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void pbLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFile(openFileDialog.FileName);
            }
        }

        private void pbLoadDropdown_Click(object sender, EventArgs e)
        {
            PictureBox owner = sender as PictureBox;
            List<string> saveFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).Take(10).ToList();
            ContextMenuStrip dropDown = new ContextMenuStrip();
            foreach (var file in saveFiles)
            {
                dropDown.Items.Add(file.Replace(".txt", string.Empty), null, ContextMenuStrip_Click);
            }
            dropDown.Show(owner, new System.Drawing.Point(0, owner.Height));
        }

        private void ContextMenuStrip_Click(object sender, EventArgs e)
        {
            LoadFile(Path.Combine(Directory.GetCurrentDirectory(), sender.ToString() + ".txt"));
        }

        private void pbCopyToClipboard_Click(object sender, EventArgs e)
        {
            string errorText;
            ErrorProvider errorProvider = new ErrorProvider(Application.OpenForms.OfType<FormPublisher>().FirstOrDefault());

            try
            {
                if (!tbQAFolderPath.Text.EndsWith("\\") && tbQAFolderPath.Text != "") tbQAFolderPath.Text = tbQAFolderPath.Text + "\\";
                Clipboard.SetText(Path.Combine(tbQAFolderPath.Text + tbTaskName.Text));
                errorText = "QA folder path copied to clipboard.";
                errorProvider.Icon = Properties.Resources.Success;
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            }
            catch (ArgumentNullException)
            {
                Clipboard.Clear();
                errorText = "No QA folder and task name!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected exception has occurred:\n" + ex.Message + "\n\nCopy to Clipboard operation failed.", "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            errorProvider.SetError(pbCopyToClipboard, errorText);
            System.Threading.Tasks.Task.Delay(3000).ContinueWith(t => errorProvider.Dispose());
        }

        #endregion

        /// <summary>
        /// Hardcodes all TextBoxes into a List<TextBox> for ease
        /// of operation later.
        /// </summary>
        private void ListTextBoxes()
        {
            AllTextBoxesList.Clear();
            AllTextBoxesList.Add(tbECheckPath);
            AllTextBoxesList.Add(tbCorePath);
            AllTextBoxesList.Add(tbServicePath);
            AllTextBoxesList.Add(tbQAFolderPath);

            DebugTextBoxesList.Clear();
            DebugTextBoxesList.Add(tbECheckPath);
            DebugTextBoxesList.Add(tbCorePath);
            DebugTextBoxesList.Add(tbServicePath);
        }

        /// <summary>
        /// Opens a FolderBrowserDialog that allows the user to select a directory,
        /// the path to which will be set as the calling TextBox's Text property.
        /// </summary>
        private void Browse(object sender, EventArgs e)
        {
            //First we get the TextBox, corresponding to the pressed button.
            //Then we set the selected path of the FolderBrowserDialog to the text of said TextBox.
            //If the TextBox has invalid text it will just default to "Desktop".
            //Lastly if the user clicked "OK" we set the TextBox text to be the selected path and add
            //a backslash to its end (by use of shorthand "if... then... else" statement) if it already doesn't have one.
            Control control = sender as Control;
            TextBox textBox = tlpMain.Controls.Find(control.Name.Replace("btn", "tb").Replace("Browse", "Path"), false).OfType<TextBox>().FirstOrDefault();
            folderBrowserDialog.SelectedPath = textBox.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) textBox.Text = folderBrowserDialog.SelectedPath.EndsWith("\\") ? folderBrowserDialog.SelectedPath : folderBrowserDialog.SelectedPath + "\\";
        }

        /// <summary>
        /// Reads the given file line by line and attempts to retrieve the values for all TextBoxes.
        /// It will display a MessageBox with all TextBoxes for which it could not find a value.
        /// </summary>
        /// <param name="filePath">The full path to the save file.</param>
        private void LoadFile(string filePath)
        {
            //We will use this list to tell if a value for a TextBox was missing in the save file.
            List<TextBox> notFoundBoxes = AllTextBoxesList.ToList();
            string line;
            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        try
                        {
                            if (line.Length > 1)
                            {
                                //We get the substring from the beginning of the line to the first occurance of our separator. That
                                //should be the name of one of our TextBoxes. If we can match the substring to a TextBox that means
                                //we have a value for it and we take it out of the list "notFoundBoxes".
                                //Thus in the end we have a list of only the TextBoxes we couldn't find a value for.
                                TextBox tb = tlpMain.Controls.Find(line.Substring(0, Math.Max(1, line.IndexOf(Separator))), false).OfType<TextBox>().FirstOrDefault();
                                if (tb == null) continue;
                                tb.Text = line.Substring(line.IndexOf(Separator) + 2);
                                notFoundBoxes.Remove(tb);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An unexpected exception has occurred while reading the save file:\n" + ex.Message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    //For user-friendlyness-ness-ness-ness we format the shown error in singular or plural case.
                    if (notFoundBoxes.Count == 1)
                    {
                        MessageBox.Show("The path for " + StringOperations.NameReplace(notFoundBoxes[0]) + " could not be found in the file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (notFoundBoxes.Count > 1)
                    {
                        StringBuilder stringBuilder = new StringBuilder("The paths for the following TextBoxes:" + Environment.NewLine + Environment.NewLine);
                        foreach (var tb in notFoundBoxes)
                        {
                            stringBuilder.AppendLine(StringOperations.NameReplace(tb));
                        }
                        stringBuilder.Append(Environment.NewLine + "could not be found in the save file.");
                        MessageBox.Show(stringBuilder.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("A null argument has been passed to the load method.", "Null argument exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected exception has occurred while trying to load the save file:\n" + ex.Message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Changes the mouse cursor between "WaitCursor" and "Default".
        /// </summary>
        private void CursorChange()
        {
            if (Cursor == Cursors.Default)
            {
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
    }
}