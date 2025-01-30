using Godot;
using System;
using GameObjects;
public partial class RetractRodsButton : InteractableObject
{
    protected override void OnInteractionAttempted()
    {
        
		if (reactor.ControlRodState == -1) {
			reactor.ControlRodState = 0;
			return;
		}
		if (reactor.ControlRodState == 0 || reactor.ControlRodState == 1) {
            GD.Print("Retracting Rods!");
			reactor.ControlRodState = -1;
			return;
		}
    }
}
