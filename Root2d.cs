using Godot;
using System;

public partial class Root2d : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Instantiate Main Menu on game load
		PackedScene mainMenuScenePacked = GD.Load<PackedScene>("res://MainMenu/MainMenu.tscn");
		var mainMenuScene = mainMenuScenePacked.Instantiate();
		AddChild(mainMenuScene);
		GD.Print(mainMenuScene.position);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
