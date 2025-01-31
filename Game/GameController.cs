using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

public partial class GameController : Node2D
{
	public Action EndGame = () => {};
	public Dictionary<string, PackedScene> packedScenes = new Dictionary<string, PackedScene>();	
	public GamePhases CurrentGamePhase = GamePhases.DayStart;
	public Node CurrentGameScene;
	public GameStatus GameStats = new GameStatus();	

	public double ChanceForDailyEvent = 0.5;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		packedScenes.Add("DayStart", GD.Load<PackedScene>("res://Game/DayStart/DayStart.tscn"));
		packedScenes.Add("DayEnd", GD.Load<PackedScene>("res://Game/DayEnd/DayEnd.tscn"));
		packedScenes.Add("MainGame", GD.Load<PackedScene>("res://Game/MainGame/MainGame.tscn"));
		CurrentGameScene = packedScenes["DayStart"].Instantiate();
		AddChild(CurrentGameScene);
		CurrentGamePhase = GamePhases.DayStart;
	}

	public void AdvanceGamePhase() {
		CurrentGameScene.QueueFree();
		switch (CurrentGamePhase) {
			case GamePhases.DayStart:				
				CurrentGameScene = packedScenes["MainGame"].Instantiate();
				CurrentGamePhase = GamePhases.MainGame;
				break;				
			case GamePhases.MainGame:
				CurrentGameScene = packedScenes["DayEnd"].Instantiate();
				CurrentGamePhase = GamePhases.DayEnd;
				EndDay();
				break;
			case GamePhases.DayEnd:
				CurrentGameScene = packedScenes["DayStart"].Instantiate();
				CurrentGamePhase = GamePhases.DayStart;
				StartDay();
				break;			
		}
		AddChild(CurrentGameScene);
	}

	public void EndDay() {
		
	}

	public void StartDay() {
		GameStats.CurrentDay++;
		GameStats.WattHoursGeneratedToday = 0;

		// Decide random event for the day
		Random rand = new Random();
		if (rand.NextDouble() < ChanceForDailyEvent) {			
			int eventCount = Enum.GetNames(typeof(RandomEventsDaily)).Length-1;
			int eventIndex = rand.Next(eventCount);
			GameStats.CurrentRandomDaily = (RandomEventsDaily)eventIndex;
		} else {
			GameStats.CurrentRandomDaily = RandomEventsDaily.NoEvent;
		}

	}
	
	
}
