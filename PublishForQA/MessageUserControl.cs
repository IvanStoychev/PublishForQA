using System;
using System.Windows.Forms;
using static PublishForQA.Enums;

namespace PublishForQA
{
    /// <summary>
    /// Displays an error or warning message, concerning failed validations
    /// for the copy operation.
    /// </summary>
    public partial class MessageUserControl : UserControl
    {
        /// <summary>
        /// The method the "Fix" button will call when clicked.
        /// </summary>
        Func<bool> fixMethod = null;

        /// <summary>
        /// Creates a new instance of the MessageUserControl class, displaying the given message, icon and buttons.
        /// </summary>
        /// <param name="message">The message to display in the label control.</param>
        /// <param name="icon">The type of icon to display.</param>
        /// <param name="buttons">The type of buttons to display.</param>
        /// <param name="func">The method to be called on button click.</param>
        public MessageUserControl(string message, MessageUserControlIcons icon, MessageUserControlButtons buttons, Func<bool> func = null)
        {
            InitializeComponent();
            lblMessage.Text = message;
            btnFix.Visible = buttons == MessageUserControlButtons.Fix;
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

            // Extra precaution.
            if (func == null) btnFix.Visible = false;
        }

        private void btnFix_Click(object sender, EventArgs e)
        {
            // If the fixMethod succeeds, and thus returns "true",
            // disable the button so the operation can't be doubled.
            btnFix.Enabled = !fixMethod();
        }
    }
}
