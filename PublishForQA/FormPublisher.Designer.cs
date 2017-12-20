﻿namespace PublishForQA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPublisher));
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.lblECheck = new System.Windows.Forms.Label();
            this.lblCore = new System.Windows.Forms.Label();
            this.lblService = new System.Windows.Forms.Label();
            this.lblQAFolder = new System.Windows.Forms.Label();
            this.btnQAFolderBrowse = new System.Windows.Forms.Button();
            this.btnServiceBrowse = new System.Windows.Forms.Button();
            this.btnCoreBrowse = new System.Windows.Forms.Button();
            this.btnECheckBrowse = new System.Windows.Forms.Button();
            this.btnPublish = new System.Windows.Forms.Button();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tbECheckPath = new PublishForQA.TextBoxFormPublisher();
            this.tbCorePath = new PublishForQA.TextBoxFormPublisher();
            this.tbServicePath = new PublishForQA.TextBoxFormPublisher();
            this.tbQAFolderPath = new PublishForQA.TextBoxFormPublisher();
            this.tbTaskName = new PublishForQA.TextBoxFormPublisher();
            this.btnLocate = new PublishForQA.MenuButton();
            this.pbCopyToClipboard = new System.Windows.Forms.PictureBox();
            this.pbLoad = new System.Windows.Forms.PictureBox();
            this.pbSave = new System.Windows.Forms.PictureBox();
            this.pbHelp = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyToClipboard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHelp)).BeginInit();
            this.SuspendLayout();
            // 
            // lblECheck
            // 
            this.lblECheck.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblECheck.AutoSize = true;
            this.lblECheck.Location = new System.Drawing.Point(12, 40);
            this.lblECheck.Name = "lblECheck";
            this.lblECheck.Size = new System.Drawing.Size(118, 13);
            this.lblECheck.TabIndex = 0;
            this.lblECheck.Text = "E-Check Debug Folder:";
            // 
            // lblCore
            // 
            this.lblCore.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCore.AutoSize = true;
            this.lblCore.Location = new System.Drawing.Point(12, 111);
            this.lblCore.Name = "lblCore";
            this.lblCore.Size = new System.Drawing.Size(143, 13);
            this.lblCore.TabIndex = 0;
            this.lblCore.Text = "E-Check Core Debug Folder:";
            // 
            // lblService
            // 
            this.lblService.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblService.AutoSize = true;
            this.lblService.Location = new System.Drawing.Point(12, 182);
            this.lblService.Name = "lblService";
            this.lblService.Size = new System.Drawing.Size(157, 13);
            this.lblService.TabIndex = 0;
            this.lblService.Text = "E-Check Service Debug Folder:";
            // 
            // lblQAFolder
            // 
            this.lblQAFolder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblQAFolder.AutoSize = true;
            this.lblQAFolder.Location = new System.Drawing.Point(12, 253);
            this.lblQAFolder.Name = "lblQAFolder";
            this.lblQAFolder.Size = new System.Drawing.Size(82, 13);
            this.lblQAFolder.TabIndex = 0;
            this.lblQAFolder.Text = "Your QA Folder:";
            // 
            // btnQAFolderBrowse
            // 
            this.btnQAFolderBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
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
            this.btnServiceBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
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
            this.btnCoreBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
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
            this.btnECheckBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
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
            this.btnPublish.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPublish.Location = new System.Drawing.Point(152, 385);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(100, 37);
            this.btnPublish.TabIndex = 3;
            this.btnPublish.Text = "Publish";
            this.btnPublish.UseVisualStyleBackColor = false;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // lblTaskName
            // 
            this.lblTaskName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTaskName.AutoSize = true;
            this.lblTaskName.Location = new System.Drawing.Point(173, 328);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(65, 13);
            this.lblTaskName.TabIndex = 13;
            this.lblTaskName.Text = "Task Name:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Text files|*.txt";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // tbECheckPath
            // 
            this.tbECheckPath.AllowDrop = true;
            this.tbECheckPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbECheckPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbECheckPath.Location = new System.Drawing.Point(12, 58);
            this.tbECheckPath.Multiline = true;
            this.tbECheckPath.Name = "tbECheckPath";
            this.tbECheckPath.Size = new System.Drawing.Size(380, 32);
            this.tbECheckPath.TabIndex = 20;
            this.tbECheckPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbECheckPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbECheckPath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbCorePath
            // 
            this.tbCorePath.AllowDrop = true;
            this.tbCorePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCorePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCorePath.Location = new System.Drawing.Point(12, 129);
            this.tbCorePath.Multiline = true;
            this.tbCorePath.Name = "tbCorePath";
            this.tbCorePath.Size = new System.Drawing.Size(380, 32);
            this.tbCorePath.TabIndex = 19;
            this.tbCorePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbCorePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbCorePath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbServicePath
            // 
            this.tbServicePath.AllowDrop = true;
            this.tbServicePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServicePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbServicePath.Location = new System.Drawing.Point(12, 200);
            this.tbServicePath.Multiline = true;
            this.tbServicePath.Name = "tbServicePath";
            this.tbServicePath.Size = new System.Drawing.Size(380, 32);
            this.tbServicePath.TabIndex = 18;
            this.tbServicePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbServicePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbServicePath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbQAFolderPath
            // 
            this.tbQAFolderPath.AllowDrop = true;
            this.tbQAFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbQAFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbQAFolderPath.Location = new System.Drawing.Point(12, 271);
            this.tbQAFolderPath.Multiline = true;
            this.tbQAFolderPath.Name = "tbQAFolderPath";
            this.tbQAFolderPath.Size = new System.Drawing.Size(380, 32);
            this.tbQAFolderPath.TabIndex = 17;
            this.tbQAFolderPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbQAFolderPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbQAFolderPath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbTaskName
            // 
            this.tbTaskName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTaskName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTaskName.Location = new System.Drawing.Point(27, 347);
            this.tbTaskName.Name = "tbTaskName";
            this.tbTaskName.Size = new System.Drawing.Size(350, 20);
            this.tbTaskName.TabIndex = 16;
            this.tbTaskName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbTaskName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbTaskName.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // btnLocate
            // 
            this.btnLocate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLocate.Location = new System.Drawing.Point(296, 13);
            this.btnLocate.Name = "btnLocate";
            this.btnLocate.Size = new System.Drawing.Size(75, 23);
            this.btnLocate.TabIndex = 10;
            this.btnLocate.Text = "Locate";
            this.btnLocate.UseVisualStyleBackColor = true;
            // 
            // pbCopyToClipboard
            // 
            this.pbCopyToClipboard.Image = ((System.Drawing.Image)(resources.GetObject("pbCopyToClipboard.Image")));
            this.pbCopyToClipboard.Location = new System.Drawing.Point(270, 387);
            this.pbCopyToClipboard.Name = "pbCopyToClipboard";
            this.pbCopyToClipboard.Size = new System.Drawing.Size(27, 32);
            this.pbCopyToClipboard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCopyToClipboard.TabIndex = 21;
            this.pbCopyToClipboard.TabStop = false;
            this.pbCopyToClipboard.Click += new System.EventHandler(this.pbCopyToClipboard_Click);
            // 
            // pbLoad
            // 
            this.pbLoad.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pbLoad.Image = global::PublishForQA.Properties.Resources.Load;
            this.pbLoad.Location = new System.Drawing.Point(266, 12);
            this.pbLoad.Name = "pbLoad";
            this.pbLoad.Size = new System.Drawing.Size(25, 25);
            this.pbLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLoad.TabIndex = 15;
            this.pbLoad.TabStop = false;
            this.pbLoad.Click += new System.EventHandler(this.pbLoad_Click);
            // 
            // pbSave
            // 
            this.pbSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pbSave.Image = global::PublishForQA.Properties.Resources.Save;
            this.pbSave.Location = new System.Drawing.Point(234, 12);
            this.pbSave.Name = "pbSave";
            this.pbSave.Size = new System.Drawing.Size(25, 25);
            this.pbSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSave.TabIndex = 15;
            this.pbSave.TabStop = false;
            this.pbSave.Click += new System.EventHandler(this.pbSave_Click);
            // 
            // pbHelp
            // 
            this.pbHelp.Image = global::PublishForQA.Properties.Resources.icon_help_circled_128;
            this.pbHelp.Location = new System.Drawing.Point(1, 1);
            this.pbHelp.Name = "pbHelp";
            this.pbHelp.Size = new System.Drawing.Size(18, 18);
            this.pbHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbHelp.TabIndex = 14;
            this.pbHelp.TabStop = false;
            this.pbHelp.Click += new System.EventHandler(this.pbHelp_Click);
            // 
            // FormPublisher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 433);
            this.Controls.Add(this.pbCopyToClipboard);
            this.Controls.Add(this.tbECheckPath);
            this.Controls.Add(this.tbCorePath);
            this.Controls.Add(this.tbServicePath);
            this.Controls.Add(this.tbQAFolderPath);
            this.Controls.Add(this.tbTaskName);
            this.Controls.Add(this.pbLoad);
            this.Controls.Add(this.pbSave);
            this.Controls.Add(this.pbHelp);
            this.Controls.Add(this.lblTaskName);
            this.Controls.Add(this.btnLocate);
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.btnECheckBrowse);
            this.Controls.Add(this.btnCoreBrowse);
            this.Controls.Add(this.btnServiceBrowse);
            this.Controls.Add(this.btnQAFolderBrowse);
            this.Controls.Add(this.lblQAFolder);
            this.Controls.Add(this.lblService);
            this.Controls.Add(this.lblCore);
            this.Controls.Add(this.lblECheck);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPublisher";
            this.Text = "Publisher Alpha v1.0";
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyToClipboard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHelp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label lblECheck;
        private System.Windows.Forms.Label lblCore;
        private System.Windows.Forms.Label lblService;
        private System.Windows.Forms.Label lblQAFolder;
        private System.Windows.Forms.Button btnQAFolderBrowse;
        private System.Windows.Forms.Button btnServiceBrowse;
        private System.Windows.Forms.Button btnCoreBrowse;
        private System.Windows.Forms.Button btnECheckBrowse;
        private System.Windows.Forms.Button btnPublish;
        private MenuButton btnLocate;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.PictureBox pbHelp;
        private System.Windows.Forms.PictureBox pbSave;
        private System.Windows.Forms.PictureBox pbLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private TextBoxFormPublisher tbTaskName;
        private TextBoxFormPublisher tbQAFolderPath;
        private TextBoxFormPublisher tbServicePath;
        private TextBoxFormPublisher tbCorePath;
        private TextBoxFormPublisher tbECheckPath;
        private System.Windows.Forms.PictureBox pbCopyToClipboard;
    }
}

