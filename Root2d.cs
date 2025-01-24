using Godot;
using System;

public partial class Root2d : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Instantiate Main Menu on game load
		PackedScene mainMenuScenePacked = GD.Load<PackedScene>("res://MainMenu/MainMenu.tscn");
		var mainMenuScene = mainMenuScenePacked.Instantiate<Control>();
		AddChild(mainMenuScene);
		Vector2 windowSize = DisplayServer.WindowGetSize();
		mainMenuScene.Size = windowSize;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
