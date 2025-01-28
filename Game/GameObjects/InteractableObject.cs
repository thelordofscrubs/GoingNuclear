using Godot;

public partial class InteractableObject : Node2D
{
    private bool _playerInRange = false;

    public override void _Ready()
    {
        Area2D childArea = GetNode<Area2D>("Area2D");
        childArea.AreaEntered += OnBodyEntered;
        childArea.AreaExited += OnBodyExited;
    }

    private void OnBodyEntered(Node body)
    {
        if (body.Name == "PlayerCollision")
        {
            GD.Print("Player Entered");
            Player player = body.GetParent<Player>();
            _playerInRange = true;
            player.InteractionAttempted += OnInteractionAttempted;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body.Name == "PlayerCollision")
        {
            Player player = body.GetParent<Player>();
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
