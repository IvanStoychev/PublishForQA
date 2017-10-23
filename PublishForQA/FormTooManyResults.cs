using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            //These are constants that will limit the initial size of out ListBoxes.
            //They will still resize indefinitely with the form.
            const int widthLimit = 600;
            const int heightLimit = 200;

            List<string> eCheckPath = new List<string>() { "qwhfoqwjidwhpaiwjdpwaijdpawijsdapisdjpidpIQJDPAWIJFPAIFPAIJDPAWIDJAPWIFJAPFDIJPAFHAPIFPAFJAEPFOEFJO1", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "z" };
            List<string> eCheckCorePath = new List<string>() { "qwhfoqwjidwhpaiwjdFJO1adawdawdwad", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "p", "z" };
            //params - List<string> eCheckPath, List<string> eCheckCorePath

            InitializeComponent();
            lbECheck.DataSource = eCheckPath;
            lbCore.DataSource = eCheckCorePath;
            int tallestHeightECheck = Math.Min(lbECheck.GetItemRectangle(0).Height * lbECheck.Items.Count, heightLimit);
            int tallestHeightCore = Math.Min(lbCore.GetItemRectangle(0).Height * lbCore.Items.Count, heightLimit);

            //We create a list that is going to hold all the results
            List<string> longestList = new List<string>();

            //If both ECheck AND ECheckCore have more than 1 result
            //"longestList" becomes the sum of all their results.
            if (eCheckPath.Count > 1 && eCheckCorePath.Count > 1)
            {
                longestList = eCheckPath.Concat(eCheckCorePath).ToList();
            }
            //If there is more than 1 result for Echeck, only
            else if (eCheckPath.Count > 1 && eCheckCorePath.Count < 2)
            {
                //We hide the row and set the limit for the ECheckCore
                //ListBox to 0, because even with a hidden row it
                //has a real width value and that will interfere
                //with the initial sizing of the form.
                tlpMain.RowStyles[2].Height = 0;
                tallestHeightCore = 0;
                lbCore.ClearSelected();
                longestList = eCheckPath;
            }
            //If there is more than 1 result for EcheckCore, only
            else if (eCheckPath.Count < 2 && eCheckCorePath.Count > 1)
            {
                //We hide the row and set the limit for the ECheck
                //ListBox to 0, because even with a hidden row it
                //has a real width value and that will interfere
                //with the initial sizing of the form.
                tlpMain.RowStyles[3].Height = 0;
                tallestHeightECheck = 0;
                lbECheck.ClearSelected();
                longestList = eCheckCorePath;
            }

            //We find the longest result so we can later set the ListBoxes' width appropriately
            string longest = longestList.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
            Graphics graphics = this.CreateGraphics();
            int longestWidth = Math.Min(TextRenderer.MeasureText(longest, this.Font).Width, widthLimit);
            
            //First we size the form, because if we size the ListBoxes first
            //they will be misshapen after the form resize.
            this.Size = new Size(longestWidth + 45, tallestHeightECheck + tallestHeightCore + 175);
            
            lbECheck.ClientSize = new Size(longestWidth + 15, tallestHeightECheck);
            lblECheck.Location = new Point(0, 0);
            lbECheck.Location = new Point(0, 15);
            lblCore.Location = new Point(0, 0);
            lbCore.ClientSize = new Size(longestWidth + 15, tallestHeightCore);
            lbCore.Location = new Point(0, 15);

            lbECheck.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
            lbCore.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Form parent = Application.OpenForms["FormPublisher"];
            if (lbECheck.SelectedItem != null)
            {
                string ECheck = Path.Combine(lbECheck.SelectedItem.ToString(), @"\master\WinClient\E-Check\bin\Debug\");
                string ECheckService = Path.Combine(lbECheck.SelectedItem.ToString(), @"\master\AppServer\ServiceHostNew\ServiceHostNew\bin\Debug\");
                parent.Controls.Find("tbECheckPath", false).FirstOrDefault().Text = ECheck;
                parent.Controls.Find("tbECheckServicePath", false).FirstOrDefault().Text = ECheckService;
            }
            if (lbCore.SelectedItem != null)
            {
                string ECheckCore = Path.Combine(lbCore.SelectedItem.ToString(), @"\E-CheckCore\E-CheckCoreConsoleHost\bin\Debug\");
                parent.Controls.Find("tbECheckCorePath", false).FirstOrDefault().Text = ECheckCore;
            }

            this.Dispose();
        }
    }
}
