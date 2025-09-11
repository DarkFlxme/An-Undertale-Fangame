using Godot;
using static game.Nodes;
public partial class SettingsButtonBack : Button
{
    private SettingsButton settingsbutton;
    private PlayButton playbutton;
    public override void _Ready()
    {
        Pressed += CloseSettings;
        settingsbutton = GetNode<SettingsButton>("../SettingsButton");
        playbutton = GetNode<PlayButton>("../PlayButton");
    }
    private void CloseSettings()
    {
        DisableNode(settingsnode);
        EnableNode(settingsbutton);
        EnableNode(playbutton);
        Hide();
        ProcessMode = ProcessModeEnum.Disabled;
    }
}
