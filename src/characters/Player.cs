using Godot;
using System;

public class Player : KinematicBody2D
{
    private int speed;
    [Export] public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    [Export(PropertyHint.File, "*.tscn")]
    private string fireBallSceneFile;
    private PackedScene fireBallScene;

    // References
    private AnimatedSprite animatedSprite;    

    private Vector2 velocity = new Vector2();

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>(new NodePath("AnimatedSprite"));
        fireBallScene = GD.Load<PackedScene>(fireBallSceneFile);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetInput();
        velocity = MoveAndSlide(velocity);
    }

    private void GetInput() {
        velocity = new Vector2();

        if (Input.IsActionPressed("right"))
        {
            animatedSprite.FlipH = false;
            velocity.x += 1;
        }
            

        if (Input.IsActionPressed("left")) {
            animatedSprite.FlipH = true;
            velocity.x -= 1;
        }
            
        if (Input.IsActionPressed("down"))
            velocity.y += 1;

        if (Input.IsActionPressed("up"))
            velocity.y -= 1;
        
        if (Input.IsActionJustReleased("shoot"))
            Shoot();

        velocity = velocity.Normalized() * speed;
    }

    private void Shoot() {
        Vector2 direction = GetGlobalMousePosition() - GlobalPosition;
        direction = direction.Normalized();

        Fireball spawnedFireBall = (Fireball) fireBallScene.Instance();
        GetTree().Root.AddChild(spawnedFireBall);
        spawnedFireBall.GlobalPosition = GlobalPosition;
        spawnedFireBall.Launch(direction);
    }
}
