using Godot;
using System;
using System.Threading.Tasks;

public partial class MiniSami : Node2D
{
	float time;
	bool initialized = false;
	RandomNumberGenerator rng = new();
	public override async void _Ready()
	{
		Modulate = new Color(1, 1, 1, 0);
		time = 0f;
		float current = Rotation;
		rng.Randomize();
		if (rng.RandiRange(0, 1) == 1) current = (game.Nodes.PlayerNode.GlobalPosition - GlobalPosition).Angle();
		else current = (new Vector2(568, 387) - GlobalPosition).Angle();
		float target = (game.Nodes.PlayerNode.GlobalPosition - GlobalPosition).Angle();
		float diff = Mathf.Wrap(target - current, -Mathf.Pi, Mathf.Pi);
		float finalTarget;
		if (diff >= 0)
		{
			finalTarget = target + Mathf.Tau;
		}
		else
		{
			finalTarget = target - Mathf.Tau;
		}
		Tween _tween = CreateTween();
		_tween.SetParallel(true);
		_tween.TweenProperty(this, "rotation", finalTarget, 0.6f).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
		_tween.TweenProperty(this, "modulate:a", 1f, 0.6f).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
		await ToSignal(_tween, Tween.SignalName.Finished);
		initialized = true;
	}
	public override void _Process(double delta)
	{
		if (!initialized) return;
		if (game.Settings.BossFightTime < 5.0) QueueFree();
		Position += Transform.X * 500f * (float)delta;
		time += (float)delta;
		if (time >= 3f) QueueFree();
	}

}
