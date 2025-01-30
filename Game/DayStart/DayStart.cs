using Godot;
using System;
using System.Collections.Generic;

public partial class DayStart : Control
{
	public Dictionary<string, Label> ShopLabels = new Dictionary<string, Label>();
	private GameStatus gameStatus;
	public enum ShopItems {
		FuelCell,
		ForkLift,
		ControlRoomEngineer,
		TurbineMechanic,
		ReactorEngineer
	}
	private readonly int shopItemCount = 5;
	private double[] baseShopPrices = {20, 200, 100, 100, 100};
	private double[] shopPrices = new double[5];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameStatus = GetParent<GameController>().GameStats;
		
		setPrices();
	}

	private void setPrices() {
		for(int i = 0; i < shopItemCount; i++) {
			shopPrices[i] = baseShopPrices[i] * getRandomPriceMultiplier();
		}
		switch (gameStatus.CurrentRandomDaily) {
			case RandomEventsDaily.CheapFuel:
				shopPrices[(int)ShopItems.FuelCell] *= 0.8;			
				break;
			case RandomEventsDaily.ExpensiveFuel:
				shopPrices[(int)ShopItems.FuelCell] *= 1.2;
				break;
			case RandomEventsDaily.CheapHires:
				shopPrices[(int)ShopItems.ControlRoomEngineer] *= 0.8;
				shopPrices[(int)ShopItems.TurbineMechanic] *= 0.8;
				shopPrices[(int)ShopItems.ReactorEngineer] *= 0.8;
				break;
			case RandomEventsDaily.ExpensiveHires:
				shopPrices[(int)ShopItems.ControlRoomEngineer] *= 1.2;
				shopPrices[(int)ShopItems.TurbineMechanic] *= 1.2;
				shopPrices[(int)ShopItems.ReactorEngineer] *= 1.2;
				break;
		}
	}

	private double getRandomPriceMultiplier() {
		var rand = new Random().NextDouble();
		double multAdder = rand * (1 / 4);
		if (rand < 0.5) {
			multAdder *= -1;
		}
		return 1 + multAdder;
	}

	private void createShopUI() {
		var packedItemScene = GD.Load("res://Game/DayStart/ShopItem.tscn");
		
	}

	private void findLabels() {
		var children = GetChildren();
		foreach (var child in children) {
			if (child.Name.ToString().StartsWith("ShopLabel") && child is Label) {
				ShopLabels.Add(child.Name.ToString(), (Label)child);
			}
		}
	}

	private void setLabel(string key, string value) {
		if (ShopLabels.ContainsKey(key)) {
			ShopLabels[key].Text = value;
		} else {
			GD.Print($"Attempted to set text on label missing from dictionary: {key}");
		}
	}

}
