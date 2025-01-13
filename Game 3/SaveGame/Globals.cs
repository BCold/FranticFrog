using Godot;

public partial class Globals : Node
{
    private static Globals _instance;
    public static Globals Instance => _instance;
    public int HighScore {get; set;} = 0;
    public int PrevHigh {get; set;} = 0;

    // Use _EnterTree to make sure the Singleton instance is available in _Ready()
    public override void _EnterTree()
    {
        if (_instance != null){
            this.QueueFree(); // The singleton is already loaded, kill this instance
        }
        
        _instance = this;
    }

    public void Save(){
        SaveGame saveGame = new();

        saveGame.HighScore = HighScore;
        saveGame.PrevHigh = PrevHigh;

        ResourceSaver.Save(saveGame, "user://savedGame.tres");
    }

    public void Load(){
        SaveGame saveGame = GD.Load("user://savedGame.tres") as SaveGame;

        HighScore = saveGame.HighScore;
        PrevHigh = saveGame.PrevHigh;

    }
}
