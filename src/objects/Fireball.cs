using Godot;
using System;

public class Fireball : KinematicBody2D
{
    private Vector2 direction;
    private bool launch = false;
    
    private int speed = 10;
    [Export] public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (launch) {
            MoveAndSlide(direction * Speed);
        }
    }

    public void Launch(Vector2 direction) {
        this.direction = direction;
        launch = true;
    }
}
