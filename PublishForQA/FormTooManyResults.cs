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
            List<string> eCheckPath = new List<string>() { "qwhfoqwjidwhpaiwjdpwaijdpawijsdapisdjpidpIQJDPAWIJFPAIFPAIJDPAWIDJAPWIFJAPFDIJPAFHAPIFPAFJAEPFOEFJO1", "g"};
            List<string> eCheckCorePath = new List<string>() { "qwhfoqwjidwhpaiwjdFJO1adawdawdwad", "p" };
            //params - List<string> eCheckPath, List<string> eCheckCorePath
            InitializeComponent();
            
            Label lblText = new Label();
            lblText.Text = "More than one result was found.  Please choose which one is to be used.";
            lblText.AutoSize = false;
            lblText.Size = new Size(195, 29);
            lblText.UseMnemonic = false;

            Panel panel = new Panel();
            //panel.AutoSize = true;
            panel.AutoScroll = true;
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(3, 3, 0, 0);
            panel.Controls.Add(lblText);

            TableLayoutPanel tlpMain = new TableLayoutPanel();
            tlpMain.Dock = DockStyle.Fill;
            //tlpMain.AutoSize = true;
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tlpMain.RowCount = 1;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 33));
            tlpMain.Controls.Add(panel, 0, 0);
            tlpMain.Location = new Point(8, 10);
            this.Controls.Add(tlpMain);

            tlpMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;


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

            //We find the longest result so we can later set the ListBoxes' width appropriately
            string longest = longestList.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
            Graphics graphics = this.CreateGraphics();
            //We limit the width to 600. The number is chosen arbitrarily
            int longestWidth = Math.Min(TextRenderer.MeasureText(longest, this.Font).Width, 600);

            //Labels and ListBoxes are created, only if needed (more than one result)
            if (eCheckPath.Count > 1)
            {
                Panel pEcheck = new Panel();
                pEcheck.AutoScroll = true;
                pEcheck.Dock = DockStyle.Fill;
                pEcheck.Margin = new Padding(5);

                Label lblECheck = new Label();
                lblECheck.Text = "E-Check location:";
                lblECheck.AutoSize = true;
                lblECheck.UseMnemonic = false;

                ListBox lbECheck = new ListBox();
                lbECheck.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
                lbECheck.DataSource = eCheckPath;
                lbECheck.Location = new Point(0, 15);
                lbECheck.Size = new Size(longestWidth, 15 * eCheckPath.Count);

                tlpMain.RowCount++;
                tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                tlpMain.Controls.Add(pEcheck, 0, tlpMain.RowCount - 1);
                pEcheck.Controls.Add(lblECheck);
                pEcheck.Controls.Add(lbECheck);
            }
            if (eCheckCorePath.Count > 1)
            {
                Panel pCore = new Panel();
                pCore.AutoScroll = true;
                pCore.Dock = DockStyle.Fill;
                pCore.Margin = new Padding(5);

                Label lblECheckCore = new Label();
                lblECheckCore.Text = "E-CheckCore location:";
                lblECheckCore.AutoSize = true;
                lblECheckCore.UseMnemonic = false;

                ListBox lbECheckCore = new ListBox();
                lbECheckCore.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
                lbECheckCore.DataSource = eCheckCorePath;
                lbECheckCore.Location = new Point(0, 15);
                lbECheckCore.Size = new Size(longestWidth, 15 * eCheckCorePath.Count);

                tlpMain.RowCount++;
                tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                tlpMain.Controls.Add(pCore, 0, tlpMain.RowCount - 1);
                pCore.Controls.Add(lblECheckCore);
                pCore.Controls.Add(lbECheckCore);
            }

            Button btnOK = new Button();
            btnOK.Anchor = AnchorStyles.Right;
            btnOK.Click += new EventHandler(btnOK_Click);
            btnOK.Text = "OK";
            tlpMain.RowCount++;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, btnOK.Height + 10));
            tlpMain.Controls.Add(btnOK, 0, tlpMain.RowCount - 1);

            this.Size = new Size(longestWidth + 30, 15 * eCheckCorePath.Count + 15 * eCheckPath.Count + 200);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
        }
    }
}
