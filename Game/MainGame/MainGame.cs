using Godot;
using System;
using GameObjects;

public partial class MainGame : Node2D
{
	NuclearReactor reactor = new NuclearReactor();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Called in _Process as a high level game loop function
	private void gameLoop() {
		
	}


}
