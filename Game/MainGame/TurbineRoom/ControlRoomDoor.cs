using Godot;
using System;

public partial class ControlRoomDoor : InteractableObject
{
    MainGame mainGame;
    public override void _Ready() {
        base._Ready();
        mainGame = GetNode<MainGame>("../..");
    }

    protected override void OnInteractionAttempted()
    {
        GD.Print("Interaction attempted on Control Room door!");
		mainGame.SwitchScenes("ControlRoom", "TurbineRoomDoor");
    }
}
