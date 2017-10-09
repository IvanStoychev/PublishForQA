using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PublishForQA
{
    public partial class FormPublisher : Form
    {
        public FormPublisher()
        {
            InitializeComponent();
            //cbVersions.Items.AddRange();
        }

        private static void Locate(string version)
        {
            List<DriveInfo> drives = new List<DriveInfo>();
            List<string> folders = new List<string>();
            List<string> results = new List<string>();
            List<string> accessDeniedFolders = new List<string>();
            drives.AddRange(DriveInfo.GetDrives());
            drives = drives
                           .Where(x => x.DriveType == DriveType.Fixed || x.DriveType == DriveType.Removable)
                           .Select(x => x)
                           .ToList();

            foreach (var drive in drives)
            {
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
                            accessDeniedFolders.Add(ex.Message.Replace(@"Access to the path '", "").Replace(@"' is denied.", ""));
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
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

        private void Browse(TextBox textBox)
        {
            folderBrowserDialog.ShowDialog();
            textBox.Text = folderBrowserDialog.SelectedPath;
        }

        public static void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Locate(e.ClickedItem.ToString());
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
            contextMenuStrip.Items.Add("gg");
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