namespace winforms_fluent_ui_gallery
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressRing1 = new WinForms.Fluent.UI.ProgressRing();
            this.progressRing2 = new WinForms.Fluent.UI.ProgressRing();
            this.SuspendLayout();
            // 
            // progressRing1
            // 
            this.progressRing1.EllipseWidth = 7F;
            this.progressRing1.IsIndeterminate = true;
            this.progressRing1.Location = new System.Drawing.Point(203, 78);
            this.progressRing1.MaxValue = 100F;
            this.progressRing1.Name = "progressRing1";
            this.progressRing1.Size = new System.Drawing.Size(75, 75);
            this.progressRing1.TabIndex = 0;
            this.progressRing1.Text = "progressRing1";
            this.progressRing1.Value = 0F;
            // 
            // progressRing2
            // 
            this.progressRing2.EllipseWidth = 7F;
            this.progressRing2.IsIndeterminate = true;
            this.progressRing2.Location = new System.Drawing.Point(321, 78);
            this.progressRing2.MaxValue = 100F;
            this.progressRing2.Name = "progressRing2";
            this.progressRing2.Size = new System.Drawing.Size(75, 75);
            this.progressRing2.TabIndex = 1;
            this.progressRing2.Text = "progressRing2";
            this.progressRing2.Value = 0F;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.progressRing2);
            this.Controls.Add(this.progressRing1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private WinForms.Fluent.UI.ProgressRing progressRing1;
        private WinForms.Fluent.UI.ProgressRing progressRing2;
    }
}