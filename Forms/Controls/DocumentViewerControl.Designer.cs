namespace MarkoutBackupViewer.Forms.Controls
{
    partial class DocumentViewerControl
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
            this.viewerPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // viewerPanel
            // 
            this.viewerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewerPanel.Location = new System.Drawing.Point(0, 0);
            this.viewerPanel.Name = "viewerPanel";
            this.viewerPanel.Size = new System.Drawing.Size(707, 531);
            this.viewerPanel.TabIndex = 0;
            // 
            // DocumentViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.viewerPanel);
            this.Name = "DocumentViewerControl";
            this.Size = new System.Drawing.Size(707, 531);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel viewerPanel;

    }
}
