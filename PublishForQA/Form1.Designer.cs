namespace PublishForQA
{
    partial class Form1
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
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.lblECheck = new System.Windows.Forms.Label();
            this.lblECheckCore = new System.Windows.Forms.Label();
            this.lblECheckService = new System.Windows.Forms.Label();
            this.lblQAFolder = new System.Windows.Forms.Label();
            this.lblECheckPath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblECheck
            // 
            this.lblECheck.AutoSize = true;
            this.lblECheck.Location = new System.Drawing.Point(12, 9);
            this.lblECheck.Name = "lblECheck";
            this.lblECheck.Size = new System.Drawing.Size(83, 13);
            this.lblECheck.TabIndex = 0;
            this.lblECheck.Text = "E-Check Folder:";
            // 
            // lblECheckCore
            // 
            this.lblECheckCore.AutoSize = true;
            this.lblECheckCore.Location = new System.Drawing.Point(12, 69);
            this.lblECheckCore.Name = "lblECheckCore";
            this.lblECheckCore.Size = new System.Drawing.Size(108, 13);
            this.lblECheckCore.TabIndex = 0;
            this.lblECheckCore.Text = "E-Check Core Folder:";
            // 
            // lblECheckService
            // 
            this.lblECheckService.AutoSize = true;
            this.lblECheckService.Location = new System.Drawing.Point(12, 129);
            this.lblECheckService.Name = "lblECheckService";
            this.lblECheckService.Size = new System.Drawing.Size(122, 13);
            this.lblECheckService.TabIndex = 0;
            this.lblECheckService.Text = "E-Check Service Folder:";
            // 
            // lblQAFolder
            // 
            this.lblQAFolder.AutoSize = true;
            this.lblQAFolder.Location = new System.Drawing.Point(12, 189);
            this.lblQAFolder.Name = "lblQAFolder";
            this.lblQAFolder.Size = new System.Drawing.Size(82, 13);
            this.lblQAFolder.TabIndex = 0;
            this.lblQAFolder.Text = "Your QA Folder:";
            // 
            // lblECheckPath
            // 
            this.lblECheckPath.AutoEllipsis = true;
            this.lblECheckPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblECheckPath.Location = new System.Drawing.Point(12, 25);
            this.lblECheckPath.Name = "lblECheckPath";
            this.lblECheckPath.Size = new System.Drawing.Size(380, 28);
            this.lblECheckPath.TabIndex = 1;
            this.lblECheckPath.Text = "C:\\Development\\Workspaces\\SmartIT\\master\\E-Check\\E-Check\\master\\WinClient\\master\\" +
    "E-Check\\AppData\\bin\\debug";
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(380, 28);
            this.label1.TabIndex = 1;
            this.label1.Text = "C:\\Development\\Workspaces\\SmartIT\\master\\E-Check\\E-Check\\master\\WinClient\\master\\" +
    "E-Check\\AppData\\bin\\debug";
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(12, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(380, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "C:\\Development\\Workspaces\\SmartIT\\master\\E-Check\\E-Check\\master\\WinClient\\master\\" +
    "E-Check\\AppData\\bin\\debug";
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(12, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(380, 28);
            this.label3.TabIndex = 1;
            this.label3.Text = "C:\\Development\\Workspaces\\SmartIT\\master\\E-Check\\E-Check\\master\\WinClient\\master\\" +
    "E-Check\\AppData\\bin\\debug";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 389);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblECheckPath);
            this.Controls.Add(this.lblQAFolder);
            this.Controls.Add(this.lblECheckService);
            this.Controls.Add(this.lblECheckCore);
            this.Controls.Add(this.lblECheck);
            this.Name = "Form1";
            this.Text = "Publisher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label lblECheck;
        private System.Windows.Forms.Label lblECheckCore;
        private System.Windows.Forms.Label lblECheckService;
        private System.Windows.Forms.Label lblQAFolder;
        private System.Windows.Forms.Label lblECheckPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

