using Godot;
using System;

public partial class GameUI : Control
{
    private Label Label;
    private Sprite2D samoystaBoss;
    public override void _Ready()
    {
        Label = GetNode<Label>("Label2");
        samoystaBoss = GetNode<Sprite2D>("Samoysta");
    }
    public override void _Process(double delta)
    {
        game.Settings.BossFightTime += delta;
        TimeSpan timeSpan = TimeSpan.FromSeconds(game.Settings.BossFightTime);
        Label.Text = string.Format("ZINA  {0:D2}:{1:D2}  HP         {2}/{3}", timeSpan.Minutes, timeSpan.Seconds, game.Nodes.PlayerNode.health, game.Nodes.PlayerNode.maxHealth);
    }
    private void SamoystaChangeTexture(bool texture) // 1 = normal, 0 = dehşet
    {
        if(texture)
        {
            samoystaBoss.Texture = GD.Load<Texture2D>("res://assets/samiboss1.png");
        }
        else
        {
            samoystaBoss.Texture = GD.Load<Texture2D>("res://assets/samiboss0.png");
        }
    }
}
