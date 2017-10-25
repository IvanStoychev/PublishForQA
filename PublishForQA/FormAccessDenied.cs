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
    public partial class FormAccessDenied : Form
    {
        public FormAccessDenied(List<string> accessDeniedFolders)
        {
            InitializeComponent();
            lbAccessDenied.DataSource = accessDeniedFolders;
        }

        private void lbAccessDenied_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                e.SuppressKeyPress = true;
                CopySelectedValuesToClipboard();
            }
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (string item in lbAccessDenied.SelectedItems)
            {
                builder.AppendLine(item);
            }

            Clipboard.SetText(builder.ToString());
        }
    }
}
