using Godot;
using System;

public partial class Root : Node2D
{
	PackedScene mainMenuScenePacked;
	PackedScene gameControllerScenePacked;
	
	GameController GameScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Instantiate Main Menu on game load
		mainMenuScenePacked = GD.Load<PackedScene>("res://MainMenu/MainMenu.tscn");
		gameControllerScenePacked = GD.Load<PackedScene>("res://Game/GameController.tscn");
		GoToMainMenu();	
	}

	public void GoToMainMenu() {
		var mainMenuScene = mainMenuScenePacked.Instantiate<CanvasLayer>();
		AddChild(mainMenuScene);
		Button playButton = GetNode<Button>("MainMenu/MarginContainer/VBoxContainer/PlayButton");
		playButton.Pressed += StartGame;
		Button quitButton = GetNode<Button>("MainMenu/MarginContainer/VBoxContainer/QuitButton");
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
		GameScene = gameControllerScenePacked.Instantiate<GameController>();
		GameScene.EndGame = EndGame;
		AddChild(GameScene);		
	}

	public void EndGame() {
		GameScene.QueueFree();
		GoToMainMenu();
	}
}
