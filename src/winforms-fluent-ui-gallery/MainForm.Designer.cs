using System.Windows.Forms;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.progressRing1 = new WinForms.Fluent.UI.ProgressRing();
            this.label1 = new System.Windows.Forms.Label();
            this.progressRingValueNud = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new WinForms.Fluent.UI.CheckBox();
            this.checkBox2 = new WinForms.Fluent.UI.CheckBox();
            this.progressRingIsDeterminateCheck = new WinForms.Fluent.UI.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.personPicture1 = new WinForms.Fluent.UI.PersonPicture();
            this.label4 = new System.Windows.Forms.Label();
            this.profileImageRadio = new System.Windows.Forms.RadioButton();
            this.displayNameRadio = new System.Windows.Forms.RadioButton();
            this.initialsRadio = new System.Windows.Forms.RadioButton();
            this.profileTypeGroup = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.personPicture2 = new WinForms.Fluent.UI.PersonPicture();
            ((System.ComponentModel.ISupportInitialize)(this.progressRingValueNud)).BeginInit();
            this.profileTypeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressRing1
            // 
            this.progressRing1.BackColor = System.Drawing.SystemColors.Control;
            this.progressRing1.EllipseWidth = 8F;
            this.progressRing1.IsIndeterminate = false;
            this.progressRing1.Location = new System.Drawing.Point(47, 111);
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
            this.label1.Location = new System.Drawing.Point(47, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Progress Ring";
            // 
            // progressRingValueNud
            // 
            this.progressRingValueNud.Location = new System.Drawing.Point(159, 163);
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
            this.label2.Location = new System.Drawing.Point(159, 145);
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
            this.checkBox1.Location = new System.Drawing.Point(341, 111);
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
            this.checkBox2.Location = new System.Drawing.Point(341, 145);
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
            this.progressRingIsDeterminateCheck.Location = new System.Drawing.Point(159, 111);
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
            this.label3.Location = new System.Drawing.Point(341, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Check Box";
            // 
            // personPicture1
            // 
            this.personPicture1.DisplayName = "Gary Antier";
            this.personPicture1.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.personPicture1.Initials = "SB";
            this.personPicture1.Location = new System.Drawing.Point(37, 266);
            this.personPicture1.Name = "personPicture1";
            this.personPicture1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.personPicture1.ProfilePicture = ((System.Drawing.Image)(resources.GetObject("personPicture1.ProfilePicture")));
            this.personPicture1.ProfileType = WinForms.Fluent.UI.Utilities.Enums.ProfileType.ProfileImage;
            this.personPicture1.Size = new System.Drawing.Size(91, 94);
            this.personPicture1.TabIndex = 13;
            this.personPicture1.Text = "personPicture1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "Person Picture";
            // 
            // profileImageRadio
            // 
            this.profileImageRadio.AutoSize = true;
            this.profileImageRadio.Checked = true;
            this.profileImageRadio.Location = new System.Drawing.Point(23, 36);
            this.profileImageRadio.Name = "profileImageRadio";
            this.profileImageRadio.Size = new System.Drawing.Size(95, 19);
            this.profileImageRadio.TabIndex = 16;
            this.profileImageRadio.TabStop = true;
            this.profileImageRadio.Text = "Profile Image";
            this.profileImageRadio.UseVisualStyleBackColor = true;
            this.profileImageRadio.CheckedChanged += new System.EventHandler(this.profileImageRadio_CheckedChanged);
            // 
            // displayNameRadio
            // 
            this.displayNameRadio.AutoSize = true;
            this.displayNameRadio.Location = new System.Drawing.Point(23, 61);
            this.displayNameRadio.Name = "displayNameRadio";
            this.displayNameRadio.Size = new System.Drawing.Size(98, 19);
            this.displayNameRadio.TabIndex = 17;
            this.displayNameRadio.Text = "Display Name";
            this.displayNameRadio.UseVisualStyleBackColor = true;
            this.displayNameRadio.CheckedChanged += new System.EventHandler(this.profileImageRadio_CheckedChanged);
            // 
            // initialsRadio
            // 
            this.initialsRadio.AutoSize = true;
            this.initialsRadio.Location = new System.Drawing.Point(23, 86);
            this.initialsRadio.Name = "initialsRadio";
            this.initialsRadio.Size = new System.Drawing.Size(59, 19);
            this.initialsRadio.TabIndex = 18;
            this.initialsRadio.Text = "Initials";
            this.initialsRadio.UseVisualStyleBackColor = true;
            this.initialsRadio.CheckedChanged += new System.EventHandler(this.profileImageRadio_CheckedChanged);
            // 
            // profileTypeGroup
            // 
            this.profileTypeGroup.Controls.Add(this.profileImageRadio);
            this.profileTypeGroup.Controls.Add(this.initialsRadio);
            this.profileTypeGroup.Controls.Add(this.displayNameRadio);
            this.profileTypeGroup.Location = new System.Drawing.Point(157, 243);
            this.profileTypeGroup.Name = "profileTypeGroup";
            this.profileTypeGroup.Size = new System.Drawing.Size(149, 129);
            this.profileTypeGroup.TabIndex = 19;
            this.profileTypeGroup.TabStop = false;
            this.profileTypeGroup.Text = "Profile Type";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(482, 276);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 24);
            this.button1.TabIndex = 20;
            this.button1.Text = "Open Test Form";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // personPicture2
            // 
            this.personPicture2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.personPicture2.DisplayName = "Gary Antier";
            this.personPicture2.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.personPicture2.Initials = "SB";
            this.personPicture2.Location = new System.Drawing.Point(576, 11);
            this.personPicture2.Name = "personPicture2";
            this.personPicture2.ProfilePicture = ((System.Drawing.Image)(resources.GetObject("personPicture2.ProfilePicture")));
            this.personPicture2.ProfileType = WinForms.Fluent.UI.Utilities.Enums.ProfileType.ProfileImage;
            this.personPicture2.Size = new System.Drawing.Size(28, 28);
            this.personPicture2.TabIndex = 21;
            this.personPicture2.Text = "personPicture2";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.CaptionHeight = 50;
            this.CaptionTextFont = new System.Drawing.Font("Segoe UI Variable Text", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ClientSize = new System.Drawing.Size(767, 411);
            this.Controls.Add(this.personPicture2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.profileTypeGroup);
            this.Controls.Add(this.personPicture1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressRingIsDeterminateCheck);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressRingValueNud);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressRing1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WinForms Fluent UI";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.progressRingValueNud)).EndInit();
            this.profileTypeGroup.ResumeLayout(false);
            this.profileTypeGroup.PerformLayout();
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
        private WinForms.Fluent.UI.PersonPicture personPicture1;
        private Label label4;
        private RadioButton profileImageRadio;
        private RadioButton displayNameRadio;
        private RadioButton initialsRadio;
        private GroupBox profileTypeGroup;
        private Button button1;
        private WinForms.Fluent.UI.PersonPicture personPicture2;
    }
}