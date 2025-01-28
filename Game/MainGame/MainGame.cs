using Godot;
using System;
using GameObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;

public partial class MainGame : Node2D
{
	NuclearReactor reactor;

	public double TotalEnergyGenerated = 0;

	public Queue<double> Last5EnergyTicks = new Queue<double>( new double[10]);

	public Dictionary<string, PackedScene> packedScenes = new Dictionary<string, PackedScene>();

	public DebugLabelManager debugLabels;

	public Button RaiseControlRodsButton;
	public Button LowerControlRodsButton;

	public Action EndGame = () => {};

	public Node CurrentScene;
	public Player Player;

	public double DisplayWattage() {
		if (Last5EnergyTicks.Count == 0) return 0;
		return Last5EnergyTicks.Sum()/Last5EnergyTicks.Count; 
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load all game scenes into memory
		packedScenes.Add("ControlRoom", GD.Load<PackedScene>("res://Game/MainGame/ControlRoom/ControlRoom.tscn"));
		packedScenes.Add("TurbineRoom", GD.Load<PackedScene>("res://Game/MainGame/TurbineRoom/TurbineRoom.tscn"));
		packedScenes.Add("Player", GD.Load<PackedScene>("res://Game/MainGame/Player.tscn"));
		packedScenes.Add("DebugLabels", GD.Load<PackedScene>("res://Game/MainGame/DebugLabels.tscn"));

		// Start in the control room
		var controlRoomScene = packedScenes["ControlRoom"].Instantiate();
		AddChild(controlRoomScene);
		CurrentScene = controlRoomScene;		
		var playerScene = packedScenes["Player"].Instantiate<Player>();
		AddChild(playerScene);
		Player = playerScene;
		var debugLabelsScene = packedScenes["DebugLabels"].Instantiate();
		AddChild(debugLabelsScene);
		
		reactor = new NuclearReactor(meltdown);
		debugLabels = new DebugLabelManager(GetNode("DebugLabels"));

		RaiseControlRodsButton = GetNode<Button>("BackgroundImage/RaiseControlRodsButton");
		RaiseControlRodsButton.ButtonDown += () => {reactor.ControlRodState = 1;};
		RaiseControlRodsButton.ButtonUp += () => {reactor.ControlRodState = 0;};
		LowerControlRodsButton = GetNode<Button>("BackgroundImage/LowerControlRodsButton");
		LowerControlRodsButton.ButtonDown += () => {reactor.ControlRodState = -1;};
		LowerControlRodsButton.ButtonUp += () => {reactor.ControlRodState = 0;};

		GD.Print("Main Game script is ready");
		
	}

	public void SwitchScenes(string newScene, string nodeToTeleportTo = null) {
		if (!packedScenes.ContainsKey(newScene)) {
			GD.PrintErr($"Invalid scene argument in SwitchScenes! {newScene}");
			return;
		}
		
		if (CurrentScene != null) CurrentScene.QueueFree();
		var newLoadedScene = packedScenes[newScene].Instantiate();
		AddChild(newLoadedScene);

		CurrentScene = newLoadedScene;

		if (nodeToTeleportTo != null) {
			var node = newLoadedScene.GetNode<Node2D>(nodeToTeleportTo);
			Player.Position = node.Position;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		gameTick(delta);
	}

	private void meltdown() {
		GD.Print("Meltdown triggered");
		EndGame();
	}

	// Called in _Process as a high level game loop function
	private void gameTick(double delta) {
		reactor.Degrade(delta);
		double energyGenerated = reactor.GameTick(delta);
		TotalEnergyGenerated += energyGenerated;

		// Set label values
		debugLabels.UpdateLabel("ReactorCoreTempLabel", $"Reactor Core Temperature: {reactor.CoreTemperature - 273.15:F4} Degrees Celsius");		
		debugLabels.UpdateLabel("CoolantTempLabel", $"Coolant Temperature: {reactor.coolant.Temperature - 273.15:F4} Degrees Celsius");
		debugLabels.UpdateLabel("WattageLabel", $"Current Generation: {energyGenerated/delta/1000000:F3} Megawatts");
		debugLabels.UpdateLabel("FuelLevelLabel", $"Fuel Level: {reactor.FuelFreshness:P}");
		debugLabels.UpdateLabel("ControlRodDepthLabel", $"Control Rod Depth: {reactor.ControlRodDepth:P}");
	}

}
