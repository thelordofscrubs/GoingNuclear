using Godot;
using System;
using System.Collections.Generic;

public partial class DayStart : CanvasLayer
{
	private GameStatus gameStatus;	
	private GameController gameController;
	private Dictionary<ItemType, ShopItem> shopItems;
	private Label moneyLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameController = GetParent<GameController>();
		gameStatus = gameController.GameStats;
		shopItems = gameStatus.GetShopItems();
		
		moneyLabel = GetNode<Label>("ShopInfoContainer/MoneyLabel");
		updateMoneyLabel();

		GetNode<Button>("EndPhaseButton").Pressed += gameController.AdvanceGamePhase;

		setPrices();
		createShopUI();
		
	}

	private void updateMoneyLabel() {
		moneyLabel.Text = $"Money: {gameStatus.Money:F2}";
	}

	private void setPrices() {
		foreach(ShopItem item in shopItems.Values) {
			item.MultiplyPrice((float)getRandomPriceMultiplier());
		}
		switch (gameStatus.CurrentRandomDaily) {
			case RandomEventsDaily.CheapFuel:
				shopItems[ItemType.FuelCell].MultiplyPrice(0.8f);			
				break;
			case RandomEventsDaily.ExpensiveFuel:
				shopItems[ItemType.FuelCell].MultiplyPrice(1.2f);
				break;
			case RandomEventsDaily.CheapHires:
				shopItems[ItemType.ControlRoomEngineer].MultiplyPrice(0.8f);
				shopItems[ItemType.TurbineMechanic].MultiplyPrice(0.8f);
				shopItems[ItemType.ReactorEngineer].MultiplyPrice(0.8f);
				break;
			case RandomEventsDaily.ExpensiveHires:
				shopItems[ItemType.ControlRoomEngineer].MultiplyPrice(1.2f);
				shopItems[ItemType.TurbineMechanic].MultiplyPrice(1.2f);
				shopItems[ItemType.ReactorEngineer].MultiplyPrice(1.2f);
				break;
		}
	}

	private double getRandomPriceMultiplier() {
		var rand = new Random();
		double multAdder = rand.NextDouble() / 4;
		if (rand.NextDouble() < 0.5) {
			multAdder *= -1;
		}
		return 1 + multAdder;
	}

	private void createShopUI() {
		var packedItemScene = GD.Load<PackedScene>("res://Game/DayStart/ShopItem.tscn");
		var shopUiParent = GetNode<HFlowContainer>("ShopUiContainer");
		foreach(ShopItem item in shopItems.Values) {
			var loadedScene = packedItemScene.Instantiate<Control>();
			shopUiParent.AddChild(loadedScene);
			
			item.DisplayLabel = loadedScene.GetNode<Label>("VBox/ShopItemTitleAndPrice");			
			Button button = loadedScene.GetNode<Button>("VBox/BuyButton");
			button.Pressed += () => {PurchaseItemButtonPressed(item.Type);};
			button.Text = "Purchase";
			item.purchaseButton = button;
			Label playerOwnedLabel = loadedScene.GetNode<Label>("VBox/CurrentItemCount");
			item.playerOwnedCountLabel = playerOwnedLabel;
			item.SetLabelContents();
		}
	}

	private void PurchaseItemButtonPressed(ItemType type) {
		if (shopItems[type].QuantityAvailable <= 0) return;
		if (gameStatus.SpendMoney(shopItems[type].RealPrice) == false) return;

		shopItems[type].PurchaseItem();		
		updateMoneyLabel();
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("dev_mode")) {
			GD.Print("Dev key pressed, granting moolah");
			gameStatus.Money += 10000;
			updateMoneyLabel();
			foreach(var item in shopItems.Values) {
				item.ChangeAvailableQuantity(100);
				item.SetLabelContents();
			}			
		}
    }


}
