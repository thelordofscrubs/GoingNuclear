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

	public Queue<double> Last5EnergyTicks = new Queue<double>();

	public double DisplayWattage() {
		if (Last5EnergyTicks.Count == 0) return 0;
		return Last5EnergyTicks.Sum()/Last5EnergyTicks.Count; 
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		reactor = new NuclearReactor(meltdown);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		gameLoop(delta);
	}

	private void meltdown() {
		GD.Print("Meltdown triggered");
		QueueFree();
	}

	// Called in _Process as a high level game loop function
	private void gameLoop(double delta) {
		reactor.Degrade(delta);
		double energyGenerated = reactor.GameTick(delta);
		TotalEnergyGenerated += energyGenerated;
		Last5EnergyTicks.Enqueue(energyGenerated);
		if (Last5EnergyTicks.Count > 5) {
			Last5EnergyTicks.Dequeue();
		}
	}



}
