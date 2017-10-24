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
            this.components = new System.ComponentModel.Container();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.lblECheck = new System.Windows.Forms.Label();
            this.lblCore = new System.Windows.Forms.Label();
            this.lblService = new System.Windows.Forms.Label();
            this.lblQAFolder = new System.Windows.Forms.Label();
            this.lblQAFolderPath = new System.Windows.Forms.Label();
            this.btnQAFolderBrowse = new System.Windows.Forms.Button();
            this.btnServiceBrowse = new System.Windows.Forms.Button();
            this.btnCoreBrowse = new System.Windows.Forms.Button();
            this.btnECheckBrowse = new System.Windows.Forms.Button();
            this.btnPublish = new System.Windows.Forms.Button();
            this.tbQAFolderPath = new System.Windows.Forms.TextBox();
            this.tbServicePath = new System.Windows.Forms.TextBox();
            this.tbCorePath = new System.Windows.Forms.TextBox();
            this.tbECheckPath = new System.Windows.Forms.TextBox();
            this.pbAccessDenied = new System.Windows.Forms.PictureBox();
            this.tbTaskName = new System.Windows.Forms.TextBox();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.btnLocate = new PublishForQA.MenuButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbAccessDenied)).BeginInit();
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
            // lblCore
            // 
            this.lblCore.AutoSize = true;
            this.lblCore.Location = new System.Drawing.Point(12, 111);
            this.lblCore.Name = "lblCore";
            this.lblCore.Size = new System.Drawing.Size(143, 13);
            this.lblCore.TabIndex = 0;
            this.lblCore.Text = "E-Check Core Debug Folder:";
            // 
            // lblService
            // 
            this.lblService.AutoSize = true;
            this.lblService.Location = new System.Drawing.Point(12, 182);
            this.lblService.Name = "lblService";
            this.lblService.Size = new System.Drawing.Size(157, 13);
            this.lblService.TabIndex = 0;
            this.lblService.Text = "E-Check Service Debug Folder:";
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
            this.btnQAFolderBrowse.Click += new System.EventHandler(this.Browse);
            // 
            // btnServiceBrowse
            // 
            this.btnServiceBrowse.Location = new System.Drawing.Point(317, 234);
            this.btnServiceBrowse.Name = "btnServiceBrowse";
            this.btnServiceBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnServiceBrowse.TabIndex = 2;
            this.btnServiceBrowse.Text = "Browse...";
            this.btnServiceBrowse.UseVisualStyleBackColor = true;
            this.btnServiceBrowse.Click += new System.EventHandler(this.Browse);
            // 
            // btnCoreBrowse
            // 
            this.btnCoreBrowse.Location = new System.Drawing.Point(317, 163);
            this.btnCoreBrowse.Name = "btnCoreBrowse";
            this.btnCoreBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCoreBrowse.TabIndex = 2;
            this.btnCoreBrowse.Text = "Browse...";
            this.btnCoreBrowse.UseVisualStyleBackColor = true;
            this.btnCoreBrowse.Click += new System.EventHandler(this.Browse);
            // 
            // btnECheckBrowse
            // 
            this.btnECheckBrowse.Location = new System.Drawing.Point(317, 92);
            this.btnECheckBrowse.Name = "btnECheckBrowse";
            this.btnECheckBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnECheckBrowse.TabIndex = 2;
            this.btnECheckBrowse.Text = "Browse...";
            this.btnECheckBrowse.UseVisualStyleBackColor = true;
            this.btnECheckBrowse.Click += new System.EventHandler(this.Browse);
            // 
            // btnPublish
            // 
            this.btnPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPublish.Location = new System.Drawing.Point(152, 385);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(100, 37);
            this.btnPublish.TabIndex = 3;
            this.btnPublish.Text = "Publish";
            this.btnPublish.UseVisualStyleBackColor = false;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // tbQAFolderPath
            // 
            this.tbQAFolderPath.AllowDrop = true;
            this.tbQAFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbQAFolderPath.Location = new System.Drawing.Point(12, 271);
            this.tbQAFolderPath.Multiline = true;
            this.tbQAFolderPath.Name = "tbQAFolderPath";
            this.tbQAFolderPath.Size = new System.Drawing.Size(380, 32);
            this.tbQAFolderPath.TabIndex = 9;
            this.tbQAFolderPath.Text = "E:\\Ogrest\\";
            this.tbQAFolderPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbQAFolderPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbQAFolderPath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbServicePath
            // 
            this.tbServicePath.AllowDrop = true;
            this.tbServicePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbServicePath.Location = new System.Drawing.Point(12, 200);
            this.tbServicePath.Multiline = true;
            this.tbServicePath.Name = "tbServicePath";
            this.tbServicePath.Size = new System.Drawing.Size(380, 32);
            this.tbServicePath.TabIndex = 9;
            this.tbServicePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbServicePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbServicePath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbCorePath
            // 
            this.tbCorePath.AllowDrop = true;
            this.tbCorePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCorePath.Location = new System.Drawing.Point(12, 129);
            this.tbCorePath.Multiline = true;
            this.tbCorePath.Name = "tbCorePath";
            this.tbCorePath.Size = new System.Drawing.Size(380, 32);
            this.tbCorePath.TabIndex = 9;
            this.tbCorePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbCorePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbCorePath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbECheckPath
            // 
            this.tbECheckPath.AllowDrop = true;
            this.tbECheckPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbECheckPath.Location = new System.Drawing.Point(12, 58);
            this.tbECheckPath.Multiline = true;
            this.tbECheckPath.Name = "tbECheckPath";
            this.tbECheckPath.Size = new System.Drawing.Size(380, 32);
            this.tbECheckPath.TabIndex = 9;
            this.tbECheckPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbECheckPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbECheckPath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // pbAccessDenied
            // 
            this.pbAccessDenied.Image = global::PublishForQA.Properties.Resources.AccessDeniedIcon;
            this.pbAccessDenied.Location = new System.Drawing.Point(265, 12);
            this.pbAccessDenied.Name = "pbAccessDenied";
            this.pbAccessDenied.Size = new System.Drawing.Size(25, 25);
            this.pbAccessDenied.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAccessDenied.TabIndex = 11;
            this.pbAccessDenied.TabStop = false;
            this.pbAccessDenied.Visible = false;
            this.pbAccessDenied.Click += new System.EventHandler(this.pbAccessDenied_Click);
            // 
            // tbTaskName
            // 
            this.tbTaskName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTaskName.Location = new System.Drawing.Point(27, 347);
            this.tbTaskName.Name = "tbTaskName";
            this.tbTaskName.Size = new System.Drawing.Size(350, 20);
            this.tbTaskName.TabIndex = 12;
            this.tbTaskName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbTaskName.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // lblTaskName
            // 
            this.lblTaskName.AutoSize = true;
            this.lblTaskName.Location = new System.Drawing.Point(173, 328);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(65, 13);
            this.lblTaskName.TabIndex = 13;
            this.lblTaskName.Text = "Task Name:";
            // 
            // btnLocate
            // 
            this.btnLocate.Location = new System.Drawing.Point(296, 13);
            this.btnLocate.Name = "btnLocate";
            this.btnLocate.Size = new System.Drawing.Size(75, 23);
            this.btnLocate.TabIndex = 10;
            this.btnLocate.Text = "Locate";
            this.btnLocate.UseVisualStyleBackColor = true;
            // 
            // FormPublisher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 433);
            this.Controls.Add(this.lblTaskName);
            this.Controls.Add(this.tbTaskName);
            this.Controls.Add(this.pbAccessDenied);
            this.Controls.Add(this.btnLocate);
            this.Controls.Add(this.tbECheckPath);
            this.Controls.Add(this.tbCorePath);
            this.Controls.Add(this.tbServicePath);
            this.Controls.Add(this.tbQAFolderPath);
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.btnECheckBrowse);
            this.Controls.Add(this.btnCoreBrowse);
            this.Controls.Add(this.btnServiceBrowse);
            this.Controls.Add(this.btnQAFolderBrowse);
            this.Controls.Add(this.lblQAFolderPath);
            this.Controls.Add(this.lblQAFolder);
            this.Controls.Add(this.lblService);
            this.Controls.Add(this.lblCore);
            this.Controls.Add(this.lblECheck);
            this.Name = "FormPublisher";
            this.Text = "Publisher";
            ((System.ComponentModel.ISupportInitialize)(this.pbAccessDenied)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label lblECheck;
        private System.Windows.Forms.Label lblCore;
        private System.Windows.Forms.Label lblService;
        private System.Windows.Forms.Label lblQAFolder;
        private System.Windows.Forms.Label lblQAFolderPath;
        private System.Windows.Forms.Button btnQAFolderBrowse;
        private System.Windows.Forms.Button btnServiceBrowse;
        private System.Windows.Forms.Button btnCoreBrowse;
        private System.Windows.Forms.Button btnECheckBrowse;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.TextBox tbQAFolderPath;
        private System.Windows.Forms.TextBox tbServicePath;
        private System.Windows.Forms.TextBox tbCorePath;
        private System.Windows.Forms.TextBox tbECheckPath;
        private MenuButton btnLocate;
        private System.Windows.Forms.PictureBox pbAccessDenied;
        private System.Windows.Forms.TextBox tbTaskName;
        private System.Windows.Forms.Label lblTaskName;
    }
}

