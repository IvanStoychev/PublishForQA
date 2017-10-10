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

namespace PublishForQA
{
    public partial class FormPublisher : Form
    {
        public static List<string> AccessDeniedFolders = new List<string>();

        public FormPublisher()
        {   
            InitializeComponent();
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
                        results.AddRange(Directory.GetDirectories(folder, /*version*/"Torrents", SearchOption.AllDirectories));
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

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = (e.KeyCode == Keys.Enter);
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            string[] destinationPaths =
                {
                tbQAFolderPath.Text + "E-Check\\",
                tbQAFolderPath.Text + "E-CheckCore\\",
                tbQAFolderPath.Text + "E-CheckService\\"
                };
            string[] sourcePaths =
                {
                tbECheckPath.Text,
                tbECheckCorePath.Text,
                tbECheckServicePath.Text
                };

            for (int i = 0; i < 3; i++)
            {
                //First we create the directory structure
                foreach (string dirPath in Directory.GetDirectories(sourcePaths[i], "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourcePaths[i], destinationPaths[i]));

                //Then we copy all files, overwriting any existing ones
                foreach (string filePath in Directory.GetFiles(sourcePaths[i], "*", SearchOption.AllDirectories))
                    File.Copy(filePath, filePath.Replace(sourcePaths[i], destinationPaths[i]), true);
            }
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