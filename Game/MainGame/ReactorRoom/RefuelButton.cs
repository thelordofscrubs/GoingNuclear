using Godot;
using System;

public partial class RefuelButton : InteractableObject
{
    protected override void OnInteractionAttempted()
    {
        reactor.ChangeFuelFreshness(0.1);
    }
}
