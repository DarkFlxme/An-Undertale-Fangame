using Godot;
using System;

public partial class GameUI : Control
{
    public float elapsedTime = 0.0f;
    private Label timeLabel;
    private Sprite2D samoystaBoss;
    public override void _Ready()
    {
        timeLabel = GetNode<Label>("Label2");
        samoystaBoss = GetNode<Sprite2D>("Samoysta");
    }
    public override void _Process(double delta)
    {
        elapsedTime += (float)delta;
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        timeLabel.Text = string.Format("ZINA  {0:D2}:{1:D2}  HP         {2}/100", timeSpan.Minutes, timeSpan.Seconds, game.Nodes.PlayerNode.health);
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
