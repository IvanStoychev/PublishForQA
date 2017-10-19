using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PublishForQA
{
    public partial class FormTooManyResults : Form
    {
        public FormTooManyResults()
        {
            List<string> eCheckPath = new List<string>() { "qwhfoqwjidwhpaiwjdpwaijdpawijsdapisdjpidpIQJDPAWIJFPAIFPAIJDPAWIDJAPWIFJAPFDIJPAFHAPIFPAFJAEPFOEFJO1", "l"};
            List<string> eCheckCorePath = new List<string>() { "qwhfoqwjidwhpaiwjdFJO1adawdawdwad", "o" };
            //params - List<string> eCheckPath, List<string> eCheckCorePath
            InitializeComponent();

            Label lblText = new Label();
            lblText.Text = "More than one result was found.  Please choose which one is to be used.";
            lblText.AutoSize = false;
            lblText.Size = new Size(195, 29);
            lblText.UseMnemonic = false;

            TableLayoutPanel tlpMain = new TableLayoutPanel();
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tlpMain.RowCount = 1;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tlpMain.AutoSize = true;
            tlpMain.Controls.Add(lblText, 0, 0);
            tlpMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpMain.Location = new Point(12, 12);
            this.Controls.Add(tlpMain);

            //We create a list that is going to hold all the results
            List<string> longestList = new List<string>();

            //If there are more than 1 result for both ECheck AND ECheckCore
            //"longestList" becomes the sum of all their results.
            if (eCheckPath.Count > 1 && eCheckCorePath.Count > 1)
            {
                longestList = eCheckPath.Concat(eCheckCorePath).ToList();
            }
            //If there is more than 1 result for Echeck, only
            else if (eCheckPath.Count > 1 && eCheckCorePath.Count < 2)
            {
                longestList = eCheckPath;
            }
            //If there is more than 1 result for EcheckCore, only
            else if (eCheckPath.Count < 2 && eCheckCorePath.Count > 1)
            {
                longestList = eCheckCorePath;
            }

            string longest = longestList.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);

            if (eCheckPath.Count > 1)
            {
                Panel pEcheck = new Panel();
                pEcheck.Dock = DockStyle.Fill;
                pEcheck.AutoSize = true;
                pEcheck.AutoScroll = true;

                Label lblECheck = new Label();
                lblECheck.Text = "E-Check location:";
                lblECheck.AutoSize = true;
                lblECheck.UseMnemonic = false;

                ListBox lbECheck = new ListBox();
                
                Graphics graphics = this.CreateGraphics();
                SizeF size = graphics.MeasureString(longest, lbECheck.Font);
                size.Width = MeasureString(longest, lbECheck.Font);
                size.Height = 15 * eCheckPath.Count;
                lbECheck.Size = Size.Round(size);
                lbECheck.DataSource = eCheckPath;
                lbECheck.Location = new Point(0, 15);

                tlpMain.RowCount++;
                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
                tlpMain.Controls.Add(pEcheck, 0, tlpMain.RowCount - 1);
                pEcheck.Controls.Add(lblECheck);
                pEcheck.Controls.Add(lbECheck);
            }
            if (eCheckCorePath.Count > 1)
            {
                Panel pCore = new Panel();
                pCore.Dock = DockStyle.Fill;
                pCore.AutoSize = true;
                pCore.AutoScroll = true;

                Label lblECheck = new Label();
                lblECheck.Text = "E-Check location:";
                lblECheck.AutoSize = true;
                lblECheck.UseMnemonic = false;

                ListBox lbECheck = new ListBox();

                Graphics graphics = this.CreateGraphics();
                SizeF size = graphics.MeasureString(longest, lbECheck.Font);
                size.Width = MeasureString(longest, lbECheck.Font);
                size.Height = 15 * eCheckPath.Count;
                lbECheck.Size = Size.Round(size);
                lbECheck.DataSource = eCheckPath;
                lbECheck.Location = new Point(0, 15);

                tlpMain.RowCount++;
                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
                tlpMain.Controls.Add(pCore, 0, tlpMain.RowCount - 1);
                pCore.Controls.Add(lblECheck);
                pCore.Controls.Add(lbECheck);
            }
        }

        static public int MeasureDisplayStringWidth2(Graphics graphics, string text, Font font)
        {
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0, 0, 1000, 1000);
            System.Drawing.CharacterRange[] ranges = {new System.Drawing.CharacterRange(0, text.Length) };
            System.Drawing.Region[] regions = new System.Drawing.Region[1];

            format.SetMeasurableCharacterRanges(ranges);

            regions = graphics.MeasureCharacterRanges(text, font, rect, format);
            rect = regions[0].GetBounds(graphics);

            return (int)(rect.Right + 1.0f);
        }

        float MeasureString(string text, Font font)
        {
            Graphics g = Graphics.FromImage(new Bitmap(1, 1));
            return g.MeasureString(text + "|", font, this.Width, StringFormat.GenericTypographic).Width - g.MeasureString("|", this.Font, PointF.Empty, StringFormat.GenericTypographic).Width;
        }

        static public int MeasureDisplayStringWidth1(Graphics graphics, string text,
                                            Font font)
        {
            const int width = 32;

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, 1, graphics);
            System.Drawing.SizeF size = graphics.MeasureString(text, font);
            System.Drawing.Graphics anagra = System.Drawing.Graphics.FromImage(bitmap);

            int measured_width = (int)size.Width;

            if (anagra != null)
            {
                anagra.Clear(Color.White);
                anagra.DrawString(text + "|", font, Brushes.Black,
                                   width - measured_width, -font.Height / 2);

                for (int i = width - 1; i >= 0; i--)
                {
                    measured_width--;
                    if (bitmap.GetPixel(i, 0).R != 255)    // found a non-white pixel ?
                        break;
                }
            }

            return measured_width;
        }
    }
}
