using Godot;
using static game.Nodes;
public partial class SettingsButton : Button
{
    private SettingsButtonBack settingsbuttonback;
    private PlayButton playbutton;
    public override void _Ready()
    {
        Pressed += OpenSettings;
        settingsbuttonback = GetNode<SettingsButtonBack>("../SettingsButtonBack");
        playbutton = GetNode<PlayButton>("../PlayButton");
    }
    private void OpenSettings()
    {
        EnableNode(settingsnode);
        EnableNode(settingsbuttonback);
        DisableNode(playbutton);
        Hide();
        ProcessMode = ProcessModeEnum.Disabled;
    }
}
