using Godot;


public partial class Player : Area2D
{
    [Signal]
    public delegate void dieEventHandler();
    [Signal]
    public delegate void scoreEventHandler();
    [Signal]
    public delegate void LevelClearEventHandler();
    [Export]
    public  int maxLives = 6;
    [Export]
    public float beginWater = 0;

    private int moveMagnitude = 64;
    private bool colliding = false;
    Vector2 moveDirection;
    AnimatedSprite2D animatedSprite2D;
    Node parent;
    private int _lives;

    public int Lives{
        get => _lives;
        set{
            _lives = value;
        }
    }

    private int _endzones = 5;

    public int Endzones{
        get => _endzones;
        set{
            _endzones = value;
        }
    }

    public override void _Ready()
    {   
        Lives = maxLives;
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        parent = GetParent();
    }

    public override void _PhysicsProcess(double delta)
    {   
        moveDirection = Vector2.Zero;
        GetInput();
        MovePlayer();

        if (GlobalPosition.Y > 864){
            GlobalPosition = new Vector2(x:GlobalPosition.X, y: 864);
        }

        if (GlobalPosition.Y < beginWater & colliding != true){
            EmitSignal("die");
        }

        if (GlobalPosition.Y < 96){
            GlobalPosition = new Vector2(x:GlobalPosition.X, y: 96);
        }


        
    }

    // Assign Movement Direction
    public void GetInput(){
        // Assign Movement Direction
        if (Input.IsActionJustPressed("move_right")){
			moveDirection.X += 1;
		}

		if (Input.IsActionJustPressed("move_left")){
			moveDirection.X -= 1;
		}

		if (Input.IsActionJustPressed("move_up")){
			moveDirection.Y -= 1;
		}

		if (Input.IsActionJustPressed("move_down")){
			moveDirection.Y += 1;
		}
    }
    
    // Move the player based on assigned movement direction values
    public void MovePlayer(){
        // Move Up
        if (moveDirection.Y < 0 && moveDirection.X == 0){
            animatedSprite2D.RotationDegrees = 0;
            animatedSprite2D.FlipV = false;
            animatedSprite2D.Play("jump");
            Position = new Vector2(x: Position.X, Position.Y + -moveMagnitude);
        }
        // Move Down
        if (moveDirection.Y > 0 && moveDirection.X == 0){
            animatedSprite2D.RotationDegrees = 0;
            animatedSprite2D.FlipV = true;
            animatedSprite2D.Play("jump");
            Position = new Vector2(x: Position.X, Position.Y + moveMagnitude);
        }
        //Move Left
        if (moveDirection.X < 0 && moveDirection.Y == 0){
            if (animatedSprite2D.FlipV){
                animatedSprite2D.RotationDegrees = 90;
            }
            else{
                animatedSprite2D.RotationDegrees = -90;
            }

            animatedSprite2D.Play("jump");
            Position = new Vector2(x: Position.X + -moveMagnitude, Position.Y);

        }
        // Move Right
        if (moveDirection.X > 0 && moveDirection.Y == 0){
            if (animatedSprite2D.FlipV){
                animatedSprite2D.RotationDegrees = -90;
            }
            else{
                animatedSprite2D.RotationDegrees = 90;
            }
            animatedSprite2D.Play("jump");
            Position = new Vector2(x: Position.X + moveMagnitude, Position.Y);
        }
    }

    private void OnAreaEntered(Area2D area){
        if (area is Platform){
            if (area.IsInGroup("Endzone")){
            AnimatedSprite2D endSprite = area.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
                if (area.Monitorable){
                    endSprite.Play("change");
                }
                area.CallDeferred("set_monitorable", false);
                Endzones -= 1;
                EmitSignal("score");
                if (Endzones == 0){
                    EmitSignal("LevelClear");
                }
            }
            colliding = true;
            // CallDeferred("reparent", area);
            Reparent(area);
        }
        else if (area is Vehicle){
            EmitSignal("die");
        }
        // else if (area.IsInGroup("Endzone")){
        //     AnimatedSprite2D endSprite = area.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        //     if (area.Monitorable){
        //         endSprite.Play("change");
        //     }
        //     area.CallDeferred("set_monitorable", false);
        //     EmitSignal("score");
        // }
	}

    private void OnAreaExit(Area2D area){
        if (area is Platform){
            colliding = false;
            // CallDeferred("reparent", parent);
            Reparent(parent);
        }
        else if (area.IsInGroup("ScoreArea") & GlobalPosition.Y < area.GlobalPosition.Y){
            area.CallDeferred("set_monitorable", false);
            EmitSignal("score");
        }
	}

    private void GameOver(){

    }
}
