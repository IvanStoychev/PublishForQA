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
            //cbVersions.Items.AddRange();
        }

        private void btnLocate_Click(object sender, EventArgs e)
        {

        }

        private void btnECheckBrowse_Click(object sender, EventArgs e)
        {
            Browse(tbECheckPath);
        }

        private void btnECheckCoreBrowse_Click(object sender, EventArgs e)
        {
            Browse(tbECheckCorePath);
        }

        private void btnECheckServiceBrowse_Click(object sender, EventArgs e)
        {
            Browse(tbECheckServicePath);
        }

        private void btnQAFolderBrowse_Click(object sender, EventArgs e)
        {
            Browse(tbQAFolderPath);
        }

        private void Browse(TextBox textBox)
        {
            folderBrowserDialog.ShowDialog();
            textBox.Text = folderBrowserDialog.SelectedPath;
        }
    }
}