using Godot;
using System;

public partial class DayEnd : CanvasLayer
{
	public GameController gameController;
    public Button AdvanceGameButton;
    public Label ScoreLabel;
    public Label ResultLabel;
    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        gameController = GetParent<GameController>();
        AdvanceGameButton = GetNode<Button>("VBoxContainer/NextDayButton");
        ScoreLabel = GetNode<Label>("VBoxContainer/ScoreLabel");
        ResultLabel = GetNode<Label>("VBoxContainer/ResultLabel");

        ScoreLabel.Text = $"Accumulated Score: {gameController.GameStats.GameScore:F0}";

        switch (gameController.dayResult) {            
            case DayResult.Success:
                ResultLabel.Text = $"Day {gameController.GameStats.CurrentDay + 1} Finished";
                AdvanceGameButton.Pressed += gameController.AdvanceGamePhase;
                break;
            case DayResult.Meltdown:
                ResultLabel.Text = "You pushed the reactor too hard and caused a meltdown!";
                AdvanceGameButton.Pressed += gameController.EndGame;
                AdvanceGameButton.Text = "Quit to Main Menu";
                break;
            case DayResult.OutOfTime:
                ResultLabel.Text = "You didn't meet the electricity requirement and have been deemed unfit for this job.";
                AdvanceGameButton.Pressed += gameController.EndGame;
                AdvanceGameButton.Text = "Quit to Main Menu";
                break;
        }
        
	}
}
