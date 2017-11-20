using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace PublishForQA
{
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

    public class TextBoxFormPublisher : TextBox
    {
        //We capture the "paste" event, so that we can eliminate any
        //illegal characters being copy-pasted into the TextBox.
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x302 && Clipboard.ContainsText())
            {
                this.SelectedText = ReplaceIllegalChars(Clipboard.GetText());
                return;
            }
            base.WndProc(ref m);
        }

        //For readability we remove each character separately.
        private string ReplaceIllegalChars(string str)
        {
            if (this.Name == "tbTaskName")
            {
                str = str.Replace(":", string.Empty);
                str = str.Replace("\\", string.Empty);
            }
            str = str.Replace("\"", string.Empty);
            str = str.Replace("/", string.Empty);
            str = str.Replace("?", string.Empty);
            str = str.Replace("|", string.Empty);
            str = str.Replace("*", string.Empty);
            str = str.Replace("<", string.Empty);
            str = str.Replace(">", string.Empty);

            return str;
        }
    }
}
