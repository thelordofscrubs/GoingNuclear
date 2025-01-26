using Godot;
using System;
using GameObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class MainGame : Node2D
{
	NuclearReactor reactor;

	public double TotalEnergyGenerated = 0;

	public Queue<double> Last5EnergyTicks = new Queue<double>( new double[10]);

	public Label CoreTempLabel;
	public Label CoolantTempLabel;
	public Label WattageLabel;
	public Label FuelLevelLabel;
	public Label ControlRodDepthLabel;

	public Button RaiseControlRodsButton;
	public Button LowerControlRodsButton;

	public Action EndGame = () => {};

	public double DisplayWattage() {
		if (Last5EnergyTicks.Count == 0) return 0;
		return Last5EnergyTicks.Sum()/Last5EnergyTicks.Count; 
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		reactor = new NuclearReactor(meltdown);
		CoreTempLabel = GetNode<Label>("TempBackground/ReactorCoreTempLabel/DisplayValue");
		CoolantTempLabel = GetNode<Label>("TempBackground/CoolantTempLabel/DisplayValue");
		WattageLabel = GetNode<Label>("TempBackground/WattageLabel/DisplayValue");
		FuelLevelLabel = GetNode<Label>("TempBackground/FuelFreshnessLabel/DisplayValue");
		ControlRodDepthLabel = GetNode<Label>("TempBackground/ControlRodDepthLabel/DisplayValue");

		RaiseControlRodsButton = GetNode<Button>("TempBackground/RaiseControlRodsButton");
		RaiseControlRodsButton.ButtonDown += () => {reactor.ControlRodState = 1;};
		RaiseControlRodsButton.ButtonUp += () => {reactor.ControlRodState = 0;};
		LowerControlRodsButton = GetNode<Button>("TempBackground/LowerControlRodsButton");
		LowerControlRodsButton.ButtonDown += () => {reactor.ControlRodState = -1;};
		LowerControlRodsButton.ButtonUp += () => {reactor.ControlRodState = 0;};

		GD.Print("Main Game script is ready");
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
		CoreTempLabel.Text = $"{(reactor.CoreTemperature - 273.15):F4} Degrees Celsius";		
		CoolantTempLabel.Text = $"{(reactor.coolant.Temperature - 273.15):F4} Degrees Celsius";
		WattageLabel.Text = $"{energyGenerated/delta/1000000:F3} Megawatts";
		FuelLevelLabel.Text = $"{reactor.FuelFreshness:P}";
		ControlRodDepthLabel.Text = $"{reactor.ControlRodDepth:P}";
	}

}
