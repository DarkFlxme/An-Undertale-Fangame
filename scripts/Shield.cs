using Godot;
using System;

public partial class Shield : Node2D
{
	[Export] public float speed;
	float a = 1;
	float b = 1;
	float targetRot;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		targetRot = toRad(180);
		GlobalRotation = targetRot;
	}

	public float toRad(float x)
	{
		return Mathf.DegToRad(x);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalRotation = Mathf.RotateToward(GlobalRotation, targetRot, speed * (float)delta);
		if (Input.IsActionJustPressed("move_up"))
		{
			b = 1;
		}
		else if (Input.IsActionJustPressed("move_right"))
		{
			b = 2;
		}
		else if (Input.IsActionJustPressed("move_down"))
		{
			b = 3;
		}
		else if (Input.IsActionJustPressed("move_left"))
		{
			b = 4;
		}

		if (b - a == 3)
		{
			targetRot += toRad(-90);
		}
		else if (b - a == -3)
		{
			targetRot += toRad(90);
		}
		else
		{
			targetRot += toRad((b - a) * 90);
		}
		a = b;
	}
}
