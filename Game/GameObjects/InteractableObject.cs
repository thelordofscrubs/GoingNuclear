using Godot;

public partial class InteractableObject : Node2D
{
    private bool _playerInRange = false;

    public override void _Ready()
    {
        GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
        GetNode<Area2D>("Area2D").BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            _playerInRange = true;
            player.InteractionAttempted += OnInteractionAttempted;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Player player)
        {
            _playerInRange = false;
            player.InteractionAttempted -= OnInteractionAttempted;
        }
    }

    protected virtual void OnInteractionAttempted()
    {
        if (_playerInRange)
        {
            GD.Print("Default interaction triggered!");
        }
    }
}
