﻿using System;
using System.Windows.Forms;

namespace PublishForQA
{
    public partial class ValidationCheck : UserControl
    {
        bool isCollapsed;

        public ValidationCheck(string validationName, string validationDetails)
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
            }
            else
            {
                tlpMain.RowStyles[1].SizeType = SizeType.Absolute;
                tlpMain.RowStyles[1].Height = 0;
            }

            isCollapsed = !isCollapsed;
        }
    }
}
