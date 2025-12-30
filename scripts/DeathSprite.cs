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
        switch (game.Nodes.PlayerNode._heartSprite.Frame)
        {
            case 0:
                Modulate = new Color(254f/255f,0,0);
                break;
            case 1:
                Modulate = new Color(0,60f/255f,254f/255f);
                break;
            case 2:
                Modulate = new Color(1f/255f,192f/255f,0);
                break;
            case 3:
                Modulate = new Color(229f/255f,110/255f,167/255f);
                break;
            case 4:
                Modulate = new Color(1,1,0);
                deathSprite.GlobalRotationDegrees = 180;
                break;
            case 5:
                Modulate = new Color(1,127f/255f,39f/255f);
                break; 
        }
        deathSound.Play();
        await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
        deathParticles.Emitting = true;
        deathSprite.Hide();
    }
}
