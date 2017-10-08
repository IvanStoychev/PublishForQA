namespace PublishForQA
{
    partial class FormPublisher
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
            this.lblECheckCorePath = new System.Windows.Forms.Label();
            this.lblECheckServicePath = new System.Windows.Forms.Label();
            this.lblQAFolderPath = new System.Windows.Forms.Label();
            this.btnQAFolderBrowse = new System.Windows.Forms.Button();
            this.btnECheckServiceBrowse = new System.Windows.Forms.Button();
            this.btnECheckCoreBrowse = new System.Windows.Forms.Button();
            this.btnECheckBrowse = new System.Windows.Forms.Button();
            this.btnPublish = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.cbVersions = new System.Windows.Forms.ComboBox();
            this.btnLocate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblECheck
            // 
            this.lblECheck.AutoSize = true;
            this.lblECheck.Location = new System.Drawing.Point(12, 40);
            this.lblECheck.Name = "lblECheck";
            this.lblECheck.Size = new System.Drawing.Size(118, 13);
            this.lblECheck.TabIndex = 0;
            this.lblECheck.Text = "E-Check Debug Folder:";
            // 
            // lblECheckCore
            // 
            this.lblECheckCore.AutoSize = true;
            this.lblECheckCore.Location = new System.Drawing.Point(12, 111);
            this.lblECheckCore.Name = "lblECheckCore";
            this.lblECheckCore.Size = new System.Drawing.Size(143, 13);
            this.lblECheckCore.TabIndex = 0;
            this.lblECheckCore.Text = "E-Check Core Debug Folder:";
            // 
            // lblECheckService
            // 
            this.lblECheckService.AutoSize = true;
            this.lblECheckService.Location = new System.Drawing.Point(12, 182);
            this.lblECheckService.Name = "lblECheckService";
            this.lblECheckService.Size = new System.Drawing.Size(157, 13);
            this.lblECheckService.TabIndex = 0;
            this.lblECheckService.Text = "E-Check Service Debug Folder:";
            // 
            // lblQAFolder
            // 
            this.lblQAFolder.AutoSize = true;
            this.lblQAFolder.Location = new System.Drawing.Point(12, 253);
            this.lblQAFolder.Name = "lblQAFolder";
            this.lblQAFolder.Size = new System.Drawing.Size(82, 13);
            this.lblQAFolder.TabIndex = 0;
            this.lblQAFolder.Text = "Your QA Folder:";
            // 
            // lblECheckPath
            // 
            this.lblECheckPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblECheckPath.Location = new System.Drawing.Point(12, 61);
            this.lblECheckPath.Name = "lblECheckPath";
            this.lblECheckPath.Size = new System.Drawing.Size(380, 28);
            this.lblECheckPath.TabIndex = 1;
            this.lblECheckPath.Text = "C:\\Development\\Workspaces\\SmartIT\\master\\E-Check\\E-Check\\master\\WinClient\\master\\" +
    "E-Check\\AppData\\bin\\debug";
            // 
            // lblECheckCorePath
            // 
            this.lblECheckCorePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblECheckCorePath.Location = new System.Drawing.Point(12, 132);
            this.lblECheckCorePath.Name = "lblECheckCorePath";
            this.lblECheckCorePath.Size = new System.Drawing.Size(380, 28);
            this.lblECheckCorePath.TabIndex = 1;
            this.lblECheckCorePath.Text = "C:\\Development\\Workspaces\\SmartIT\\master\\E-Check\\E-Check\\master\\WinClient\\master\\" +
    "E-Check\\AppData\\bin\\debug";
            // 
            // lblECheckServicePath
            // 
            this.lblECheckServicePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblECheckServicePath.Location = new System.Drawing.Point(12, 203);
            this.lblECheckServicePath.Name = "lblECheckServicePath";
            this.lblECheckServicePath.Size = new System.Drawing.Size(380, 28);
            this.lblECheckServicePath.TabIndex = 1;
            this.lblECheckServicePath.Text = "C:\\Development\\Workspaces\\SmartIT\\master\\E-Check\\E-Check\\master\\WinClient\\master\\" +
    "E-Check\\AppData\\bin\\debug";
            // 
            // lblQAFolderPath
            // 
            this.lblQAFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblQAFolderPath.Location = new System.Drawing.Point(12, 274);
            this.lblQAFolderPath.Name = "lblQAFolderPath";
            this.lblQAFolderPath.Size = new System.Drawing.Size(380, 28);
            this.lblQAFolderPath.TabIndex = 1;
            this.lblQAFolderPath.Text = "C:\\Development\\Workspaces\\SmartIT\\master\\E-Check\\E-Check\\master\\WinClient\\master\\" +
    "E-Check\\AppData\\bin\\debug";
            // 
            // btnQAFolderBrowse
            // 
            this.btnQAFolderBrowse.Location = new System.Drawing.Point(317, 305);
            this.btnQAFolderBrowse.Name = "btnQAFolderBrowse";
            this.btnQAFolderBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnQAFolderBrowse.TabIndex = 2;
            this.btnQAFolderBrowse.Text = "Browse...";
            this.btnQAFolderBrowse.UseVisualStyleBackColor = true;
            // 
            // btnECheckServiceBrowse
            // 
            this.btnECheckServiceBrowse.Location = new System.Drawing.Point(317, 234);
            this.btnECheckServiceBrowse.Name = "btnECheckServiceBrowse";
            this.btnECheckServiceBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnECheckServiceBrowse.TabIndex = 2;
            this.btnECheckServiceBrowse.Text = "Browse...";
            this.btnECheckServiceBrowse.UseVisualStyleBackColor = true;
            // 
            // btnECheckCoreBrowse
            // 
            this.btnECheckCoreBrowse.Location = new System.Drawing.Point(317, 163);
            this.btnECheckCoreBrowse.Name = "btnECheckCoreBrowse";
            this.btnECheckCoreBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnECheckCoreBrowse.TabIndex = 2;
            this.btnECheckCoreBrowse.Text = "Browse...";
            this.btnECheckCoreBrowse.UseVisualStyleBackColor = true;
            // 
            // btnECheckBrowse
            // 
            this.btnECheckBrowse.Location = new System.Drawing.Point(317, 92);
            this.btnECheckBrowse.Name = "btnECheckBrowse";
            this.btnECheckBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnECheckBrowse.TabIndex = 2;
            this.btnECheckBrowse.Text = "Browse...";
            this.btnECheckBrowse.UseVisualStyleBackColor = true;
            // 
            // btnPublish
            // 
            this.btnPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPublish.Location = new System.Drawing.Point(160, 352);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(100, 37);
            this.btnPublish.TabIndex = 3;
            this.btnPublish.Text = "Publish";
            this.btnPublish.UseVisualStyleBackColor = false;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblVersion.Location = new System.Drawing.Point(50, 2);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(160, 23);
            this.lblVersion.TabIndex = 5;
            this.lblVersion.Text = "E-Check Version to publish:";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbVersions
            // 
            this.cbVersions.FormattingEnabled = true;
            this.cbVersions.Items.AddRange(new object[] {
            "Viva2012"});
            this.cbVersions.Location = new System.Drawing.Point(216, 5);
            this.cbVersions.Name = "cbVersions";
            this.cbVersions.Size = new System.Drawing.Size(79, 21);
            this.cbVersions.TabIndex = 7;
            // 
            // btnLocate
            // 
            this.btnLocate.Location = new System.Drawing.Point(312, 5);
            this.btnLocate.Name = "btnLocate";
            this.btnLocate.Size = new System.Drawing.Size(53, 23);
            this.btnLocate.TabIndex = 8;
            this.btnLocate.Text = "Locate";
            this.btnLocate.UseVisualStyleBackColor = true;
            this.btnLocate.Click += new System.EventHandler(this.btnLocate_Click);
            // 
            // FormPublisher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 398);
            this.Controls.Add(this.btnLocate);
            this.Controls.Add(this.cbVersions);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.btnECheckBrowse);
            this.Controls.Add(this.btnECheckCoreBrowse);
            this.Controls.Add(this.btnECheckServiceBrowse);
            this.Controls.Add(this.btnQAFolderBrowse);
            this.Controls.Add(this.lblQAFolderPath);
            this.Controls.Add(this.lblECheckServicePath);
            this.Controls.Add(this.lblECheckCorePath);
            this.Controls.Add(this.lblECheckPath);
            this.Controls.Add(this.lblQAFolder);
            this.Controls.Add(this.lblECheckService);
            this.Controls.Add(this.lblECheckCore);
            this.Controls.Add(this.lblECheck);
            this.Name = "FormPublisher";
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
        private System.Windows.Forms.Label lblECheckCorePath;
        private System.Windows.Forms.Label lblECheckServicePath;
        private System.Windows.Forms.Label lblQAFolderPath;
        private System.Windows.Forms.Button btnQAFolderBrowse;
        private System.Windows.Forms.Button btnECheckServiceBrowse;
        private System.Windows.Forms.Button btnECheckCoreBrowse;
        private System.Windows.Forms.Button btnECheckBrowse;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ComboBox cbVersions;
        private System.Windows.Forms.Button btnLocate;
    }
}

