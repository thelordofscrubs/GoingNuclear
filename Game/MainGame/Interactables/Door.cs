using Godot;
using System;

public partial class Door : InteractableObject
{
    [Export]
    public string Destination;    
    [Export]
    public string DestinationNode;            

    protected override void OnInteractionAttempted()
    {
		  mainGame.SwitchScenes(Destination, DestinationNode);
    }

}

