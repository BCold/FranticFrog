using Godot;
using System;

public partial class MovingObstacle : Area2D
{
    [Export]
    public float speed = 10;
    [Export]
    protected bool moveLeft = true;
    protected Vector2 point0 = new Vector2(x: 1056, y: 32);
    protected Vector2 point1 = new Vector2(x: -288, y: 32);

    public Vector2 velocity;

    protected Sprite2D sprite;

    public override void _Ready()
    {
        if (!IsInGroup("Safezone") & !IsInGroup("Endzone") & !IsInGroup("AnimatedObstacle")){
            sprite = GetNode<Sprite2D>("Sprite2D");
        }
        else if (IsInGroup("AnimatedxObstacle")){
            AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("Sprite2D");
        }
    }

    public override void _PhysicsProcess(double delta)
    {   
        if (moveLeft){
            velocity = new Vector2(x: (float)(GlobalPosition.X - speed * delta), y: GlobalPosition.Y);

            if (GlobalPosition.X >= point1.X){
                GlobalPosition = velocity;
            }
            else if (GlobalPosition.X <= point1.X){
                GlobalPosition = new Vector2(x: point0.X, y: GlobalPosition.Y);
            }
        }
        else if (moveLeft == false){
            sprite.FlipH = true;
            velocity = new Vector2(x: (float)(GlobalPosition.X + speed * delta), y: GlobalPosition.Y);

            if (GlobalPosition.X <= point0.X){
                GlobalPosition = velocity;
            }
            else if (GlobalPosition.X >= point0.X){
                GlobalPosition = new Vector2(x: point1.X, y: GlobalPosition.Y);
            }
        }
        
    }
}
