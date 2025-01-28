using Godot;
using System;
using GameObjects;
public partial class RetractRodsButton : InteractableObject
{
    NuclearReactor reactor;

    public override void _Ready() {
        base._Ready();
        reactor = GetNode<MainGame>("../..").Reactor;
    }

    protected override void OnInteractionAttempted()
    {
        GD.Print("Retracting Rods!");
		if (reactor.ControlRodState == -1) {
			reactor.ControlRodState = 0;
			return;
		}
		if (reactor.ControlRodState == 0 || reactor.ControlRodState == 1) {
			reactor.ControlRodState = -1;
			return;
		}
    }
}
