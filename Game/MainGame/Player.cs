using Godot;
using System;

public partial class Player : AnimatedSprite2D
{

	public float WalkingSpeed = 200f; // Movement speed

	public double RunEnergy = 1.0;
	public double RunEnergyUsedPerSecond = 0.1;
	public double RunEnergyRechargeRate = 0.1;
	public float RunSpeedMultiplier = 2f;
	public double RunCooldown = 0.0;
	public bool CurrentlyRunning = false;

	public void ChangeRunEnergy(double changeAmount) {
		RunEnergy = Math.Clamp(RunEnergy + changeAmount, 0.0, 1.0);
		UserInterfaceNode.GetNode<ProgressBar>("Control/VBoxContainer/ProgressBar").Value = RunEnergy;
	}

	public Node UserInterfaceNode;

	private void determineRun(double delta) {
		if (Input.IsActionPressed("sprint")) {
			if (RunCooldown > 0) {
				RunCooldown = Math.Max(RunCooldown, 1.5);
				CurrentlyRunning = false;
				return;
			}
			if (RunEnergy == 0) {
				RunCooldown = 3.0;
				CurrentlyRunning = false;
				return;
			}
			CurrentlyRunning = true;			
		} else {
			// Sprint is not currently pressed
			if (CurrentlyRunning == true) {
				RunCooldown = Math.Max(RunCooldown, 1.5);
				CurrentlyRunning = false;
				return;
			}
			if (RunCooldown > 0) {
				RunCooldown = Math.Max(0.0, RunCooldown - delta);
				return;
			}
			ChangeRunEnergy(RunEnergyRechargeRate*delta);
		}			
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float currentSpeed = WalkingSpeed;

		determineRun(delta);
		if (CurrentlyRunning) {
			currentSpeed *= RunSpeedMultiplier;
			
		}

		// Movement vector
        Vector2 velocity = Vector2.Zero;

        // Input handling
        if (Input.IsActionPressed("move_up"))
            velocity.Y -= 1;
        if (Input.IsActionPressed("move_down"))
            velocity.Y += 1;
        if (Input.IsActionPressed("move_left"))
            velocity.X -= 1;
        if (Input.IsActionPressed("move_right"))
            velocity.X += 1;

        // Normalize to avoid faster diagonal movement, then apply speed
        velocity = velocity.Normalized() * currentSpeed * (float)delta;
		if (velocity.LengthSquared() > 0) {
			Play("Walking");
            if (CurrentlyRunning) {
                ChangeRunEnergy(-RunEnergyUsedPerSecond * delta);
            }
		} else {
			Play("Idle");

		}
		Position += velocity;

		// Interaction handler
		if (Input.IsActionJustPressed("interact"))
        {
            EmitSignal(SignalName.InteractionAttempted);
        }
	}

	[Signal] public delegate void InteractionAttemptedEventHandler();
}
