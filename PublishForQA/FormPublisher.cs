using System;
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
            tbECheckList.Add(tbECheckCorePath);
            tbECheckList.Add(tbECheckServicePath);
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
            List<string> results = new List<string>();
            drives.AddRange(DriveInfo.GetDrives());
            drives = drives
                           .Where(x => x.DriveType == DriveType.Fixed || x.DriveType == DriveType.Removable)
                           .Select(x => x)
                           .ToList();

            foreach (var drive in drives)
            {
                List<string> folders = new List<string>();
                folders.AddRange(Directory.GetDirectories(drive.Name));
                foreach (var folder in folders)
                {
                    try
                    {
                        results.AddRange(Directory.GetDirectories(folder, version, SearchOption.AllDirectories));
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
            if (AccessDeniedFolders.Count > 0)
            {
                AccessDeniedFolders.Sort();
                pbAccessDenied.Visible = true;
            }
            else
            {
                pbAccessDenied.Visible = false;
            }
        }

        private void Browse(TextBox textBox)
        {
            folderBrowserDialog.ShowDialog();
            textBox.Text = folderBrowserDialog.SelectedPath + "\\";
        }

        private void btnECheckBrowse_Click(object sender, EventArgs e)
        {
            Browse(tbECheckPath);
        }

        private void btnECheckCoreBrowse_Click(object sender, EventArgs e)
        {
            Browse(tbECheckCorePath);
        }

        private void btnECheckServiceBrowse_Click(object sender, EventArgs e)
        {
            Browse(tbECheckServicePath);
        }

        private void btnQAFolderBrowse_Click(object sender, EventArgs e)
        {
            Browse(tbQAFolderPath);
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

        private void tb_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.EndsWith(".")) tb.Text = tb.Text.TrimEnd('.');
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            #region Validation
            //For clarity and "just in case", we add a slash
            //at the end of paths that don't have one.
            foreach (var tb in tbECheckList)
            {   
                if (!tb.Text.EndsWith("\\")) tb.Text = tb.Text + "\\";
            }
            if (!tbQAFolderPath.Text.EndsWith("\\")) tbQAFolderPath.Text = tbQAFolderPath.Text + "\\";

            //We check if each path ends in "\\bin\\debug\\"
            List<TextBox> tbNoBinDebugList = new List<TextBox>();
            foreach (var tb in tbECheckList)
            {
                //Considering the previous validation all paths should end in "\\" but
                //just in case we also check for "\\bin\\debug".
                if (!tb.Text.ToLower().EndsWith("\\bin\\debug\\") && !tb.Text.ToLower().EndsWith("\\bin\\debug"))
                {
                    tbNoBinDebugList.Add(tb);
                }
            }

            if (tbNoBinDebugList.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder("The following paths don't end with a \"bin\\Debug\" folder:" + System.Environment.NewLine + System.Environment.NewLine);
                foreach (var tb in tbNoBinDebugList)
                {
                    stringBuilder.Append(tb.Name.Replace("tbECheck", "E-Check ").Replace("Path", "") + System.Environment.NewLine);

                }
                stringBuilder.Append(System.Environment.NewLine + "Are you sure you wish to proceed?");
                DialogResult confirm = MessageBox.Show(stringBuilder.ToString(), "Path warning", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.No) return;
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
                tbECheckCorePath.Text,
                tbECheckServicePath.Text
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