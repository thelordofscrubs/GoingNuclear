using GameObjects;
using Godot;
using System;

public partial class FixTurbines : InteractableObject
{
    protected override void OnInteractionAttempted()
    {
        GD.Print("Fixing Turbines!");
		// Increase turbine repair level by 10%
		reactor.turbineBay.ChangeRepairLevel(0.1);	
    }
}
