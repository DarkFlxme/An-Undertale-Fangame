using Godot;
using System;

public partial class Listecik : StaticBody2D
{
	public int i = 0;
	[Export] public Node2D[] secenekler;
	Vector2 firstScale;
	Vector2 targetScale;
	[Export] float speed;
	[Export] float buyuklukKati;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		firstScale = secenekler[0].GlobalScale;
		targetScale = new Vector2(firstScale.X * buyuklukKati, firstScale.Y * buyuklukKati);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (Node2D secenek in secenekler)
		{
			if (secenek == secenekler[i])
			{
				secenek.GlobalScale = secenek.GlobalScale.Lerp(targetScale, speed * (float)delta);
			}
			else
			{
				secenek.GlobalScale = secenek.GlobalScale.Lerp(firstScale, speed * (float)delta);
			}
		}
	}
}
