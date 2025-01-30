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

	public double TotalEnergyGenerated = 0;

	public Queue<double> Last5EnergyTicks = new Queue<double>( new double[10]);

	public Dictionary<string, PackedScene> packedScenes = new Dictionary<string, PackedScene>();
    public Dictionary<string, PackedScene> packedRoomScenes = new Dictionary<string, PackedScene>();

	public DebugLabelManager debugLabels;

	public Button RaiseControlRodsButton;
	public Button LowerControlRodsButton;	

	public Node CurrentScene;
	public Player PlayerRef;
	public bool MeltdownOccured = false;

	public double DisplayWattage() {
		if (Last5EnergyTicks.Count == 0) return 0;
		return Last5EnergyTicks.Sum()/Last5EnergyTicks.Count; 
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		
		
		debugLabels = new DebugLabelManager(GetNode("DebugLabels"));

		GD.Print("Main Game script is ready");
		
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
		gameTick(delta);
	}

	private void meltdown() {
		MeltdownOccured = true;
	}

	// Called in _Process as a high level game loop function
	private void gameTick(double delta) {
		Reactor.Degrade(delta);
		double energyGenerated = Reactor.GameTick(delta);
		TotalEnergyGenerated += energyGenerated;

		// Set label values
		debugLabels.UpdateLabel("ReactorCoreTempLabel", $"Reactor Core Temperature: {Reactor.CoreTemperature - 273.15:F4} Degrees Celsius");		
		debugLabels.UpdateLabel("CoolantTempLabel", $"Coolant Temperature: {Reactor.coolant.Temperature - 273.15:F4} Degrees Celsius");
		debugLabels.UpdateLabel("WattageLabel", $"Current Generation: {energyGenerated/delta/1000000:F3} Megawatts");
		debugLabels.UpdateLabel("FuelLevelLabel", $"Fuel Level: {Reactor.FuelFreshness:P}");
		debugLabels.UpdateLabel("ControlRodDepthLabel", $"Control Rod Depth: {Reactor.ControlRodDepth:P}");
		debugLabels.UpdateLabel("TurbineRepairLabel", $"Turbine Repair Level: {Reactor.turbineBay.RepairLevel:P}");
		debugLabels.UpdateLabel("RunCooldownLabel", $"Run Cooldown: {PlayerRef.RunCooldown:F2}");
	}

}
