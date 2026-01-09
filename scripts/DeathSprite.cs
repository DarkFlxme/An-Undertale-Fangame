using Godot;
using System;

public partial class DeathSprite : Node2D
{
    [Export] public Sprite2D deathSprite;
    [Export] public GpuParticles2D deathParticles;
    [Export] public AudioStreamPlayer deathSound;
    public override async void _Ready()
    {
        GlobalPosition = game.Nodes.PlayerNode.GlobalPosition;
        switch (game.Nodes.PlayerNode.HeartColor)
        {
            case Player.HeartColorEnum.Red:
                Modulate = new Color(254f/255f,0,0);
                break;
            case Player.HeartColorEnum.Blue:
                Modulate = new Color(0,60f/255f,254f/255f);
                break;
            case Player.HeartColorEnum.Green:
                Modulate = new Color(1f/255f,192f/255f,0);
                break;
            case Player.HeartColorEnum.Purple:
                Modulate = new Color(229f/255f,110f/255f,167f/255f);
                break;
            case Player.HeartColorEnum.Yellow:
                Modulate = new Color(1,1,0);
                break;
            case Player.HeartColorEnum.Orange:
                Modulate = new Color(1,127f/255f,39f/255f);
                break; 
        }
        deathSprite.GlobalRotationDegrees = game.Nodes.PlayerNode.GlobalRotationDegrees;
        deathSound.Play();
        await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
        deathParticles.Emitting = true;
        deathSprite.Hide();
    }
}
