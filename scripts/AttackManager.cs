using game;
using Godot;
using System;
using System.ComponentModel;

public partial class AttackManager : Node2D
{
	[Export] PackedScene bullet1;
	[Export] Node2D player;
	[Export] float bullet1CD;
	float bul1Cd;
	RandomNumberGenerator rnd = new RandomNumberGenerator();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rnd.Randomize();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{      
		if (bul1Cd > 0)
		{
			bul1Cd -= (float)delta;
		}
		double playerTime = Settings.BossFightTime;
		if (playerTime > 0 && playerTime < 20)
		{
			if (bul1Cd <= 0)
			{
				SpawnBullet();
				bul1Cd = bullet1CD;
			}
		}
	}

	void SpawnBullet()
	{
		Vector2 pos;
		Bullet bullet = (Bullet)bullet1.Instantiate();
		if (rnd.RandiRange(0,2) == 1)
		{
			pos.X = rnd.RandfRange(-100,0);
		}
		else
		{
			pos.X = rnd.RandiRange(1200,1300);
		}

		if (rnd.RandiRange(0,2) == 1)
		{
			pos.Y = rnd.RandfRange(-100,0);
		}
		else
		{
			pos.Y = rnd.RandfRange(650,750);
		}

		bullet.GlobalPosition = pos;
		GetTree().CurrentScene.AddChild(bullet);
		bullet.SetVelocity((player.GlobalPosition - bullet.GlobalPosition).Normalized());
	}
}
