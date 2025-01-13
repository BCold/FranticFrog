using Godot;
using System;

public partial class level_0 : Node
{
    [Export]
    Node2D path0StartPos;

    [Export]
    Node2D path0EndPos;

    Vector2 resetPos;

    public override void _Ready()
    {
        resetPos = path0StartPos.Position;
    }

    public override void _Process(double delta)
    {
        path0StartPos.Position = new Vector2(x: (float)(path0StartPos.Position.X - 100 * delta), path0StartPos.Position.Y);
        if (path0StartPos.Position <= path0EndPos.Position){
            path0StartPos.Position = resetPos;
        }
    }


}
