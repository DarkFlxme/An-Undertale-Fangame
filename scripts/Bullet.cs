using Godot;

public partial class Bullet : Area2D
{
    [Export] float speed;
    int wallTouched;
    Vector2 velocity;
    float timer  = 5;

    public override void _Ready()
    {
        velocity = new Vector2(speed,speed);
    }

    public override void _Process(double delta)
    {
        GlobalPosition += velocity * (float)delta;
        LookAt(GlobalPosition + velocity.Normalized());
        timer -= (float)delta;
        if (timer <= 0)
        {
            QueueFree();
        }
    }
    public void SetVelocity(Vector2 vel)
    {
        velocity *= vel;
        GD.Print("ew");
    }

    void BodyEntered2D(Node2D body)
    {
        if (wallTouched > 0 && wallTouched < 3 && body.IsInGroup("Walls"))
        {
            if (body.IsInGroup("DikeyWalls"))
            {
                velocity.X *= -1;
            }
            else
            {
                velocity.Y *= -1;
            }
        }
        if (body.IsInGroup("Walls"))
        {
            wallTouched++;
        }
    }

}