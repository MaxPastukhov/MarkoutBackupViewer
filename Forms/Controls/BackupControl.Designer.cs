namespace MarkoutBackupViewer.Forms.Controls
{
    partial class BackupControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.documentTreeControl = new MarkoutBackupViewer.Forms.Controls.DocumentTreeControl();
            this.documentViewerControl = new MarkoutBackupViewer.Forms.Controls.DocumentViewerControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.documentTreeControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.documentViewerControl);
            this.splitContainer1.Size = new System.Drawing.Size(676, 351);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 0;
            // 
            // documentTreeControl
            // 
            this.documentTreeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentTreeControl.Location = new System.Drawing.Point(0, 0);
            this.documentTreeControl.Name = "documentTreeControl";
            this.documentTreeControl.Size = new System.Drawing.Size(225, 351);
            this.documentTreeControl.TabIndex = 0;
            // 
            // documentViewerControl
            // 
            this.documentViewerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentViewerControl.Location = new System.Drawing.Point(0, 0);
            this.documentViewerControl.Name = "documentViewerControl";
            this.documentViewerControl.Size = new System.Drawing.Size(447, 351);
            this.documentViewerControl.TabIndex = 0;
            // 
            // BackupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "BackupControl";
            this.Size = new System.Drawing.Size(676, 351);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DocumentTreeControl documentTreeControl;
        private DocumentViewerControl documentViewerControl;
    }
}
