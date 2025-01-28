using Godot;
using System;
using GameObjects;
public partial class InsertRodsButton : InteractableObject
{
    NuclearReactor reactor;

    public override void _Ready() {
        base._Ready();
        reactor = GetNode<MainGame>("../..").Reactor;
		GD.Print("insert rods button ready");
    }

    protected override void OnInteractionAttempted()
    {
        GD.Print("Inserting Rods!");
		if (reactor.ControlRodState == 1) {
			reactor.ControlRodState = 0;
			return;
		}
		if (reactor.ControlRodState == 0 || reactor.ControlRodState == -1) {
			reactor.ControlRodState = 1;
			return;
		}
    }
}
