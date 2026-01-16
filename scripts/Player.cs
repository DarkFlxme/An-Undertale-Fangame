using System.Runtime.CompilerServices;
using System.Threading;
using Godot;
using game;
public partial class Player : CharacterBody2D
{
	[Export] public int Speed = 200;
	[Export] public int Gravity = 800;
	[Export] public int JumpForce = 450;
	[Export] public Sprite2D heartSprite;
	[Export] public Area2D damageHitbox;
	[Export] public AnimationPlayer damageBlink;
	bool canJump;
	bool isJumping;
	float horizontalDirection = 0;
	float verticalDirection = 0;
	public int health;
	public int maxHealth;
	public int damage;
	[Export] public ProgressBar healthBar;
	[Export] public Node2D[] lines;
	[Export] public float speedPurple;
	int i = 1;
	int z = 0;
	public enum HeartColorEnum
	{
		Red,
		Blue,
		Green,
		Purple,
		Yellow,
		Orange
	}
	public HeartColorEnum HeartColor = HeartColorEnum.Red;
	public override void _Ready()
	{
		damageHitbox.AreaEntered += DamageHitboxEntered;
	}
	public override void _PhysicsProcess(double delta)
	{
		healthBar.Value = health;
		float deltaF = (float)delta;

		if (HeartColor != HeartColorEnum.Green)
		{
			Nodes.DisableNode(Nodes.map2);
			Nodes.EnableNode(Nodes.map1);
			Nodes.DisableNode(Nodes.shield_green);
		}
		if (HeartColor != HeartColorEnum.Blue || HeartColor != HeartColorEnum.Yellow)
		{
			GlobalRotationDegrees = 0;
		}

		switch (HeartColor)
		{
			case HeartColorEnum.Red:
				horizontalDirection = Input.GetAxis("move_left", "move_right");
				verticalDirection = Input.GetAxis("move_up", "move_down");
				Velocity = new Vector2(horizontalDirection, verticalDirection).Normalized() * Speed;
				Nodes.purplelines.Hide();
				Modulate= new Color(254f/255f,0,0);
				break;
			case HeartColorEnum.Blue:
				GlobalRotationDegrees = 90 * z;
				Modulate= new Color(0,60f/255f,254f/255f);
				if (GlobalRotationDegrees == 0)
				{
					horizontalDirection = Input.GetAxis("move_left", "move_right");
					Velocity = new Vector2(horizontalDirection * Speed, Velocity.Y + Gravity * deltaF);
					if (canJump && Input.IsActionJustPressed("move_up"))
					{
						Velocity = new Vector2(Velocity.X, -JumpForce);
						isJumping = true;
					}
					if (isJumping && Input.IsActionJustReleased("move_up"))
					{
						Velocity = new Vector2(Velocity.X, 0);
					}
					if (isJumping && Velocity.Y > 0)
					{
						isJumping = false;
					}
				}
				else if (GlobalRotationDegrees == -90)
				{
					Velocity = new Vector2(Velocity.X + Gravity * deltaF, Velocity.Y);
					if (Input.IsActionPressed("move_up"))
					{
						Velocity = new Vector2(Velocity.X, -Speed);
					}
					else if (Input.IsActionPressed("move_down"))
					{
						Velocity = new Vector2(Velocity.X, Speed);
					}
					else
					{
						Velocity = new Vector2(Velocity.X, 0);
					}
					if (canJump && Input.IsActionJustPressed("move_left"))
					{
						Velocity = new Vector2(-JumpForce, Velocity.Y);
						isJumping = true;
					}
					if (isJumping && Input.IsActionJustReleased("move_left"))
					{
						Velocity = new Vector2(0, Velocity.Y);
					}
					if (isJumping && Velocity.X > 0)
					{
						isJumping = false;
					}
				}
				else if (GlobalRotationDegrees == 90)
				{
					Velocity = new Vector2(Velocity.X + -Gravity * deltaF, Velocity.Y);
					if (Input.IsActionPressed("move_up"))
					{
						Velocity = new Vector2(Velocity.X, -Speed);
					}
					else if (Input.IsActionPressed("move_down"))
					{
						Velocity = new Vector2(Velocity.X, Speed);
					}
					else
					{
						Velocity = new Vector2(Velocity.X, 0);
					}
					if (canJump && Input.IsActionJustPressed("move_right"))
					{
						Velocity = new Vector2(JumpForce, Velocity.Y);
						isJumping = true;
					}
					if (isJumping && Input.IsActionJustReleased("move_right"))
					{
						Velocity = new Vector2(0, Velocity.Y);
					}
					if (isJumping && Velocity.X < 0)
					{
						isJumping = false;
					}
				}
				else if (Mathf.Abs(GlobalRotationDegrees + 180) <= 1)
				{
					horizontalDirection = Input.GetAxis("move_left", "move_right");
					Velocity = new Vector2(horizontalDirection * Speed, Velocity.Y + -Gravity * deltaF);
					if (canJump && Input.IsActionJustPressed("move_down"))
					{
						Velocity = new Vector2(Velocity.X, JumpForce);
						isJumping = true;
					}
					if (isJumping && Input.IsActionJustReleased("move_down"))
					{
						Velocity = new Vector2(Velocity.X, 0);
					}
					if (isJumping && Velocity.Y < 0)
					{
						isJumping = false;
					}
				}

				Nodes.purplelines.Hide();
				break;

			case HeartColorEnum.Green:
				Modulate= new Color(1f/255f,192f/255f,0);
				Nodes.DisableNode(Nodes.map1);
				Nodes.EnableNode(Nodes.map2);
				Nodes.EnableNode(Nodes.shield_green);
				Position = new Vector2(583, 426);
				Velocity = Vector2.Zero;
				Nodes.purplelines.Hide();
				break;
			case HeartColorEnum.Purple:
				Modulate= new Color(229f/255f,110f/255f,167f/255f);
				Nodes.purplelines.Show();
				GlobalPosition = new Vector2(GlobalPosition.X, Mathf.MoveToward(GlobalPosition.Y, lines[i].GlobalPosition.Y, speedPurple * deltaF));
				horizontalDirection = Input.GetAxis("move_left", "move_right");
				Velocity = new Vector2(horizontalDirection * Speed, Velocity.Y);
				if (Input.IsActionJustPressed("move_up"))
				{
					if (i != 0)
					{
						i--;
					}
				}
				else if (Input.IsActionJustPressed("move_down"))
				{
					if (i != 2)
					{
						i++;
					}
				}
				break;

			case HeartColorEnum.Yellow:
				Modulate= new Color(1,1,0);
				GlobalRotationDegrees = 180;
				horizontalDirection = Input.GetAxis("move_left", "move_right");
				verticalDirection = Input.GetAxis("move_up", "move_down");
				Velocity = new Vector2(horizontalDirection * Speed, verticalDirection * Speed);

				if (Input.IsActionPressed("yellow_heart_shot"))
				{
					if (Nodes.BulletTimer.TimeLeft == 0)
					{
						var bullet = GD.Load<PackedScene>("res://scenes/Bullet.tscn").Instantiate<Bullet>();
						bullet.Position = Position;
						GetParent().AddChild(bullet);
						Nodes.BulletTimer.Start(0.3);
					}
				}
				Nodes.purplelines.Hide();
				break;

			case HeartColorEnum.Orange:
				Modulate= new Color(1,127f/255f,39f/255f);
				horizontalDirection = Input.GetAxis("move_left", "move_right");
				verticalDirection = Input.GetAxis("move_up", "move_down");
				if (horizontalDirection != 0 || verticalDirection != 0)
					Velocity = new Vector2(horizontalDirection, verticalDirection).Normalized() * Speed;
				Nodes.purplelines.Hide();
				break;
		}

		if (Input.IsActionJustPressed("0")) HeartColor = HeartColorEnum.Red;
		else if (Input.IsActionJustPressed("1") && HeartColor != HeartColorEnum.Blue)
		{
			HeartColor = HeartColorEnum.Blue;
			z = 0;
		}
		else if (Input.IsActionJustPressed("1") && HeartColor == HeartColorEnum.Blue)
		{
			if (z == 2)
			{
				z = -1;
			}
			else if (z == -1)
			{
				z = 0;
			}
			else
			{
				z++;
			}
		}
		else if (Input.IsActionJustPressed("2")) HeartColor = HeartColorEnum.Green;
		else if (Input.IsActionJustPressed("3"))
		{
			HeartColor = HeartColorEnum.Purple;
			Velocity = Vector2.Zero;
			GlobalPosition = new Vector2(583, lines[1].GlobalPosition.Y);
		}
		else if (Input.IsActionJustPressed("4")) HeartColor = HeartColorEnum.Yellow;
		else if (Input.IsActionJustPressed("5")) HeartColor = HeartColorEnum.Orange;
		MoveAndSlide();
	}

	void JumpingBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Walls"))
		{
			canJump = true;
		}
	}
	void JumpingBodyExited(Node2D body)
	{
		if (body.IsInGroup("Walls"))
		{
			canJump = false;
		}
	}
	void DamageHitboxEntered(Node2D body)
	{
		if (body.IsInGroup("Attacks"))
		{
			health -= damage;
			if (health > 0)
			{
				GetNode<AudioStreamPlayer>("DamageTakenAudioPlayer").Play();
				damageBlink.Play("i frame blink");
				damageHitbox.SetDeferred("monitorable", false);
				damageHitbox.SetDeferred("monitoring", false);
				GetTree().CreateTimer(1.5).Timeout += () =>
				{
					damageBlink.Stop();
					damageHitbox.SetDeferred("monitorable", true);
					damageHitbox.SetDeferred("monitoring", true);
				};
			}
			else
			{
				Nodes.FadeRect.Color = new Color(0, 0, 0, 1);
				var deathSprite = GD.Load<PackedScene>("res://scenes/deathsprite.tscn").Instantiate<DeathSprite>();
				GetNode<CanvasLayer>("../CanvasLayer").AddChild(deathSprite);
			}
		}
	}
}
