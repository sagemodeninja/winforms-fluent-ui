using System;
using System.Windows.Forms;
using WinForms.Fluent.UI;
using WinForms.Fluent.UI.Utilities.Enums;

namespace winforms_fluent_ui_gallery
{
    public partial class MainForm : FluentForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ProgressRingIsDeterminateCheck_CheckedChange(object sender, EventArgs e)
        {
            progressRing1.IsIndeterminate = progressRingIsDeterminateCheck.Checked;
            progressRingValueNud.Enabled = !progressRingIsDeterminateCheck.Checked;
        }

        private void progressRingValueNud_ValueChanged(object sender, EventArgs e)
        {
            if (progressRingIsDeterminateCheck.Checked)
                return;

            progressRing1.Value = (float)progressRingValueNud.Value;
        }

        private void profileImageRadio_CheckedChanged(object sender, EventArgs e)
        {
            for (var idx = 0; idx < 3; idx++)
            {
                var control = (RadioButton)profileTypeGroup.Controls[idx];
                if (control.Checked)
                {
                    personPicture1.ProfileType = (ProfileType)idx;
                    break;
                }
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void testFormBtn_Click(object sender, EventArgs e)
        {
            var form = new TestForm();
            form.Show();
        }
    }
}