using System.Runtime.CompilerServices;
using System.Threading;
using Godot;
using game;
public partial class Player : CharacterBody2D
{
	public const int Speed = 200;
	public const int Gravity = 800;
	public const int JumpForce = 450;
	public const int MaxJumpDistance = 150;
	private AnimatedSprite2D _heartSprite;
	private bool _isJumping = false;
	private float _jumpStartY = 0.0f;
	float horizontalDirection = 0;
	float verticalDirection = 0;
	public override void _Ready()
	{
		_heartSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		if (_heartSprite.Frame == 0)//red heart
		{
			horizontalDirection = Input.GetAxis("move_left", "move_right");
			verticalDirection = Input.GetAxis("move_up", "move_down");
			Velocity = new Vector2(horizontalDirection, verticalDirection).Normalized() * Speed;
		}
		else if (_heartSprite.Frame == 1) //blue heart
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
		else if (_heartSprite.Frame == 2) //green heart
		{
			Nodes.DisableNode(Nodes.map1);
			Nodes.EnableNode(Nodes.map2);
			Position = new Vector2(583, 426);
			Velocity = new Vector2(0, 0);
		}
		else if (_heartSprite.Frame == 3) // purple heart
		{

		}
		else if (_heartSprite.Frame == 4) // yellow heart
		{

			horizontalDirection = Input.GetAxis("move_left", "move_right");
			verticalDirection = Input.GetAxis("move_up", "move_down");
			Velocity = new Vector2(horizontalDirection * Speed, verticalDirection * Speed);
			if (Input.IsActionJustPressed("yellow_heart_shot"))
			{
				if (Nodes.BulletTimer.TimeLeft == 0)
				{
					var bullet = GD.Load<PackedScene>("res://scenes//Bullet.tscn").Instantiate<Bullet>();
					bullet.Position = Position;
					GetParent().AddChild(bullet);
					Nodes.BulletTimer.Start(0.3);
				}
			}
		}
		else if (_heartSprite.Frame == 5)
		{
			horizontalDirection = Input.GetAxis("move_left", "move_right");
			verticalDirection = Input.GetAxis("move_up", "move_down");
			if (horizontalDirection != 0 || verticalDirection != 0)
				Velocity = new Vector2(horizontalDirection, verticalDirection).Normalized() * Speed;
		}
		if (Input.IsActionJustPressed("0")) _heartSprite.Frame = 0;
		else if (Input.IsActionJustPressed("1")) _heartSprite.Frame = 1;
		else if (Input.IsActionJustPressed("2")) _heartSprite.Frame = 2;
		else if (Input.IsActionJustPressed("3")) _heartSprite.Frame = 3;
		else if (Input.IsActionJustPressed("4")) _heartSprite.Frame = 4;
		else if (Input.IsActionJustPressed("5")) _heartSprite.Frame = 5;
		MoveAndSlide();
	}
}
