﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text;

namespace PublishForQA
{
    public partial class FormPublisher : Form
    {
        public static List<string> AccessDeniedFolders = new List<string>();
        public static List<TextBox> tbECheckList = new List<TextBox>();

        public FormPublisher()
        {   
            InitializeComponent();
            tbECheckList.Add(tbECheckPath);
            tbECheckList.Add(tbCorePath);
            tbECheckList.Add(tbServicePath);
        }

        /// <summary>
        /// Gets a list of all the folders of the fixed and removable drives on the system and searches through them for
        /// a folder named as the passed version parameter.
        /// </summary>
        /// <param name="version">The folder name of the E-Check version to search for</param>
        /// <remarks>The method also keeps a list of all folders that access was denied to</remarks>
        private void Locate(string version)
        {
            List<DriveInfo> drives = new List<DriveInfo>();
            List<string> ECheckresults = new List<string>();
            List<string> Coreresults = new List<string>();
            drives.AddRange(DriveInfo.GetDrives());
            drives = drives
                           .Where(x => x.DriveType == DriveType.Fixed || x.DriveType == DriveType.Removable)
                           .ToList();

            this.Cursor = Cursors.WaitCursor;

            //For each Fixed or Removable storage drive on the system we search for folders
            //named after the selected version and "E-CheckCore".
            //We also create a list of all folders to which access was denied to.
            foreach (var drive in drives)
            {
                List<string> folders = new List<string>();
                folders.AddRange(Directory.GetDirectories(drive.Name));
                foreach (var folder in folders)
                {
                    try
                    {
                        ECheckresults.AddRange(Directory.GetDirectories(folder, version, SearchOption.AllDirectories));
                        Coreresults.AddRange(Directory.GetDirectories(folder, "E-CheckCore", SearchOption.AllDirectories));
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.UnauthorizedAccessException)
                        {
                            AccessDeniedFolders.Add(ex.Message.Replace(@"Access to the path '", "").Replace(@"' is denied.", ""));
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            //If there were any folders that access was denied to we show the
            //button which opens the dialog which lists them.
            if (AccessDeniedFolders.Count > 0)
            {
                AccessDeniedFolders = AccessDeniedFolders.Distinct().ToList();
                AccessDeniedFolders.Sort();
                pbAccessDenied.Visible = true;
            }
            else
            {
                pbAccessDenied.Visible = false;
            }

            this.Cursor = Cursors.Default;

            List<string> eCheckPath = ECheckresults.Where(x => Directory.Exists(x + @"\master\WinClient\E-Check\bin\Debug\") && Directory.Exists(x + @"\master\AppServer\ServiceHostNew\ServiceHostNew\bin\Debug\")).ToList();
            List<string> corePath = Coreresults.Where(x => Directory.Exists(x + @"\E-CheckCore\E-CheckCoreConsoleHost\bin\Debug\")).ToList();

            //No results for either E-Check or E-CheckCore
            if (eCheckPath.Count < 1 && corePath.Count < 1)
            {
                MessageBox.Show("Neither " + version + " nor E-CheckCore were found");
                return;
            }
            //No results for E-Check
            else if (eCheckPath.Count > 0 && corePath.Count < 1)
            {
                MessageBox.Show(version + "was not found.");
                return;
            }
            //No results for E-CheckCore
            else if (eCheckPath.Count < 1 && corePath.Count > 0)
            {
                MessageBox.Show("E-CheckCore was not found.");
                return;
            }

            if (eCheckPath.Count == 1)
            {
                tbECheckPath.Text = Path.Combine(eCheckPath[0], @"master\WinClient\E-Check\bin\Debug\");
                tbServicePath.Text = Path.Combine(eCheckPath[0], @"master\AppServer\ServiceHostNew\ServiceHostNew\bin\Debug\");
            }
            if (corePath.Count == 1)
            {
                tbCorePath.Text = Path.Combine(corePath[0], @"E-CheckCore\E-CheckCoreConsoleHost\bin\Debug\");
            }

            if (eCheckPath.Count > 1 || corePath.Count > 1)
            {
                FormTooManyResults formTooManyResults = new FormTooManyResults(eCheckPath, corePath);
                formTooManyResults.ShowDialog();
            }
        }

        private void Browse(object sender, EventArgs e)
        {
            //First we get the TextBox, corresponding to the pressed button.
            //Then we set the selected path of the FolderBrowserDialog to the text of said TextBox.
            //If the TextBox has invalid text it will just default to "Desktop".
            //Lastly if the user clicked "OK" we set the TextBox text to be the selected path and add
            //a backslash to its end (by use of shorthand "if... then... else" statement) if it already doesn't have one.
            Control control = sender as Control;
            TextBox textBox = this.Controls.Find(control.Name.Replace("btn", "tb").Replace("Browse", "Path"), false).OfType<TextBox>().FirstOrDefault();
            folderBrowserDialog.SelectedPath = textBox.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) textBox.Text = folderBrowserDialog.SelectedPath.EndsWith("\\") ? folderBrowserDialog.SelectedPath : folderBrowserDialog.SelectedPath + "\\";
        }

        public static void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ((FormPublisher)Form.ActiveForm).btnLocate.Menu.Close();
            ((FormPublisher)Form.ActiveForm).Locate(e.ClickedItem.ToString());
        }

        private void pbAccessDenied_Click(object sender, EventArgs e)
        {
            FormAccessDenied accessDenied = new FormAccessDenied();
            accessDenied.ShowDialog();
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
                    e.KeyChar == ':' ||
                    e.KeyChar == '*' ||
                    e.KeyChar == '<' ||
                    e.KeyChar == '>'
                );

            //There is a special case for the "Task Name" textbox
            if (sender.Equals(tbTaskName) && e.KeyChar == '\\') e.Handled = true;

            //If the pressed key is the "Enter" key we call the textbox "Leave" event.
            if (e.KeyChar == (char)Keys.Return) tb_Leave(sender, new EventArgs());
        }

        //This event is only used to emulate "Control + A" select all text behaviour
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
            this.Cursor = Cursors.WaitCursor;

            #region Validation
            //For clarity and "just in case", we add a slash at the end of paths that don't have one.
            //And we check if the paths ends with in "bin\Debug" folder.
            List<TextBox> tbNoBinDebugList = new List<TextBox>();
            foreach (var tb in tbECheckList)
            {   
                if (!tb.Text.EndsWith("\\")) tb.Text = tb.Text + "\\";
                //Considering the previous validation all paths should end in "\\" but
                //just in case we also check for "\\bin\\debug", as well.
                if (!tb.Text.ToLower().EndsWith("\\bin\\debug\\") && !tb.Text.ToLower().EndsWith("\\bin\\debug"))
                {
                    tbNoBinDebugList.Add(tb);
                }
            }
            if (!tbQAFolderPath.Text.EndsWith("\\")) tbQAFolderPath.Text = tbQAFolderPath.Text + "\\";
            
            if (tbNoBinDebugList.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths don't end with a \"bin\\Debug\" folder:" + System.Environment.NewLine + System.Environment.NewLine);
                foreach (var tb in tbNoBinDebugList)
                {
                    stringBuilder.AppendLine(tb.Name.Replace("tb", "").Replace("Path", ""));

                }
                stringBuilder.Append(System.Environment.NewLine + "Are you sure you wish to proceed?");
                DialogResult confirm = MessageBox.Show(stringBuilder.ToString(), "Path warning", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.No)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
            }
            #endregion

            string[] destinationPaths =
                {
                tbQAFolderPath.Text + tbTaskName.Text + "\\E-Check\\",
                tbQAFolderPath.Text + tbTaskName.Text + "\\E-CheckCore\\",
                tbQAFolderPath.Text + tbTaskName.Text + "\\E-CheckService\\"
                };
            string[] sourcePaths =
                {
                tbECheckPath.Text,
                tbCorePath.Text,
                tbServicePath.Text
                };
            
            #region Copying
            for (int i = 0; i < 3; i++)
            {
                //First we create the directory structure
                foreach (string dirPath in Directory.GetDirectories(sourcePaths[i], "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourcePaths[i], destinationPaths[i]));

                //Then we copy all files, overwriting any existing ones
                foreach (string filePath in Directory.GetFiles(sourcePaths[i], "*", SearchOption.AllDirectories))
                    File.Copy(filePath, filePath.Replace(sourcePaths[i], destinationPaths[i]), true);
            }
            #endregion

            this.Cursor = Cursors.Default;
        }

        private void pbHelp_Click(object sender, EventArgs e)
        {
            new FormHelp().ShowDialog();
            ttECheck.Show("The path to the E-Check version winclient's debug folder,\nwhich you want to copy from.", tbECheckPath);
            ttCore.Show("The path to the E-Check Core's debug folder,\nwhich you want to copy from.", tbCorePath);
            ttService.Show("The path to the E-Check version service's debug folder,\nwhich you want to copy from.", tbServicePath);
            ttQAFolder.Show("The path to your QA folder, where you want to copy to.\nWorks with mapped network drives.", tbQAFolderPath);
            ttTaskName.Show("The name of the task you are currently working on.\nThis will be used as the name of the folder all the files will be copied to.", tbTaskName, tbTaskName.Width/2, tbTaskName.Height/2 - 10);
            ttPublish.Show("Clicking this button will first check if all paths end with a \"bin\\debug\" folder,\nprompt, if needed, and copy all the files from the debug folders to your QAfolder\\TaskName.", btnPublish);
            ttLocate.Show("Clicking this button will open a context menu with all current E-Check versions. Choosing one will initiate a search\non all fixed and removable drives for folders with the version name. If found, and if a \"debug\" folder exists,\nthe path to the corresponding debug folders will be automatically entered into the textboxes below.", btnLocate, btnLocate.Width / 2, btnLocate.Height / 2 - 10);
        }

        private void FormPublisher_Click(object sender, EventArgs e)
        {
            ttCore.Hide(this);
            ttECheck.Hide(this);
            ttService.Hide(this);
            ttQAFolder.Hide(this);
            ttTaskName.Hide(this);
            ttPublish.Hide(this);
            ttLocate.Hide(this);
        }
    }

    public class MenuButton : Button
    {
        [DefaultValue(null)]
        public ContextMenuStrip Menu { get; set; }

        [DefaultValue(false)]
        public bool ShowMenuUnderCursor { get; set; }

        public MenuButton()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(FormPublisher.contextMenuStrip_ItemClicked);
            Menu = contextMenuStrip;
            string result = string.Empty;
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("PublishForQA.Resources.E-CheckVersions.xml"))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            XmlReader reader = XmlReader.Create(new StringReader(result));
            var doc = XDocument.Load(reader);
            List<XElement> elements = doc.Root.Elements("Version").ToList();
            foreach (var element in elements)
            {
                contextMenuStrip.Items.Add(element.Value);
            }
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if (Menu != null && mevent.Button == MouseButtons.Left)
            {
                Point menuLocation;

                if (ShowMenuUnderCursor)
                {
                    menuLocation = mevent.Location;
                }
                else
                {
                    menuLocation = new Point(0, Height);
                }

                Menu.Show(this, menuLocation);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (Menu != null)
            {
                int arrowX = ClientRectangle.Width - 14;
                int arrowY = ClientRectangle.Height / 2 - 1;

                Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
                Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
                pevent.Graphics.FillPolygon(brush, arrows);
            }
        }
    }
}