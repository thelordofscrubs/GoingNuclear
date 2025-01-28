using GameObjects;
using Godot;
using System;

public partial class FixTurbines : InteractableObject
{
    NuclearReactor reactor;

    public override void _Ready() {
        base._Ready();
        reactor = GetNode<MainGame>("../..").Reactor;
    }

    protected override void OnInteractionAttempted()
    {
        GD.Print("Fixing Turbines!");
		// Increase turbine repair level by 10%
		reactor.turbineBay.ChangeRepairLevel(0.1);	
    }
}
