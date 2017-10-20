namespace PublishForQA
{
    partial class FormTooManyResults
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblResults = new System.Windows.Forms.Label();
            this.pECheck = new System.Windows.Forms.Panel();
            this.lbECheck = new System.Windows.Forms.ListBox();
            this.lblECheck = new System.Windows.Forms.Label();
            this.pCore = new System.Windows.Forms.Panel();
            this.lbCore = new System.Windows.Forms.ListBox();
            this.lblCore = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.tlpMain.SuspendLayout();
            this.pECheck.SuspendLayout();
            this.pCore.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.lblResults, 0, 0);
            this.tlpMain.Controls.Add(this.pECheck, 0, 1);
            this.tlpMain.Controls.Add(this.pCore, 0, 2);
            this.tlpMain.Controls.Add(this.btnOK, 0, 3);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpMain.Size = new System.Drawing.Size(457, 501);
            this.tlpMain.TabIndex = 0;
            // 
            // lblResults
            // 
            this.lblResults.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResults.Location = new System.Drawing.Point(4, 4);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(260, 34);
            this.lblResults.TabIndex = 0;
            this.lblResults.Text = "More than one result was found.  Please choose which one is to be used.";
            this.lblResults.UseMnemonic = false;
            // 
            // pECheck
            // 
            this.pECheck.Controls.Add(this.lbECheck);
            this.pECheck.Controls.Add(this.lblECheck);
            this.pECheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pECheck.Location = new System.Drawing.Point(4, 45);
            this.pECheck.Name = "pECheck";
            this.pECheck.Size = new System.Drawing.Size(449, 202);
            this.pECheck.TabIndex = 1;
            // 
            // lbECheck
            // 
            this.lbECheck.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbECheck.FormattingEnabled = true;
            this.lbECheck.Location = new System.Drawing.Point(3, 16);
            this.lbECheck.Name = "lbECheck";
            this.lbECheck.Size = new System.Drawing.Size(443, 173);
            this.lbECheck.TabIndex = 1;
            // 
            // lblECheck
            // 
            this.lblECheck.AutoSize = true;
            this.lblECheck.Location = new System.Drawing.Point(3, 0);
            this.lblECheck.Name = "lblECheck";
            this.lblECheck.Size = new System.Drawing.Size(91, 13);
            this.lblECheck.TabIndex = 0;
            this.lblECheck.Text = "E-Check location:";
            this.lblECheck.UseMnemonic = false;
            // 
            // pCore
            // 
            this.pCore.Controls.Add(this.lbCore);
            this.pCore.Controls.Add(this.lblCore);
            this.pCore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pCore.Location = new System.Drawing.Point(4, 254);
            this.pCore.Name = "pCore";
            this.pCore.Size = new System.Drawing.Size(449, 202);
            this.pCore.TabIndex = 2;
            // 
            // lbCore
            // 
            this.lbCore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbCore.FormattingEnabled = true;
            this.lbCore.Location = new System.Drawing.Point(3, 16);
            this.lbCore.Name = "lbCore";
            this.lbCore.Size = new System.Drawing.Size(443, 173);
            this.lbCore.TabIndex = 3;
            // 
            // lblCore
            // 
            this.lblCore.AutoSize = true;
            this.lblCore.Location = new System.Drawing.Point(3, 0);
            this.lblCore.Name = "lblCore";
            this.lblCore.Size = new System.Drawing.Size(113, 13);
            this.lblCore.TabIndex = 2;
            this.lblCore.Text = "E-CheckCore location:";
            this.lblCore.UseMnemonic = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOK.Location = new System.Drawing.Point(378, 468);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // FormTooManyResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(457, 501);
            this.Controls.Add(this.tlpMain);
            this.MinimizeBox = false;
            this.Name = "FormTooManyResults";
            this.Text = "Choose Paths";
            this.tlpMain.ResumeLayout(false);
            this.pECheck.ResumeLayout(false);
            this.pECheck.PerformLayout();
            this.pCore.ResumeLayout(false);
            this.pCore.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.Panel pECheck;
        private System.Windows.Forms.Panel pCore;
        private System.Windows.Forms.Label lblECheck;
        private System.Windows.Forms.ListBox lbECheck;
        private System.Windows.Forms.ListBox lbCore;
        private System.Windows.Forms.Label lblCore;
        private System.Windows.Forms.Button btnOK;
    }
}