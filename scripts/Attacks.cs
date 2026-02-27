using Godot;
using System;

public partial class Attacks : Node2D
{
	RandomNumberGenerator rng = new RandomNumberGenerator();
	bool attack = false;
	int x, y;
	private void SpawnMiniSami()
	{

		rng.Randomize();
		int roll = rng.RandiRange(1, 247);

		if (roll <= 199)
		{
			y = roll;          // 1–199
		}
		else
		{
			y = roll + 401;    // 601–648
		}

		roll = rng.RandiRange(1, 401);
		if (roll <= 199)
		{
			x = roll;           // 1–199
		}
		else
		{
			x = roll + 751;     // 951–1152
		}

		var sami = GD.Load<PackedScene>("res://scenes/Samihead.tscn").Instantiate<MiniSami>();
		sami.Position = new Vector2(x, y);
		AddChild(sami);
	}
	private async void Attack1()
	{
		if (!attack)
		{
			attack = true;
			SpawnMiniSami();
			await ToSignal(GetTree().CreateTimer(0.4f), SceneTreeTimer.SignalName.Timeout);
			attack = false;
		}
	}
	public override void _Process(double delta)
	{
		switch (Name)
		{
			case "Attack1":
				Attack1();
				break;
		}
	}
}
