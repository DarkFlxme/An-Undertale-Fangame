using Godot;
using System;

public partial class Platform : AnimatableBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(-150 * (float)delta, 0);
    }

}
