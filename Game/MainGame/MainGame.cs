using Godot;
using System;
using GameObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;

public partial class MainGame : Node2D
{
	public NuclearReactor Reactor;

	public double CurrentWattage = 0;

	public Dictionary<string, PackedScene> packedScenes = new Dictionary<string, PackedScene>();
    public Dictionary<string, PackedScene> packedRoomScenes = new Dictionary<string, PackedScene>();

	public LabelManager debugLabels;
	public LabelManager UiLabels;

	public Button RaiseControlRodsButton;
	public Button LowerControlRodsButton;	

	public Node CurrentScene;
	public Player PlayerRef;
	public GameController gameController;
	public bool MeltdownOccured = false;

	public override void _Ready()
	{
		gameController = GetParent<GameController>();
		Reactor = new NuclearReactor(meltdown);

		// Load all game scenes into memory
		packedRoomScenes.Add("ControlRoom", GD.Load<PackedScene>("res://Game/MainGame/ControlRoom/ControlRoom.tscn"));
		packedRoomScenes.Add("TurbineRoom", GD.Load<PackedScene>("res://Game/MainGame/TurbineRoom/TurbineRoom.tscn"));
        packedRoomScenes.Add("ReactorRoom", GD.Load<PackedScene>("res://Game/MainGame/ReactorRoom/ReactorRoom.tscn"));;
		packedScenes.Add("Player", GD.Load<PackedScene>("res://Game/MainGame/Player.tscn"));
		packedScenes.Add("DebugLabels", GD.Load<PackedScene>("res://Game/MainGame/DebugLabels.tscn"));
		packedScenes.Add("UserInterface", GD.Load<PackedScene>("res://Game/MainGame/UserInterface.tscn"));

		// Start in the control room
		var controlRoomScene = packedRoomScenes["ControlRoom"].Instantiate();
		AddChild(controlRoomScene);
		CurrentScene = controlRoomScene;		
		var playerScene = packedScenes["Player"].Instantiate<Player>();
		AddChild(playerScene);
		PlayerRef = playerScene;
		var playerSpawnNode = controlRoomScene.GetNode<Node2D>("InitialPlayerSpawn");
		PlayerRef.Position = playerSpawnNode.Position;
		var debugLabelsScene = packedScenes["DebugLabels"].Instantiate();
		AddChild(debugLabelsScene);
		var uiScene = packedScenes["UserInterface"].Instantiate();
		AddChild(uiScene);
		PlayerRef.UserInterfaceNode = uiScene;
		
		
		debugLabels = new LabelManager(GetNode("DebugLabels"));
		UiLabels = new LabelManager(GetNode("UserInterface"));

		GD.Print("Main Game script is ready");
		
	}

	public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("dev_mode")) {
			GD.Print("God mode");
			
		}
    }

	public void SwitchScenes(string newScene, string nodeToTeleportTo = null) {
		if (!packedRoomScenes.ContainsKey(newScene)) {
			GD.PrintErr($"Invalid scene argument in SwitchScenes! {newScene}");
			return;
		}
		
		if (CurrentScene != null) CurrentScene.QueueFree();
		var newLoadedScene = packedRoomScenes[newScene].Instantiate();
		AddChild(newLoadedScene);

		CurrentScene = newLoadedScene;

		if (nodeToTeleportTo != null) {
			var node = newLoadedScene.GetNode<Node2D>(nodeToTeleportTo);
			PlayerRef.Position = node.Position;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		gameController.GameStats.TimeLeftInDay -= delta;
		if (gameController.GameStats.TimeLeftInDay <= 0) {
			gameController.dayResult = DayResult.OutOfTime;
			gameController.AdvanceGamePhase();
			return;
		}
		if (MeltdownOccured) {
			gameController.dayResult = DayResult.Meltdown;
			gameController.AdvanceGamePhase();
		}
		if (gameController.GameStats.MegaWattHoursGeneratedToday >= gameController.GameStats.MegaWattHourRequirementToday) {
			gameController.dayResult = DayResult.Success;
			gameController.AdvanceGamePhase();
			return;
		}

		Reactor.Degrade(delta);
		double energyGenerated = Reactor.GameTick(delta);
        double megawattHours = energyGenerated / 1e6 / 60;
		gameController.GameStats.MegaWattHoursGeneratedToday += megawattHours;        
		CurrentWattage = energyGenerated / delta;
		
		updateLabels();
	}

	private void meltdown() {
		MeltdownOccured = true;
	}

	private void updateLabels() {
		UiLabels.UpdateLabel("GeneratedLabel", $"Electricity Generated: {gameController.GameStats.MegaWattHoursGeneratedToday:F1} / {gameController.GameStats.MegaWattHourRequirementToday:G}");
		UiLabels.UpdateLabel("TimeLabel", $"Time Left {(int)Math.Floor(gameController.GameStats.TimeLeftInDay/60):D2}:{(int)Math.Floor(gameController.GameStats.TimeLeftInDay % 60):D2}");
		debugLabels.UpdateLabel("ReactorCoreTempLabel", $"Reactor Core Temperature: {Reactor.CoreTemperature - 273.15:F2} Degrees Celsius");		
		debugLabels.UpdateLabel("CoolantTempLabel", $"Coolant Temperature: {Reactor.coolant.Temperature - 273.15:F2} Degrees Celsius");
		debugLabels.UpdateLabel("WattageLabel", $"Current Generation: {CurrentWattage/1000000:F2} Megawatts");
		debugLabels.UpdateLabel("FuelLevelLabel", $"Fuel Level: {Reactor.FuelFreshness:P}");
		debugLabels.UpdateLabel("ControlRodDepthLabel", $"Control Rod Depth: {Reactor.ControlRodDepth:P}");
		debugLabels.UpdateLabel("TurbineRepairLabel", $"Turbine Repair Level: {Reactor.turbineBay.RepairLevel:P}");
		debugLabels.UpdateLabel("RunCooldownLabel", $"Run Cooldown: {PlayerRef.RunCooldown:F1}");
	}

}
