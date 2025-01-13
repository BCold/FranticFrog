
using Godot;

public partial class Platform : MovingObstacle
{

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if(moveLeft == false){
            sprite.FlipH = false;
        }
    }
}
