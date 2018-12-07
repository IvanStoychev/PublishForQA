using System;
using System.Windows.Forms;
using static PublishForQA.Enums;

namespace PublishForQA
{
    public partial class MessageUserControl : UserControl
    {
        Func<bool> fixMethod = null;

        public MessageUserControl(string message, MessageUserControlIcons icon, MessageUserControlButtons buttons, Func<bool> func = null)
        {
            InitializeComponent();
            lblMessage.Text = message;
            fixMethod = func;

            switch (icon)
            {
                case MessageUserControlIcons.Warning:
                    pbIcon.Image = Properties.Resources.Warning;
                    break;
                case MessageUserControlIcons.Error:
                    pbIcon.Image = Properties.Resources.Error;
                    break;
                default:
                    break;
            }

            switch (buttons)
            {
                case MessageUserControlButtons.None:
                    btnFix.Visible = false;
                    break;
                case MessageUserControlButtons.Fix:
                    break;
                default:
                    break;
            }
        }

        private void btnFix_Click(object sender, EventArgs e)
        {
            btnFix.Enabled = !fixMethod();
        }
    }
}
