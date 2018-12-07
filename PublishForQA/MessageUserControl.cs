using System.Windows.Forms;
using static PublishForQA.Enums;

namespace PublishForQA
{
    public partial class MessageUserControl : UserControl
    {
        public MessageUserControl(string message, MessageUserControlIcons icon, MessageUserControlButtons buttons)
        {
            InitializeComponent();
        }

        private void btnFix_Click(object sender, System.EventArgs e)
        {

        }
    }
}
