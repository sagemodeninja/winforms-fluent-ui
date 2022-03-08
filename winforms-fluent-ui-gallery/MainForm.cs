namespace winforms_fluent_ui_gallery;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void progressRingIsDeterminateCheck_CheckedChanged(object sender, EventArgs e)
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
}