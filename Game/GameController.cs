using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

public partial class GameController : Node2D
{
	public Action EndGame = () => {};
	public Dictionary<string, PackedScene> packedScenes = new Dictionary<string, PackedScene>();	
	public GamePhase CurrentGamePhase = GamePhase.DayStart;
	public Node CurrentGameScene;
	public MainGame mainGame;
	public DayEnd dayEnd;
	public GameStatus GameStats = new GameStatus();	

	public double ChanceForDailyEvent = 0.5;

	// Seconds, represents minutes
	public double TimePerDay = 8 * 60;
	public double TimeLeft = 8 * 60;
	// Megawatt minutes, represents megawatt hours
	public double EnergyTarget = 1500;

	public DayResult dayResult;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		packedScenes.Add("DayStart", GD.Load<PackedScene>("res://Game/DayStart/DayStart.tscn"));
		packedScenes.Add("DayEnd", GD.Load<PackedScene>("res://Game/DayEnd/DayEnd.tscn"));
		packedScenes.Add("MainGame", GD.Load<PackedScene>("res://Game/MainGame/MainGame.tscn"));
		CurrentGameScene = packedScenes["DayStart"].Instantiate();
		AddChild(CurrentGameScene);
		CurrentGamePhase = GamePhase.DayStart;
	}

	public void AdvanceGamePhase() {
		switch (CurrentGamePhase) {
			case GamePhase.DayStart:		
				CurrentGameScene.QueueFree();		
				CurrentGameScene = packedScenes["MainGame"].Instantiate();
				mainGame = (MainGame)CurrentGameScene;
				CurrentGamePhase = GamePhase.MainGame;
				break;				
			case GamePhase.MainGame:
				CurrentGameScene = packedScenes["DayEnd"].Instantiate();
				dayEnd = (DayEnd)CurrentGameScene;
				dayEnd.GetNode<Button>("VBoxContainer/NextDayButton").Pressed += AdvanceGamePhase;
				CurrentGamePhase = GamePhase.DayEnd;
				EndDay();
				break;
			case GamePhase.DayEnd:
				mainGame.QueueFree();
				CurrentGameScene.QueueFree();
				CurrentGameScene = packedScenes["DayStart"].Instantiate();
				CurrentGamePhase = GamePhase.DayStart;
				StartDay();
				break;			
		}
		AddChild(CurrentGameScene);
	}

	public void EndDay() {
		mainGame.SetProcess(false);
	}

	public void StartDay() {
		GameStats.CurrentDay++;
		GameStats.MegaWattHoursGeneratedToday = 0;

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
