using Godot;

public partial class DoorArea : Area2D
{
    [Export] public Vector2 TargetPosition; // The position to teleport the player to
    [Export] public NodePath PlayerPath; // Path to the player node
    [Export] public string InteractionKey = "ui_accept"; // Key for interaction (e.g., "Enter")

    private bool _playerInArea = false; // Tracks if the player is in the door area
    private Node2D _player;

    public override void _Ready()
    {
        // Resolve the player node from the path
        if (!string.IsNullOrEmpty(PlayerPath))
        {
            _player = GetNode<Node2D>(PlayerPath);
        }
    }

    public override void _Process(double delta)
    {
        // Check if the player is in the area and presses the interaction key
        if (_playerInArea && Input.IsActionJustPressed(InteractionKey))
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        if (_player != null)
        {
            _player.GlobalPosition = TargetPosition;
            GD.Print("Player teleported to: ", TargetPosition);
        }
    }

    // Signal handler for when a body enters the door area
    private void OnBodyEntered(Node body)
    {
        if (body == _player)
        {
            _playerInArea = true;
            GD.Print("Player entered the door area.");
        }
    }

    // Signal handler for when a body exits the door area
    private void OnBodyExited(Node body)
    {
        if (body == _player)
        {
            _playerInArea = false;
            GD.Print("Player left the door area.");
        }
    }
}
