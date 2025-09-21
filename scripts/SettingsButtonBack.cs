using Godot;
using static game.Nodes;
public partial class SettingsButtonBack : Button
{
    private SettingsButton settingsbutton;
    public override void _Ready()
    {
        Pressed += CloseSettings;
        settingsbutton = GetNode<SettingsButton>("../SettingsButton");
    }
    private void CloseSettings()
    {
        DisableNode(settingsnode);
        EnableNode(settingsbutton);
        Hide();
        ProcessMode = ProcessModeEnum.Disabled;
    }
}
