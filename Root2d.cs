using Godot;
using System;

public partial class Root2d : Node2D
{
	PackedScene mainMenuScenePacked;
	PackedScene mainGameScenePacked;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Instantiate Main Menu on game load
		mainMenuScenePacked = GD.Load<PackedScene>("res://MainMenu/MainMenu.tscn");
		mainGameScenePacked = GD.Load<PackedScene>("res://Game/MainGame/MainGame.tscn");
		GoToMainMenu();	
	}

	public void GoToMainMenu() {
		var mainMenuScene = mainMenuScenePacked.Instantiate<Control>();
		AddChild(mainMenuScene);
		Button playButton = GetNode<Button>("MainMenu/PlayButton");
		playButton.Pressed += StartGame;
		Button quitButton = GetNode<Button>("MainMenu/QuitButton");
		quitButton.Pressed += QuitApplication;
	}

	public void LeaveMainMenu() {
		var mainMenu = GetNode("MainMenu");
		mainMenu.QueueFree();
	}

	public void QuitApplication() {
		GetTree().Quit(0);
	}

	public void StartGame() {
		LeaveMainMenu();
		var GameScene = mainGameScenePacked.Instantiate<MainGame>();
		GameScene.EndGame = EndGame;
		AddChild(GameScene);		
	}

	public void EndGame() {

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
