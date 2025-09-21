using Godot;
using System;
using static game.Nodes;
public partial class PlayButton : Button
{
    public override void _Ready()
    {
        
    }
    private void Play()
    {
        EnableNode(PlayerNode);
        EnableNode(map1);
        DisableNode(GetNode<SettingsButton>("../SettingsButton"));
        GetNode<Sprite2D>("../ActionsBar").Show();
    }
}
