namespace winforms_fluent_ui_gallery
{
    partial class TestForm
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
            this.indicatedButton1 = new WinForms.Fluent.UI.IndicatedButton();
            this.SuspendLayout();
            // 
            // indicatedButton1
            // 
            this.indicatedButton1.Location = new System.Drawing.Point(293, 219);
            this.indicatedButton1.Name = "indicatedButton1";
            this.indicatedButton1.Size = new System.Drawing.Size(326, 51);
            this.indicatedButton1.TabIndex = 0;
            this.indicatedButton1.Text = "indicatedButton1";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.indicatedButton1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private WinForms.Fluent.UI.IndicatedButton indicatedButton1;
    }
}