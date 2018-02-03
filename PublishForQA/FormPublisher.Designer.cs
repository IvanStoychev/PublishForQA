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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pTop = new System.Windows.Forms.Panel();
            this.pbLoadDropdown = new System.Windows.Forms.PictureBox();
            this.pBottom = new System.Windows.Forms.Panel();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyToClipboard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHelp)).BeginInit();
            this.tlpMain.SuspendLayout();
            this.pTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadDropdown)).BeginInit();
            this.pBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblECheck
            // 
            this.lblECheck.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblECheck.AutoSize = true;
            this.lblECheck.Location = new System.Drawing.Point(3, 41);
            this.lblECheck.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblECheck.Name = "lblECheck";
            this.lblECheck.Size = new System.Drawing.Size(118, 13);
            this.lblECheck.TabIndex = 0;
            this.lblECheck.Text = "E-Check Debug Folder:";
            // 
            // lblCore
            // 
            this.lblCore.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCore.AutoSize = true;
            this.lblCore.Location = new System.Drawing.Point(3, 118);
            this.lblCore.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblCore.Name = "lblCore";
            this.lblCore.Size = new System.Drawing.Size(143, 13);
            this.lblCore.TabIndex = 0;
            this.lblCore.Text = "E-Check Core Debug Folder:";
            // 
            // lblService
            // 
            this.lblService.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblService.AutoSize = true;
            this.lblService.Location = new System.Drawing.Point(3, 195);
            this.lblService.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblService.Name = "lblService";
            this.lblService.Size = new System.Drawing.Size(157, 13);
            this.lblService.TabIndex = 0;
            this.lblService.Text = "E-Check Service Debug Folder:";
            // 
            // lblQAFolder
            // 
            this.lblQAFolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblQAFolder.AutoSize = true;
            this.lblQAFolder.Location = new System.Drawing.Point(3, 272);
            this.lblQAFolder.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblQAFolder.Name = "lblQAFolder";
            this.lblQAFolder.Size = new System.Drawing.Size(82, 13);
            this.lblQAFolder.TabIndex = 0;
            this.lblQAFolder.Text = "Your QA Folder:";
            // 
            // btnQAFolderBrowse
            // 
            this.btnQAFolderBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnQAFolderBrowse.Location = new System.Drawing.Point(326, 324);
            this.btnQAFolderBrowse.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnQAFolderBrowse.Name = "btnQAFolderBrowse";
            this.btnQAFolderBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnQAFolderBrowse.TabIndex = 8;
            this.btnQAFolderBrowse.Text = "Browse...";
            this.btnQAFolderBrowse.UseVisualStyleBackColor = true;
            this.btnQAFolderBrowse.Click += new System.EventHandler(this.Browse);
            // 
            // btnServiceBrowse
            // 
            this.btnServiceBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnServiceBrowse.Location = new System.Drawing.Point(326, 247);
            this.btnServiceBrowse.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnServiceBrowse.Name = "btnServiceBrowse";
            this.btnServiceBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnServiceBrowse.TabIndex = 6;
            this.btnServiceBrowse.Text = "Browse...";
            this.btnServiceBrowse.UseVisualStyleBackColor = true;
            this.btnServiceBrowse.Click += new System.EventHandler(this.Browse);
            // 
            // btnCoreBrowse
            // 
            this.btnCoreBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCoreBrowse.Location = new System.Drawing.Point(326, 170);
            this.btnCoreBrowse.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCoreBrowse.Name = "btnCoreBrowse";
            this.btnCoreBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCoreBrowse.TabIndex = 4;
            this.btnCoreBrowse.Text = "Browse...";
            this.btnCoreBrowse.UseVisualStyleBackColor = true;
            this.btnCoreBrowse.Click += new System.EventHandler(this.Browse);
            // 
            // btnECheckBrowse
            // 
            this.btnECheckBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnECheckBrowse.Location = new System.Drawing.Point(326, 93);
            this.btnECheckBrowse.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnECheckBrowse.Name = "btnECheckBrowse";
            this.btnECheckBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnECheckBrowse.TabIndex = 2;
            this.btnECheckBrowse.Text = "Browse...";
            this.btnECheckBrowse.UseVisualStyleBackColor = true;
            this.btnECheckBrowse.Click += new System.EventHandler(this.Browse);
            // 
            // btnPublish
            // 
            this.btnPublish.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPublish.Location = new System.Drawing.Point(150, 1);
            this.btnPublish.Margin = new System.Windows.Forms.Padding(0);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(100, 37);
            this.btnPublish.TabIndex = 10;
            this.btnPublish.Text = "Publish";
            this.btnPublish.UseVisualStyleBackColor = false;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // lblTaskName
            // 
            this.lblTaskName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTaskName.AutoSize = true;
            this.lblTaskName.Location = new System.Drawing.Point(169, 348);
            this.lblTaskName.Margin = new System.Windows.Forms.Padding(0);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(65, 13);
            this.lblTaskName.TabIndex = 0;
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
            this.tbECheckPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbECheckPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbECheckPath.Location = new System.Drawing.Point(3, 55);
            this.tbECheckPath.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tbECheckPath.Multiline = true;
            this.tbECheckPath.Name = "tbECheckPath";
            this.tbECheckPath.Size = new System.Drawing.Size(398, 37);
            this.tbECheckPath.TabIndex = 1;
            this.tbECheckPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbECheckPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbECheckPath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbCorePath
            // 
            this.tbCorePath.AllowDrop = true;
            this.tbCorePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCorePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCorePath.Location = new System.Drawing.Point(3, 132);
            this.tbCorePath.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tbCorePath.Multiline = true;
            this.tbCorePath.Name = "tbCorePath";
            this.tbCorePath.Size = new System.Drawing.Size(398, 37);
            this.tbCorePath.TabIndex = 3;
            this.tbCorePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbCorePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbCorePath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbServicePath
            // 
            this.tbServicePath.AllowDrop = true;
            this.tbServicePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbServicePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbServicePath.Location = new System.Drawing.Point(3, 209);
            this.tbServicePath.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tbServicePath.Multiline = true;
            this.tbServicePath.Name = "tbServicePath";
            this.tbServicePath.Size = new System.Drawing.Size(398, 37);
            this.tbServicePath.TabIndex = 5;
            this.tbServicePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbServicePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbServicePath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbQAFolderPath
            // 
            this.tbQAFolderPath.AllowDrop = true;
            this.tbQAFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbQAFolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbQAFolderPath.Location = new System.Drawing.Point(3, 286);
            this.tbQAFolderPath.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tbQAFolderPath.Multiline = true;
            this.tbQAFolderPath.Name = "tbQAFolderPath";
            this.tbQAFolderPath.Size = new System.Drawing.Size(398, 37);
            this.tbQAFolderPath.TabIndex = 7;
            this.tbQAFolderPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbQAFolderPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbQAFolderPath.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // tbTaskName
            // 
            this.tbTaskName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTaskName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTaskName.Location = new System.Drawing.Point(3, 363);
            this.tbTaskName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tbTaskName.Name = "tbTaskName";
            this.tbTaskName.Size = new System.Drawing.Size(398, 20);
            this.tbTaskName.TabIndex = 9;
            this.tbTaskName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            this.tbTaskName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPress);
            this.tbTaskName.Leave += new System.EventHandler(this.tb_Leave);
            // 
            // btnLocate
            // 
            this.btnLocate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLocate.Location = new System.Drawing.Point(320, 10);
            this.btnLocate.Margin = new System.Windows.Forms.Padding(0);
            this.btnLocate.Name = "btnLocate";
            this.btnLocate.Size = new System.Drawing.Size(75, 23);
            this.btnLocate.TabIndex = 11;
            this.btnLocate.Text = "Locate";
            this.btnLocate.UseVisualStyleBackColor = true;
            // 
            // pbCopyToClipboard
            // 
            this.pbCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCopyToClipboard.Image = ((System.Drawing.Image)(resources.GetObject("pbCopyToClipboard.Image")));
            this.pbCopyToClipboard.Location = new System.Drawing.Point(260, 3);
            this.pbCopyToClipboard.Margin = new System.Windows.Forms.Padding(0);
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
            this.pbLoad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbLoad.Image = global::PublishForQA.Properties.Resources.Load;
            this.pbLoad.Location = new System.Drawing.Point(276, 9);
            this.pbLoad.Margin = new System.Windows.Forms.Padding(0);
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
            this.pbSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSave.Image = global::PublishForQA.Properties.Resources.Save;
            this.pbSave.Location = new System.Drawing.Point(243, 9);
            this.pbSave.Margin = new System.Windows.Forms.Padding(0);
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
            this.pbHelp.Location = new System.Drawing.Point(0, 0);
            this.pbHelp.Margin = new System.Windows.Forms.Padding(0);
            this.pbHelp.Name = "pbHelp";
            this.pbHelp.Size = new System.Drawing.Size(18, 18);
            this.pbHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbHelp.TabIndex = 14;
            this.pbHelp.TabStop = false;
            this.pbHelp.Click += new System.EventHandler(this.pbHelp_Click);
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.pTop, 0, 0);
            this.tlpMain.Controls.Add(this.lblECheck, 0, 1);
            this.tlpMain.Controls.Add(this.tbTaskName, 0, 14);
            this.tlpMain.Controls.Add(this.tbQAFolderPath, 0, 11);
            this.tlpMain.Controls.Add(this.lblTaskName, 0, 13);
            this.tlpMain.Controls.Add(this.tbServicePath, 0, 8);
            this.tlpMain.Controls.Add(this.tbCorePath, 0, 5);
            this.tlpMain.Controls.Add(this.btnQAFolderBrowse, 0, 12);
            this.tlpMain.Controls.Add(this.tbECheckPath, 0, 2);
            this.tlpMain.Controls.Add(this.btnECheckBrowse, 0, 3);
            this.tlpMain.Controls.Add(this.btnServiceBrowse, 0, 9);
            this.tlpMain.Controls.Add(this.lblQAFolder, 0, 10);
            this.tlpMain.Controls.Add(this.lblCore, 0, 4);
            this.tlpMain.Controls.Add(this.btnCoreBrowse, 0, 6);
            this.tlpMain.Controls.Add(this.lblService, 0, 7);
            this.tlpMain.Controls.Add(this.pBottom, 0, 15);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 16;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.Size = new System.Drawing.Size(404, 433);
            this.tlpMain.TabIndex = 0;
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.pbHelp);
            this.pTop.Controls.Add(this.pbLoadDropdown);
            this.pTop.Controls.Add(this.pbSave);
            this.pTop.Controls.Add(this.pbLoad);
            this.pTop.Controls.Add(this.btnLocate);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Margin = new System.Windows.Forms.Padding(0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(404, 40);
            this.pTop.TabIndex = 0;
            // 
            // pbLoadDropdown
            // 
            this.pbLoadDropdown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pbLoadDropdown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbLoadDropdown.Image = global::PublishForQA.Properties.Resources.ArrowDown;
            this.pbLoadDropdown.Location = new System.Drawing.Point(300, 9);
            this.pbLoadDropdown.Margin = new System.Windows.Forms.Padding(0);
            this.pbLoadDropdown.Name = "pbLoadDropdown";
            this.pbLoadDropdown.Size = new System.Drawing.Size(13, 25);
            this.pbLoadDropdown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLoadDropdown.TabIndex = 15;
            this.pbLoadDropdown.TabStop = false;
            this.pbLoadDropdown.Click += new System.EventHandler(this.pbLoadDropdown_Click);
            // 
            // pBottom
            // 
            this.pBottom.Controls.Add(this.btnPublish);
            this.pBottom.Controls.Add(this.pbCopyToClipboard);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBottom.Location = new System.Drawing.Point(3, 386);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(398, 44);
            this.pBottom.TabIndex = 14;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Text files|*.txt";
            // 
            // FormPublisher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 433);
            this.Controls.Add(this.tlpMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPublisher";
            this.Text = "Publisher Alpha v1.0";
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyToClipboard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHelp)).EndInit();
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.pTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoadDropdown)).EndInit();
            this.pBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
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
        public TextBoxFormPublisher tbTaskName;
        public TextBoxFormPublisher tbQAFolderPath;
        public TextBoxFormPublisher tbServicePath;
        public TextBoxFormPublisher tbCorePath;
        public TextBoxFormPublisher tbECheckPath;
        private System.Windows.Forms.PictureBox pbCopyToClipboard;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pBottom;
        private System.Windows.Forms.PictureBox pbLoadDropdown;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

