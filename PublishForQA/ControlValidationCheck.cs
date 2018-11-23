using System;
using System.Windows.Forms;
using PublishForQA.Properties;

namespace PublishForQA
{
    /// <summary>
    /// A user control displaying information on a failed validation test.
    /// </summary>
    public partial class ControlValidationCheck : UserControl
    {
        bool isCollapsed;

        /// <summary>
        /// Creates a new instance of this control with the given name, details and icon.
        /// </summary>
        /// <param name="validationName">The name or title of the failed validation.</param>
        /// <param name="validationDetails">The details on why the validation failed.</param>
        /// <param name="errorIcon">The type of icon to dispay.</param>
        public ControlValidationCheck(string validationName, string validationDetails, ErrorIcon errorIcon)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.tlpTitle.BorderStyle = BorderStyle.FixedSingle;
            lblValidationName.Text = validationName;
            lblValidationDetails.Text = validationDetails;

            switch (errorIcon)
            {
                case ErrorIcon.Error:
                    pbError.Image = Resources.Error;
                    break;
                case ErrorIcon.Warning:
                    pbError.Image = Resources.Warning;
                    break;
                default:
                    break;
            }
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
