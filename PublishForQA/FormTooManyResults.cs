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
        /// <summary>
        /// Initialises a new instance of the form "TooManyResults".
        /// </summary>
        /// <param name="eCheckPaths">List of results for the chosen E-Check version</param>
        /// <param name="corePaths">List of results for E-CheckCore</param>
        public FormTooManyResults(List<string> eCheckPaths, List<string> corePaths, string version)
        {
            //These are constants that will limit the initial size of out ListBoxes.
            //They will still resize indefinitely with the form.
            const int widthLimit = 600;
            const int heightLimit = 200;

            InitializeComponent();

            //If either list contains no items we insert a warning message.
            if (eCheckPaths.Count == 0) eCheckPaths.Add("No folders found for " + version + ".");
            if (corePaths.Count == 0) corePaths.Add("No folders found for E-CheckCore.");

            lbECheck.DataSource = eCheckPaths;
            lbCore.DataSource = corePaths;

            int tallestHeightECheck = Math.Min(lbECheck.GetItemRectangle(0).Height * lbECheck.Items.Count, heightLimit);
            int tallestHeightCore = Math.Min(lbCore.GetItemRectangle(0).Height * lbCore.Items.Count, heightLimit);

            //We create a list that is going to hold all the results
            List<string> longestList = eCheckPaths.Concat(corePaths).ToList();

            //We find the longest result so we can later set the ListBoxes' width appropriately
            string longest = longestList.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
            Graphics graphics = CreateGraphics();
            int longestWidth = Math.Min(TextRenderer.MeasureText(longest, Font).Width, widthLimit);
            
            //First we size the form, because if we size the ListBoxes first
            //they will be misshapen when the form is resized.
            Size = new Size(longestWidth + 45, tallestHeightECheck + tallestHeightCore + 175);
            
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

            //If an item is selected and it's not the "no folders found" text we set the paths.
            if (lbECheck.SelectedItem != null && !lbECheck.SelectedItem.ToString().Contains("No folders found for"))
            {
                string eCheck = Path.Combine(lbECheck.SelectedItem.ToString(), @"WinClient\E-Check\bin\Debug\");
                string service = Path.Combine(lbECheck.SelectedItem.ToString(), @"AppServer\ServiceHostNew\ServiceHostNew\bin\Debug\");
                parent.Controls.Find("tbECheckPath", false).FirstOrDefault().Text = eCheck;
                parent.Controls.Find("tbServicePath", false).FirstOrDefault().Text = service;
            }
            if (lbCore.SelectedItem != null && !lbCore.SelectedItem.ToString().Contains("No folders found for"))
            {
                string core = Path.Combine(lbCore.SelectedItem.ToString(), @"E-CheckCoreConsoleHost\bin\Debug\");
                parent.Controls.Find("tbCorePath", false).FirstOrDefault().Text = core;
            }

            Close();
            Dispose();
        }
    }
}
