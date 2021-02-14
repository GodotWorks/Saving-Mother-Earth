using Godot;
using System;

public class Light_Enemy : KinematicBody2D
{
    private int moveSpeed;
    [Export]
    public int MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }


    // Stats
    private int health;
    [Export]
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    private int attackDamage;
    [Export]
    public int AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }

    private int detectionRadius;
    [Export]
    public int DetectionRadius
    {
        get { return detectionRadius; }
        set
        {
            detectionRadius = value;
            UpdateDetectionRadius();
        }
    }

    // References
    private Area2D detectionRange;
    private CollisionShape2D detectionShape;
    private Node2D player;

    public override void _Ready()
    {
        // Find the nearest objective and kill it, otherwise patrol (basic AI)
        // Update detection Radius
        detectionRange = GetNode<Area2D>("DetectionRange");
        GD.Print(detectionRange);
        detectionShape = detectionRange.GetChild<CollisionShape2D>(0);
        CircleShape2D shape = (CircleShape2D)detectionShape.Shape;
        shape.Radius = detectionRadius;

        detectionRange.Connect("body_entered", this, "OnEnteredDetection");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (player != null) {
            Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
            MoveAndSlide(direction * moveSpeed);
        }
    }

    private void OnEnteredDetection(Node bodyEntered)
    {
        GD.Print("Body Entered " + bodyEntered.Name);
        player = (Node2D)bodyEntered;
    }

    private void OnExitedDetection(Node bodyExited)
    {
        GD.Print("Lost track of player ");
        player = null;
    }

    private void UpdateDetectionRadius()
    {
        if (detectionShape == null)
            return;

        CircleShape2D shape = (CircleShape2D)detectionShape.Shape;
        shape.Radius = detectionRadius;
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
        if (health <= 0) {
            QueueFree();
        }
    }

}
