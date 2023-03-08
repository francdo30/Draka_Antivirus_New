namespace ScanAutomatique
{
    partial class ScanTotal
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
            this.backgroundWorkerScanTotal = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // backgroundWorkerScanTotal
            // 
            this.backgroundWorkerScanTotal.WorkerReportsProgress = true;
            this.backgroundWorkerScanTotal.WorkerSupportsCancellation = true;
            this.backgroundWorkerScanTotal.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Dowork);
            this.backgroundWorkerScanTotal.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ProgressChanged);
            this.backgroundWorkerScanTotal.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RunWorkerComplete);
            // 
            // ScanTotal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 136);
            this.Name = "ScanTotal";
            this.Opacity = 0D;
            this.Text = "ScanTotal";
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorkerScanTotal;
    }
}