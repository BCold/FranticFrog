using Godot;

public partial class SandBox : Node
{
    [Export]
    public Label scoreLabel;
    [Export]
    public Label hiScoreLabel;
    [Export]
    public Label livesLabel;
    [Export]
    public Vector2 playerResetPos;
    Player player;

    private int _score;

    public int Score{
        get => _score;
        set{
            _score = value;
        }
    }

    public override void _Ready()
    {
        Globals.Instance.Load();
        Engine.TimeScale = 1;
        player = GetNode<Player>("Player");
        Score = 0;
        scoreLabel.Text = "Score\n" + Score;
        hiScoreLabel.Text = "Hi-Score\n" + Globals.Instance.HighScore;
        livesLabel.Text = "Lives\n" + player.Lives;

    }

    private void OnPlayerDied(){
        player.Lives -= 1;
        livesLabel.Text = "Lives\n" + player.Lives;
        if (player.Lives == 0){
            GetTree().ReloadCurrentScene();
            Globals.Instance.PrevHigh = Globals.Instance.HighScore;
            Globals.Instance.Save();
        }
        else{
            player.GlobalPosition = playerResetPos;
            GD.Print(player.Lives);
        }
    }

    private void OnPlayerScored(){
        if (player.GlobalPosition.Y <= 96){
            Score += 100;
            player.GlobalPosition = playerResetPos;
            Godot.Collections.Array<Node> scoreAreas = GetTree().GetNodesInGroup("ScoreArea");
            foreach ( Node node in scoreAreas){
                if (node is Area2D){
                    Area2D scoreArea = (Area2D)node;
                    scoreArea.CallDeferred("set_monitorable", true);
                }
            }
        }
        else{
            Score += 10;
        }
        scoreLabel.Text = "Score\n" + Score;
       
        if (Score > Globals.Instance.HighScore){
            Globals.Instance.HighScore = Score;
            if (Globals.Instance.HighScore > Globals.Instance.PrevHigh){
                Globals.Instance.PrevHigh = Globals.Instance.HighScore;
            }
            hiScoreLabel.Text = "Hi-Score\n" + Globals.Instance.HighScore;
        }

        Globals.Instance.Save();

    }

    private void OnLevelCleared(){
        Engine.TimeScale = 0;
    }

}
