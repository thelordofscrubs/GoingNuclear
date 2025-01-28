using Godot;

public partial class TurbineRoomDoor : InteractableObject
{

    MainGame mainGame;
    public override void _Ready() {
        base._Ready();
        mainGame = GetNode<MainGame>("../..");
    }

    protected override void OnInteractionAttempted()
    {
        GD.Print("Interaction attempted on Turbine Room door!");
        mainGame.SwitchScenes("TurbineRoom", "ControlRoomDoor");
    }
}
