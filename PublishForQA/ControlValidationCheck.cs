using PublishForQA.Properties;
using System;
using System.Windows.Forms;

namespace PublishForQA
{
    public partial class ControlValidationCheck : UserControl
    {
        bool isCollapsed;

        public ControlValidationCheck(string validationName, string validationDetails)
        {
            InitializeComponent();
            lblValidationName.Text = validationName;
            lblValidationDetails.Text = validationDetails;
        }

        private void pbExpandCollapse_Click(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                tlpMain.RowStyles[1].SizeType = SizeType.AutoSize;
                this.Height += tlpMain.GetRowHeights()[1];
                pbExpandCollapse.Image = Resources.ArrowUp;
            }
            else
            {
                this.Height -= tlpMain.GetRowHeights()[1];
                tlpMain.RowStyles[1].SizeType = SizeType.Absolute;
                tlpMain.RowStyles[1].Height = 0;
                pbExpandCollapse.Image = Resources.ArrowDown;
            }

            isCollapsed = !isCollapsed;
        }
    }
}
