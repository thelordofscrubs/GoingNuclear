using System;
using System.Collections.Generic;
using System.Linq;
using GameObjects;
using Godot;


public class GameStatus {
    public int CurrentDay = 0;
    public double Money = 1000;
	
    public List<FuelCell> FuelCells = new List<FuelCell>();
    public List<FuelCell> CurrentlyActiveFuelCells {get => FuelCells.Where( fc => fc.CurrentlyInserted ).ToList();}
    public Dictionary<ItemType, GameResource> Resources = new Dictionary<ItemType, GameResource>();
    public Dictionary<ItemType, ShopItem> GetShopItems() {
        Dictionary<ItemType, ShopItem> items = new Dictionary<ItemType, ShopItem>();
        foreach(ShopItem item in Resources.Values.OfType<ShopItem>()) {
            items.Add(item.Type, item);
        }
        return items;
    }

    public double MegaWattHoursGeneratedToday = 0;
    public double TotalMegaWattHoursGenerated = 0;
    public double MegaWattHourRequirementToday = 1000;
    public double BaseMegaWattHourRequirement = 1000;
    public double TimeLeftInDay = 4 * 60;
    public double TimePerDay = 4 * 60;
    public double TimeSavedTotal = 0;
    public double GameScore {get => TotalMegaWattHoursGenerated + TimeSavedTotal * 5;}
    public RandomEventsDaily CurrentRandomDaily = RandomEventsDaily.NoEvent;
    
    public bool SpendMoney(double amount) {
        if (amount > Money) return false;
        Money -= amount;
        return true; 
    }

    public void CreateShopItem(ShopItem item) {
        Resources.Add(item.Type, item);
    }

    public GameStatus() {
        CreateShopItems();
    }
    private void CreateShopItems() {
		CreateShopItem( new ShopItem(ItemType.FuelCell, "Fuel Cell", 20f, 30));
		CreateShopItem( new ShopItem(ItemType.Forklift, "Forklift", 300f, 1));
		CreateShopItem( new ShopItem(ItemType.ControlRoomEngineer, "Control Room Engineer", 100f, 5));
		CreateShopItem( new ShopItem(ItemType.TurbineMechanic, "Turbine Mechanic", 100f, 5));
		CreateShopItem( new ShopItem(ItemType.ReactorEngineer, "Reactor Engineer", 100f, 5));
	}
}