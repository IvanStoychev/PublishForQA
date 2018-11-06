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
            this.pbError = new System.Windows.Forms.PictureBox();
            this.pbExpandCollapse = new System.Windows.Forms.PictureBox();
            this.tlpTopHalf = new System.Windows.Forms.TableLayoutPanel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExpandCollapse)).BeginInit();
            this.tlpTopHalf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblValidationName
            // 
            this.lblValidationName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValidationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValidationName.Location = new System.Drawing.Point(33, 0);
            this.lblValidationName.Name = "lblValidationName";
            this.lblValidationName.Size = new System.Drawing.Size(354, 30);
            this.lblValidationName.TabIndex = 0;
            this.lblValidationName.Text = "lblValidationName";
            this.lblValidationName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbError
            // 
            this.pbError.Image = global::PublishForQA.Properties.Resources.Error;
            this.pbError.Location = new System.Drawing.Point(3, 3);
            this.pbError.Name = "pbError";
            this.pbError.Size = new System.Drawing.Size(24, 24);
            this.pbError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbError.TabIndex = 2;
            this.pbError.TabStop = false;
            // 
            // pbExpandCollapse
            // 
            this.pbExpandCollapse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbExpandCollapse.Image = global::PublishForQA.Properties.Resources.Help;
            this.pbExpandCollapse.Location = new System.Drawing.Point(393, 3);
            this.pbExpandCollapse.Name = "pbExpandCollapse";
            this.pbExpandCollapse.Size = new System.Drawing.Size(25, 24);
            this.pbExpandCollapse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbExpandCollapse.TabIndex = 1;
            this.pbExpandCollapse.TabStop = false;
            // 
            // tlpTopHalf
            // 
            this.tlpTopHalf.ColumnCount = 3;
            this.tlpTopHalf.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpTopHalf.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 360F));
            this.tlpTopHalf.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpTopHalf.Controls.Add(this.pbExpandCollapse, 2, 0);
            this.tlpTopHalf.Controls.Add(this.lblValidationName, 1, 0);
            this.tlpTopHalf.Controls.Add(this.pbError, 0, 0);
            this.tlpTopHalf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTopHalf.Location = new System.Drawing.Point(0, 0);
            this.tlpTopHalf.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTopHalf.Name = "tlpTopHalf";
            this.tlpTopHalf.RowCount = 1;
            this.tlpTopHalf.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpTopHalf.Size = new System.Drawing.Size(416, 30);
            this.tlpTopHalf.TabIndex = 0;
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
            this.splitMain.Size = new System.Drawing.Size(416, 210);
            this.splitMain.SplitterDistance = 30;
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
            this.Size = new System.Drawing.Size(416, 210);
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExpandCollapse)).EndInit();
            this.tlpTopHalf.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblValidationName;
        private System.Windows.Forms.PictureBox pbError;
        private System.Windows.Forms.PictureBox pbExpandCollapse;
        private System.Windows.Forms.TableLayoutPanel tlpTopHalf;
        private System.Windows.Forms.SplitContainer splitMain;
    }
}
