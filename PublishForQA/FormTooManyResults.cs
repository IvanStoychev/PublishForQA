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
            //they will look ugly when the form is being resized.
            this.Size = new Size(longestWidth + 30, tallestHeightECheck + tallestHeightCore + 100);
            
            lbECheck.ClientSize = new Size(longestWidth, tallestHeightECheck);
            lblECheck.Location = new Point(0, 0);
            lbECheck.Location = new Point(0, 15);
            lblCore.Location = new Point(0, 0);
            lbCore.ClientSize = new Size(longestWidth, tallestHeightCore);
            lbCore.Location = new Point(0, 15);


            //MessageBox.Show(lbCore.Location.Y.ToString());

            //tlpMain.RowStyles[1].SizeType = SizeType.Absolute;
            //tlpMain.RowStyles[1].Height = tallestHeightECheck + lblECheck.Height + 10;
            //tlpMain.RowStyles[2].SizeType = SizeType.Absolute;
            //tlpMain.RowStyles[2].Height = tallestHeightCore + lbCore.Height + 10;
            lbECheck.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
            lbCore.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
        }
    }
}
