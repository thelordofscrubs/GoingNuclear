using Godot;
using System;

public partial class Door : InteractableObject
{
    [Export]
    public string Destination;    
    [Export]
    public string DestinationNode;    
    CollisionShape2D collisionShape;
    InteractableObject interactableObject;
    
    public override void _Ready() {
        
        base._Ready();
        collisionShape = GetNode<CollisionShape2D>("AreaNode/ShapeNode");
        collisionShape.Shape = Shape;
        
    }

    protected override void OnInteractionAttempted()
    {
		  mainGame.SwitchScenes(Destination, DestinationNode);
    }

}

