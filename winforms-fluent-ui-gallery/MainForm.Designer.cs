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
            this.label1 = new System.Windows.Forms.Label();
            this.progressRingIsDeterminateCheck = new System.Windows.Forms.CheckBox();
            this.progressRingValueNud = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.progressRingValueNud)).BeginInit();
            this.SuspendLayout();
            // 
            // progressRing1
            // 
            this.progressRing1.EllipseWidth = 8F;
            this.progressRing1.IsIndeterminate = false;
            this.progressRing1.Location = new System.Drawing.Point(56, 50);
            this.progressRing1.MaxValue = 100F;
            this.progressRing1.Name = "progressRing1";
            this.progressRing1.Size = new System.Drawing.Size(75, 75);
            this.progressRing1.TabIndex = 3;
            this.progressRing1.Text = "progressRing1";
            this.progressRing1.Value = 50F;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Progress Ring";
            // 
            // progressRingIsDeterminateCheck
            // 
            this.progressRingIsDeterminateCheck.AutoSize = true;
            this.progressRingIsDeterminateCheck.Location = new System.Drawing.Point(137, 50);
            this.progressRingIsDeterminateCheck.Name = "progressRingIsDeterminateCheck";
            this.progressRingIsDeterminateCheck.Size = new System.Drawing.Size(102, 19);
            this.progressRingIsDeterminateCheck.TabIndex = 5;
            this.progressRingIsDeterminateCheck.Text = "Is Determinate";
            this.progressRingIsDeterminateCheck.UseVisualStyleBackColor = true;
            this.progressRingIsDeterminateCheck.CheckedChanged += new System.EventHandler(this.progressRingIsDeterminateCheck_CheckedChanged);
            // 
            // progressRingValueNud
            // 
            this.progressRingValueNud.Location = new System.Drawing.Point(138, 102);
            this.progressRingValueNud.Name = "progressRingValueNud";
            this.progressRingValueNud.Size = new System.Drawing.Size(67, 23);
            this.progressRingValueNud.TabIndex = 6;
            this.progressRingValueNud.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.progressRingValueNud.ValueChanged += new System.EventHandler(this.progressRingValueNud_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Value:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressRingValueNud);
            this.Controls.Add(this.progressRingIsDeterminateCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressRing1);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.progressRingValueNud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private WinForms.Fluent.UI.ProgressRing progressRing1;
        private Label label1;
        private CheckBox progressRingIsDeterminateCheck;
        private NumericUpDown progressRingValueNud;
        private Label label2;
    }
}