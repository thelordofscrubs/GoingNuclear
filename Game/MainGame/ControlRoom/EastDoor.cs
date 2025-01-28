using Godot;

public partial class DoorArea : InteractableObject
{

    MainGame mainGame;
    public override void _Ready() {
        base._Ready();
        mainGame = GetNode<MainGame>("../..");
        GD.Print("Main game node name from east door: "+mainGame.Name);
    }

    protected override void OnInteractionAttempted()
    {
        GD.Print("Interaction attempted on east door!");
    }


}
