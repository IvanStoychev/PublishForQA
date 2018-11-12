﻿namespace PublishForQA
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pbExpandCollapse = new System.Windows.Forms.PictureBox();
            this.pbError = new System.Windows.Forms.PictureBox();
            this.panelValidationDetails = new System.Windows.Forms.Panel();
            this.lblValidationDetails = new System.Windows.Forms.Label();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExpandCollapse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).BeginInit();
            this.panelValidationDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblValidationName
            // 
            this.lblValidationName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValidationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValidationName.Location = new System.Drawing.Point(33, 0);
            this.lblValidationName.Name = "lblValidationName";
            this.lblValidationName.Size = new System.Drawing.Size(348, 30);
            this.lblValidationName.TabIndex = 0;
            this.lblValidationName.Text = "lblValidationName";
            this.lblValidationName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpMain.Controls.Add(this.pbExpandCollapse, 2, 0);
            this.tlpMain.Controls.Add(this.lblValidationName, 1, 0);
            this.tlpMain.Controls.Add(this.pbError, 0, 0);
            this.tlpMain.Controls.Add(this.panelValidationDetails, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(414, 136);
            this.tlpMain.TabIndex = 0;
            // 
            // pbExpandCollapse
            // 
            this.pbExpandCollapse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbExpandCollapse.Image = global::PublishForQA.Properties.Resources.ArrowDown;
            this.pbExpandCollapse.Location = new System.Drawing.Point(384, 0);
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
            // panelValidationDetails
            // 
            this.tlpMain.SetColumnSpan(this.panelValidationDetails, 3);
            this.panelValidationDetails.Controls.Add(this.lblValidationDetails);
            this.panelValidationDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelValidationDetails.Location = new System.Drawing.Point(3, 33);
            this.panelValidationDetails.Name = "panelValidationDetails";
            this.panelValidationDetails.Size = new System.Drawing.Size(408, 100);
            this.panelValidationDetails.TabIndex = 3;
            // 
            // lblValidationDetails
            // 
            this.lblValidationDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValidationDetails.Location = new System.Drawing.Point(0, 0);
            this.lblValidationDetails.Name = "lblValidationDetails";
            this.lblValidationDetails.Size = new System.Drawing.Size(408, 100);
            this.lblValidationDetails.TabIndex = 0;
            this.lblValidationDetails.Text = "lblValidationDetails";
            // 
            // ValidationCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.tlpMain);
            this.Name = "ValidationCheck";
            this.Size = new System.Drawing.Size(414, 139);
            this.tlpMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbExpandCollapse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).EndInit();
            this.panelValidationDetails.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblValidationName;
        private System.Windows.Forms.PictureBox pbError;
        private System.Windows.Forms.PictureBox pbExpandCollapse;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Label lblValidationDetails;
        private System.Windows.Forms.Panel panelValidationDetails;
    }
}
