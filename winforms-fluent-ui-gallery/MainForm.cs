using WinForms.Fluent.UI;

namespace winforms_fluent_ui_gallery;

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
        if(progressRingIsDeterminateCheck.Checked)
            return;

        progressRing1.Value = (float) progressRingValueNud.Value;
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