using Godot;
using System;

public partial class Player : AnimatedSprite2D
{

	public float Speed = 200f; // Movement speed

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Movement vector
        Vector2 velocity = Vector2.Zero;

        // Input handling
        if (Input.IsActionPressed("ui_up"))
            velocity.Y -= 1;
        if (Input.IsActionPressed("ui_down"))
            velocity.Y += 1;
        if (Input.IsActionPressed("ui_left"))
            velocity.X -= 1;
        if (Input.IsActionPressed("ui_right"))
            velocity.X += 1;

        // Normalize to avoid faster diagonal movement, then apply speed
        velocity = velocity.Normalized() * Speed * (float)delta;
		if (velocity.LengthSquared() > 0) {
			Play("Walking");
		} else {
			Play("Idle");

		}
		Position += velocity;
        
	}
}
