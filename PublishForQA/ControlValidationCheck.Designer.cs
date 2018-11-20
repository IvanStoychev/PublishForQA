namespace PublishForQA
{
    partial class ControlValidationCheck
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblValidationName = new System.Windows.Forms.Label();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pbExpandCollapse = new System.Windows.Forms.PictureBox();
            this.pbError = new System.Windows.Forms.PictureBox();
            this.lblValidationDetails = new System.Windows.Forms.Label();
            this.tlpDetails = new System.Windows.Forms.TableLayoutPanel();
            this.tlpTitle = new System.Windows.Forms.TableLayoutPanel();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExpandCollapse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).BeginInit();
            this.tlpDetails.SuspendLayout();
            this.tlpTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblValidationName
            // 
            this.lblValidationName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValidationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValidationName.Location = new System.Drawing.Point(33, 0);
            this.lblValidationName.Name = "lblValidationName";
            this.lblValidationName.Size = new System.Drawing.Size(352, 30);
            this.lblValidationName.TabIndex = 0;
            this.lblValidationName.Text = "lblValidationName";
            this.lblValidationName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Controls.Add(this.tlpDetails, 0, 1);
            this.tlpMain.Controls.Add(this.tlpTitle, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(418, 143);
            this.tlpMain.TabIndex = 0;
            // 
            // pbExpandCollapse
            // 
            this.pbExpandCollapse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbExpandCollapse.Image = global::PublishForQA.Properties.Resources.ArrowUp;
            this.pbExpandCollapse.Location = new System.Drawing.Point(388, 0);
            this.pbExpandCollapse.Margin = new System.Windows.Forms.Padding(0);
            this.pbExpandCollapse.Name = "pbExpandCollapse";
            this.pbExpandCollapse.Size = new System.Drawing.Size(30, 30);
            this.pbExpandCollapse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbExpandCollapse.TabIndex = 1;
            this.pbExpandCollapse.TabStop = false;
            this.pbExpandCollapse.Click += new System.EventHandler(this.pbExpandCollapse_Click);
            // 
            // pbError
            // 
            this.pbError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbError.Image = global::PublishForQA.Properties.Resources.Error;
            this.pbError.Location = new System.Drawing.Point(0, 0);
            this.pbError.Margin = new System.Windows.Forms.Padding(0);
            this.pbError.MinimumSize = new System.Drawing.Size(24, 24);
            this.pbError.Name = "pbError";
            this.pbError.Size = new System.Drawing.Size(30, 30);
            this.pbError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbError.TabIndex = 2;
            this.pbError.TabStop = false;
            // 
            // lblValidationDetails
            // 
            this.lblValidationDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValidationDetails.Location = new System.Drawing.Point(3, 5);
            this.lblValidationDetails.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.lblValidationDetails.Name = "lblValidationDetails";
            this.lblValidationDetails.Size = new System.Drawing.Size(412, 103);
            this.lblValidationDetails.TabIndex = 0;
            this.lblValidationDetails.Text = "lblValidationDetails";
            // 
            // tlpDetails
            // 
            this.tlpDetails.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpDetails.ColumnCount = 1;
            this.tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpDetails.Controls.Add(this.lblValidationDetails, 0, 0);
            this.tlpDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDetails.Location = new System.Drawing.Point(0, 30);
            this.tlpDetails.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDetails.Name = "tlpDetails";
            this.tlpDetails.RowCount = 1;
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpDetails.Size = new System.Drawing.Size(418, 113);
            this.tlpDetails.TabIndex = 1;
            // 
            // tlpTitle
            // 
            this.tlpTitle.ColumnCount = 3;
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpTitle.Controls.Add(this.pbExpandCollapse, 2, 0);
            this.tlpTitle.Controls.Add(this.lblValidationName, 1, 0);
            this.tlpTitle.Controls.Add(this.pbError, 0, 0);
            this.tlpTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTitle.Location = new System.Drawing.Point(0, 0);
            this.tlpTitle.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTitle.Name = "tlpTitle";
            this.tlpTitle.RowCount = 1;
            this.tlpTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpTitle.Size = new System.Drawing.Size(418, 30);
            this.tlpTitle.TabIndex = 3;
            // 
            // ControlValidationCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "ControlValidationCheck";
            this.Size = new System.Drawing.Size(418, 143);
            this.tlpMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbExpandCollapse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).EndInit();
            this.tlpDetails.ResumeLayout(false);
            this.tlpTitle.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblValidationName;
        private System.Windows.Forms.PictureBox pbError;
        private System.Windows.Forms.PictureBox pbExpandCollapse;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Label lblValidationDetails;
        private System.Windows.Forms.TableLayoutPanel tlpDetails;
        private System.Windows.Forms.TableLayoutPanel tlpTitle;
    }
}
