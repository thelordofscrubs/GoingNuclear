using System;
using Godot;
using System.Collections.Generic;

public class ShopItem : GameResource {
    public float BasePrice;
    public float RealPrice;
    public int QuantityAvailable;
    public string DisplayName;
    public ItemType Type;

    public Label DisplayLabel;    
    public Button purchaseButton;
    public Label playerOwnedCountLabel;

    public ShopItem(ItemType type, string displayName, float basePrice, int quantityAvailable) {
        Type = type;
        DisplayName = displayName;
        BasePrice = basePrice;
        RealPrice = basePrice;
        QuantityAvailable = quantityAvailable;
    }

    public void ChangeAvailableQuantity(int changeAmount) {
        QuantityAvailable = Math.Max(QuantityAvailable + changeAmount, 0);
        if (QuantityAvailable <= 0) {
            purchaseButton.Disabled = true;
            purchaseButton.Text = "Out of stock";
        } else {
            purchaseButton.Disabled = false;
            purchaseButton.Text = "Purchase";
        }
    }

    public void SetPriceWithMultipliers(List<float> multipliers, float? basePrice = null) {
        float newPrice;
        if (basePrice == null) {
            newPrice = this.BasePrice;
        } else {
            newPrice = (float)basePrice;
        }
        
        foreach(float multiplier in multipliers) {
            newPrice *= multiplier;
        }
        RealPrice = newPrice;
    }

    public void MultiplyPrice(float multiplier) {
        RealPrice *= multiplier;
    }

    public void PurchaseItem(int count = 1) {
        ChangeAvailableQuantity(-count);
        ChangeOwnedCount(count);
        MultiplyPrice(1.1f);
        SetLabelContents();
        
    }

    public void SetLabelContents() {
        DisplayLabel.Text = $"{DisplayName}s available: {QuantityAvailable:D}\nPrice: {RealPrice:F2}";
        playerOwnedCountLabel.Text = $"Amount owned: {OwnedCount}";
    }

}