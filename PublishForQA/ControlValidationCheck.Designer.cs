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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.splitterTop = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblValidationName
            // 
            this.lblValidationName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValidationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValidationName.Location = new System.Drawing.Point(77, 0);
            this.lblValidationName.Name = "lblValidationName";
            this.lblValidationName.Size = new System.Drawing.Size(434, 74);
            this.lblValidationName.TabIndex = 1;
            this.lblValidationName.Text = "lblValidationName";
            this.lblValidationName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lblValidationName);
            this.splitContainer.Panel1.Controls.Add(this.splitterTop);
            this.splitContainer.Size = new System.Drawing.Size(511, 303);
            this.splitContainer.SplitterDistance = 74;
            this.splitContainer.TabIndex = 2;
            // 
            // splitterTop
            // 
            this.splitterTop.Location = new System.Drawing.Point(0, 0);
            this.splitterTop.Name = "splitterTop";
            this.splitterTop.Size = new System.Drawing.Size(77, 74);
            this.splitterTop.TabIndex = 0;
            this.splitterTop.TabStop = false;
            // 
            // ControlValidationCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitContainer);
            this.Name = "ControlValidationCheck";
            this.Size = new System.Drawing.Size(511, 303);
            this.splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblValidationName;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Splitter splitterTop;
    }
}
