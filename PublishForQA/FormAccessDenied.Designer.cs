namespace PublishForQA
{
    partial class FormAccessDenied
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccessDenied));
            this.lbAccessDenied = new System.Windows.Forms.ListBox();
            this.lblAccessDenied = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbAccessDenied
            // 
            this.lbAccessDenied.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAccessDenied.FormattingEnabled = true;
            this.lbAccessDenied.Location = new System.Drawing.Point(12, 27);
            this.lbAccessDenied.Name = "lbAccessDenied";
            this.lbAccessDenied.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAccessDenied.Size = new System.Drawing.Size(260, 225);
            this.lbAccessDenied.TabIndex = 0;
            this.lbAccessDenied.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbAccessDenied_KeyDown);
            // 
            // lblAccessDenied
            // 
            this.lblAccessDenied.AutoSize = true;
            this.lblAccessDenied.Font = new System.Drawing.Font("Microsoft Himalaya", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccessDenied.Location = new System.Drawing.Point(9, 9);
            this.lblAccessDenied.Name = "lblAccessDenied";
            this.lblAccessDenied.Size = new System.Drawing.Size(243, 15);
            this.lblAccessDenied.TabIndex = 1;
            this.lblAccessDenied.Text = "Access was denied to the following folders:";
            // 
            // FormAccessDenied
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lblAccessDenied);
            this.Controls.Add(this.lbAccessDenied);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAccessDenied";
            this.Text = "Access Denied Folders";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAccessDenied;
        private System.Windows.Forms.Label lblAccessDenied;
    }
}