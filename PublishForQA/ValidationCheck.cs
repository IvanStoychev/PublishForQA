using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PublishForQA
{
    public partial class ValidationCheck : UserControl
    {
        bool isCollapsed = true;

        public ValidationCheck(string validationName, string validationDetails)
        {
            InitializeComponent();
            lblValidationName.Text = validationName;
            lblValidationDetails.Text = validationDetails;
        }

        private void pbExpandCollapse_Click(object sender, EventArgs e)
        {

        }
    }
}
