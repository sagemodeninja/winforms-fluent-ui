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
            this.progressRingValueNud = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new WinForms.Fluent.UI.CheckBox();
            this.checkBox2 = new WinForms.Fluent.UI.CheckBox();
            this.progressRingIsDeterminateCheck = new WinForms.Fluent.UI.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.testFormBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.progressRingValueNud)).BeginInit();
            this.SuspendLayout();
            // 
            // progressRing1
            // 
            this.progressRing1.BackColor = System.Drawing.SystemColors.Control;
            this.progressRing1.EllipseWidth = 8F;
            this.progressRing1.IsIndeterminate = false;
            this.progressRing1.Location = new System.Drawing.Point(29, 82);
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
            this.label1.Location = new System.Drawing.Point(29, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Progress Ring";
            // 
            // progressRingValueNud
            // 
            this.progressRingValueNud.Location = new System.Drawing.Point(111, 134);
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
            this.label2.Location = new System.Drawing.Point(111, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Value:";
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = false;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.checkBox1.Font = new System.Drawing.Font("Segoe UI Variable Text", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBox1.Location = new System.Drawing.Point(339, 82);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(155, 24);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Two-state CheckBox";
            this.checkBox1.ThreeState = false;
            // 
            // checkBox2
            // 
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Font = new System.Drawing.Font("Segoe UI Variable Text", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBox2.Location = new System.Drawing.Point(339, 112);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(165, 24);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.Text = "Three-state CheckBox";
            this.checkBox2.ThreeState = true;
            // 
            // progressRingIsDeterminateCheck
            // 
            this.progressRingIsDeterminateCheck.Checked = false;
            this.progressRingIsDeterminateCheck.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.progressRingIsDeterminateCheck.Font = new System.Drawing.Font("Segoe UI Variable Text", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.progressRingIsDeterminateCheck.Location = new System.Drawing.Point(111, 82);
            this.progressRingIsDeterminateCheck.Name = "progressRingIsDeterminateCheck";
            this.progressRingIsDeterminateCheck.Size = new System.Drawing.Size(133, 24);
            this.progressRingIsDeterminateCheck.TabIndex = 10;
            this.progressRingIsDeterminateCheck.Text = "Is Indeterminate";
            this.progressRingIsDeterminateCheck.ThreeState = false;
            this.progressRingIsDeterminateCheck.CheckedChange += new System.EventHandler(this.ProgressRingIsDeterminateCheck_CheckedChange);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(339, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "CheckBox";
            // 
            // testFormBtn
            // 
            this.testFormBtn.Location = new System.Drawing.Point(209, 339);
            this.testFormBtn.Name = "testFormBtn";
            this.testFormBtn.Size = new System.Drawing.Size(112, 23);
            this.testFormBtn.TabIndex = 12;
            this.testFormBtn.Text = "Open Test Form";
            this.testFormBtn.UseVisualStyleBackColor = true;
            this.testFormBtn.Click += new System.EventHandler(this.testFormBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(806, 521);
            this.Controls.Add(this.testFormBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressRingIsDeterminateCheck);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressRingValueNud);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressRing1);
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.progressRingValueNud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private WinForms.Fluent.UI.ProgressRing progressRing1;
        private Label label1;
        private NumericUpDown progressRingValueNud;
        private Label label2;
        private WinForms.Fluent.UI.CheckBox checkBox1;
        private WinForms.Fluent.UI.CheckBox checkBox2;
        private WinForms.Fluent.UI.CheckBox progressRingIsDeterminateCheck;
        private Label label3;
        private Button testFormBtn;
    }
}