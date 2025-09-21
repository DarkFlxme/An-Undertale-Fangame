using Godot;
using static game.Nodes;
public partial class SettingsButton : Button
{
    private SettingsButtonBack settingsbuttonback;
    public override void _Ready()
    {
        Pressed += OpenSettings;
    }
    private void OpenSettings()
    {
        EnableNode(settingsnode);
        EnableNode(settingsbuttonback);
        Hide();
        ProcessMode = ProcessModeEnum.Disabled;
    }
}
