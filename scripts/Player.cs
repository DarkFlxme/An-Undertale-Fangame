using System.Runtime.CompilerServices;
using System.Threading;
using Godot;
using game;
public partial class Player : CharacterBody2D
{
	[Export] public int Speed = 200;
	[Export] public int Gravity = 800;
	[Export] public int JumpForce = 450;
	public const int MaxJumpDistance = 150;
	private AnimatedSprite2D _heartSprite;
	private bool _isJumping = false;
	private float _jumpStartY = 0.0f;
	float horizontalDirection = 0;
	float verticalDirection = 0;
	[Export] public Node2D[] lines;
	[Export] public float speedPurple;
	int i = 1;
	public override void _Ready()
	{
		_heartSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;

		if (_heartSprite.Frame != 2)
		{
			Nodes.DisableNode(Nodes.map2);
			Nodes.EnableNode(Nodes.map1);
			Nodes.DisableNode(Nodes.shield_green);
		}

		switch (_heartSprite.Frame)
		{
			case 0: // Red heart
				horizontalDirection = Input.GetAxis("move_left", "move_right");
				verticalDirection = Input.GetAxis("move_up", "move_down");
				Velocity = new Vector2(horizontalDirection, verticalDirection).Normalized() * Speed;
				Nodes.purplelines.Hide();
				break;
			case 1: // Blue heart
				if (RotationDegrees == 0)
				{
					horizontalDirection = Input.GetAxis("move_left", "move_right");
					Velocity = new Vector2(horizontalDirection * Speed, Velocity.Y + Gravity * deltaF);

					if (Input.IsActionJustPressed("move_up") && !_isJumping && IsOnFloor())
					{
						_isJumping = true;
						_jumpStartY = GlobalPosition.Y;
						Velocity = new Vector2(Velocity.X, -JumpForce);
					}

					if (_isJumping)
					{
						if (GlobalPosition.Y <= _jumpStartY - MaxJumpDistance)
						{
							_isJumping = false;
						}
					}

					if (Input.IsActionJustReleased("move_up") && Velocity.Y < 0)
					{
						Velocity = new Vector2(Velocity.X, 0);
						_isJumping = false;
					}

					if (IsOnFloor())
					{
						_isJumping = false;
					}

					if (IsOnCeiling())
					{
						_isJumping = false;
					}
				}
				Nodes.purplelines.Hide();
				break;

			case 2: // Green heart
				Nodes.DisableNode(Nodes.map1);
				Nodes.EnableNode(Nodes.map2);
				Nodes.EnableNode(Nodes.shield_green);
				Position = new Vector2(583, 426);
				Velocity = Vector2.Zero;
				Nodes.purplelines.Hide();
				break;
			case 3: // Purple heart
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

			case 4: // Yellow heart
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

			case 5: // orange heart
				horizontalDirection = Input.GetAxis("move_left", "move_right");
				verticalDirection = Input.GetAxis("move_up", "move_down");
				if (horizontalDirection != 0 || verticalDirection != 0)
					Velocity = new Vector2(horizontalDirection, verticalDirection).Normalized() * Speed;
				Nodes.purplelines.Hide();
				break;
		}

		if (Input.IsActionJustPressed("0")) _heartSprite.Frame = 0;
		else if (Input.IsActionJustPressed("1")) _heartSprite.Frame = 1;
		else if (Input.IsActionJustPressed("2")) _heartSprite.Frame = 2;
		else if (Input.IsActionJustPressed("3"))
		{
			_heartSprite.Frame = 3;
			Velocity = Vector2.Zero;
			GlobalPosition = new Vector2(583, lines[1].GlobalPosition.Y);
		}
		else if (Input.IsActionJustPressed("4")) _heartSprite.Frame = 4;
		else if (Input.IsActionJustPressed("5")) _heartSprite.Frame = 5;
		MoveAndSlide();
	}
}
