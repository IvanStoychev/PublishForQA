namespace PublishForQA
{
    partial class ValidationCheck
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
            this.tlpTopHalf = new System.Windows.Forms.TableLayoutPanel();
            this.pbExpandCollapse = new System.Windows.Forms.PictureBox();
            this.pbError = new System.Windows.Forms.PictureBox();
            this.lblValidationDetails = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tlpTopHalf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExpandCollapse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblValidationName
            // 
            this.lblValidationName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValidationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValidationName.Location = new System.Drawing.Point(27, 0);
            this.lblValidationName.Name = "lblValidationName";
            this.lblValidationName.Size = new System.Drawing.Size(361, 30);
            this.lblValidationName.TabIndex = 0;
            this.lblValidationName.Text = "lblValidationName";
            this.lblValidationName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpTopHalf
            // 
            this.tlpTopHalf.AutoSize = true;
            this.tlpTopHalf.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpTopHalf.ColumnCount = 3;
            this.tlpTopHalf.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpTopHalf.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTopHalf.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpTopHalf.Controls.Add(this.pbExpandCollapse, 2, 0);
            this.tlpTopHalf.Controls.Add(this.lblValidationName, 1, 0);
            this.tlpTopHalf.Controls.Add(this.pbError, 0, 0);
            this.tlpTopHalf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTopHalf.Location = new System.Drawing.Point(0, 0);
            this.tlpTopHalf.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTopHalf.Name = "tlpTopHalf";
            this.tlpTopHalf.RowCount = 1;
            this.tlpTopHalf.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpTopHalf.Size = new System.Drawing.Size(416, 25);
            this.tlpTopHalf.TabIndex = 0;
            // 
            // pbExpandCollapse
            // 
            this.pbExpandCollapse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbExpandCollapse.Image = global::PublishForQA.Properties.Resources.ArrowDown;
            this.pbExpandCollapse.Location = new System.Drawing.Point(394, 3);
            this.pbExpandCollapse.MaximumSize = new System.Drawing.Size(25, 24);
            this.pbExpandCollapse.MinimumSize = new System.Drawing.Size(25, 24);
            this.pbExpandCollapse.Name = "pbExpandCollapse";
            this.pbExpandCollapse.Size = new System.Drawing.Size(25, 24);
            this.pbExpandCollapse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbExpandCollapse.TabIndex = 1;
            this.pbExpandCollapse.TabStop = false;
            this.pbExpandCollapse.Click += new System.EventHandler(this.pbExpandCollapse_Click);
            // 
            // pbError
            // 
            this.pbError.Image = global::PublishForQA.Properties.Resources.Error;
            this.pbError.Location = new System.Drawing.Point(3, 3);
            this.pbError.MinimumSize = new System.Drawing.Size(24, 24);
            this.pbError.Name = "pbError";
            this.pbError.Size = new System.Drawing.Size(24, 24);
            this.pbError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbError.TabIndex = 2;
            this.pbError.TabStop = false;
            // 
            // lblValidationDetails
            // 
            this.lblValidationDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValidationDetails.Location = new System.Drawing.Point(7, 7);
            this.lblValidationDetails.Name = "lblValidationDetails";
            this.lblValidationDetails.Size = new System.Drawing.Size(402, 101);
            this.lblValidationDetails.TabIndex = 0;
            this.lblValidationDetails.Text = "lblValidationDetails";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.tlpTopHalf);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.lblValidationDetails);
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(7);
            this.splitMain.Size = new System.Drawing.Size(416, 141);
            this.splitMain.SplitterDistance = 25;
            this.splitMain.SplitterWidth = 1;
            this.splitMain.TabIndex = 0;
            this.splitMain.TabStop = false;
            // 
            // ValidationCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitMain);
            this.Name = "ValidationCheck";
            this.Size = new System.Drawing.Size(416, 141);
            this.tlpTopHalf.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbExpandCollapse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblValidationName;
        private System.Windows.Forms.PictureBox pbError;
        private System.Windows.Forms.PictureBox pbExpandCollapse;
        private System.Windows.Forms.TableLayoutPanel tlpTopHalf;
        private System.Windows.Forms.Label lblValidationDetails;
        private System.Windows.Forms.SplitContainer splitMain;
    }
}
