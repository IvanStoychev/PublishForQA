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
            List<string> eCheckCorePath = new List<string>() { "qwhfoqwjidwhpaiwjdpwaijdpawijsdapisdjpidpIQJDPAWIJFPAIFPAIJDPAWIDJAPWIFJAPFDIJPAFHAPIFPAFJAEPFOEFJO1adawdawdwad", "o" };
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
                List<string> longestList = eCheckPath.Concat(eCheckCorePath).ToList();
                string longest = longestList.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
                Graphics graphics = this.CreateGraphics();
                SizeF size = graphics.MeasureString(longest, lblECheck.Font);
                size.Width = (float)(size.Width * 0.94);
                size.Height = 15 * eCheckPath.Count;
                lbECheck.Size = Size.Round(size);
                lbECheck.DataSource = eCheckPath;
                lbECheck.Location = new Point(0, 15);
                //lbECheck.ScrollAlwaysVisible = true;

                tlpMain.RowCount++;
                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
                tlpMain.Controls.Add(pEcheck, 0, tlpMain.RowCount - 1);
                pEcheck.Controls.Add(lblECheck);
                pEcheck.Controls.Add(lbECheck);
            }
            if (eCheckCorePath.Count > 1)
            {
                Panel pCore = new Panel();
            }

            tblpPrimary.Visible = false;
        }
    }
}
