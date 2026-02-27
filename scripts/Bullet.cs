using Godot;

public partial class Bullet : Area2D
{
    private const float Speed = 600f;

    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(0, -Speed * (float)delta);
        if (Position.Y < -10) QueueFree();
    }
}