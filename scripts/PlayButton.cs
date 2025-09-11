using Godot;
using System;
using static game.Nodes;
public partial class PlayButton : Button
{
    public override void _Ready()
    {
        Pressed += Play;
    }
    private void Play()
    {
        EnableNode(PlayerNode);
        EnableNode(map1);
        GetNode<SettingsButton>("../SettingsButton").QueueFree();
        QueueFree();
    }
}
