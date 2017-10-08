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
    public partial class FormPublisher : Form
    {
        public FormPublisher()
        {
            InitializeComponent();
        }

        private void btnLocate_Click(object sender, EventArgs e)
        {

        }

        private void btnECheckBrowse_Click(object sender, EventArgs e)
        {
            Browse(lblECheckPath);
        }

        private void btnECheckCoreBrowse_Click(object sender, EventArgs e)
        {
            Browse(lblECheckCorePath);
        }

        private void btnECheckServiceBrowse_Click(object sender, EventArgs e)
        {
            Browse(lblECheckServicePath);
        }

        private void btnQAFolderBrowse_Click(object sender, EventArgs e)
        {
            Browse(lblQAFolderPath);
        }

        private void Browse(Label label)
        {
            folderBrowserDialog.ShowDialog();
            label.Text = folderBrowserDialog.SelectedPath;
        }
    }
}