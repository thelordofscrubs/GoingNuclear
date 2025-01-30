using GameObjects;
using Godot;

public partial class InteractableObject : CollisionShape2D
{
    private bool _playerInRange = false;
    protected NuclearReactor reactor;
    protected MainGame mainGame;

    public override void _Ready()
    {
        Area2D childArea = GetNode<Area2D>("AreaNode");
        childArea.AreaEntered += OnBodyEntered;
        childArea.AreaExited += OnBodyExited;
        mainGame = GetNode<MainGame>("../..");
        reactor = mainGame.Reactor;
    }

    private void OnBodyEntered(Node body)
    {
        if (body.Name == "PlayerCollision")
        {
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
