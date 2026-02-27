using game;
using Godot;
using System;

public partial class GameUI : Control
{
    private Label Label;
    private Sprite2D samoystaBoss;
    private int maxHealth;
    private bool hadRun = false;
    private string UIText;
    public override async void _Ready()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        Label = GetNode<Label>("Label2");
        samoystaBoss = GetNode<Sprite2D>("Samoysta");
    }
    public override void _Process(double delta)
    {
        if (!hadRun)
        {
            hadRun = true;
            maxHealth = Nodes.PlayerNode.health;
            if (Settings.GameDifficulty == Settings.Difficulty.Extreme)
                UIText = "      ZINA   {0:D2}:{1:D2}   HP {2}/{3}";
            else
                UIText = "ZINA  {0:D2}:{1:D2}  HP         {2}/{3}";
        }
        Settings.BossFightTime += delta;
        TimeSpan timeSpan = TimeSpan.FromSeconds(Settings.BossFightTime);
        Label.Text = string.Format(UIText, timeSpan.Minutes, timeSpan.Seconds, Nodes.PlayerNode.health, maxHealth);
        if (Settings.BossFightTime >= 5.0)
        {
            Nodes.attacks.ProcessMode = ProcessModeEnum.Inherit;
            Nodes.EnableNode(Nodes.attacks);
            SamoystaChangeTexture(false);
        }
    }
    private void SamoystaChangeTexture(bool texture) // true = normal, false = dehşet
    {
        if (texture)
        {
            samoystaBoss.Texture = GD.Load<Texture2D>("res://assets/samiboss1.png");
        }
        else
        {
            samoystaBoss.Texture = GD.Load<Texture2D>("res://assets/samiboss0.png");
        }
    }
}
