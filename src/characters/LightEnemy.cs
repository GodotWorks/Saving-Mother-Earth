using Godot;
using System;

public class LightEnemy : KinematicBody2D
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

    private float attackSpeed;
    [Export] public float AttackSpeed
    {
        get { return attackSpeed; }
        set { attackSpeed = value; }
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
    private LightEnemyAttack lightEnemyAttack;

    public override void _Ready()
    {
        // GOAL: Find the nearest objective and kill it, otherwise patrol (basic AI)
        // Update detection Radius

        // References
        detectionRange = GetNode<Area2D>("DetectionRange");
        lightEnemyAttack = GetNode<LightEnemyAttack>("LightEnemyAttack");

        // Update Detection Radius
        detectionShape = detectionRange.GetChild<CollisionShape2D>(0);
        CircleShape2D shape = (CircleShape2D)detectionShape.Shape;
        shape.Radius = detectionRadius;

        // Signals
        detectionRange.Connect("body_entered", this, nameof(OnEnteredDetection));
        lightEnemyAttack.Connect(nameof(LightEnemyAttack.PlayerEnteredRegion), this, nameof(OnEnteredAttackRange));
        lightEnemyAttack.Connect(nameof(LightEnemyAttack.PlayerExitedRegion), this, nameof(OnExittedAttackRange));
        lightEnemyAttack.Connect(nameof(LightEnemyAttack.PlayerHit), this, nameof(OnHitPlayer));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (player != null) {
            // Move Towards Player
            Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
            MoveAndSlide(direction * moveSpeed);
            lightEnemyAttack.UpdateDirection(player.GlobalPosition);
        }
    }

    private void OnEnteredDetection(Node bodyEntered)
    {  
        GD.Print("Player Entered Detection Radius!");
        player = (Node2D)bodyEntered;
    }

    private void OnExitedDetection(Node bodyExited)
    {
        GD.Print("Player Exitted Detection Radius!");
        player = null;
    }

    private void OnEnteredAttackRange() {
        lightEnemyAttack.StartAttack(attackSpeed, player.GlobalPosition);
    }

    private void OnExittedAttackRange() {
        GD.Print("Exitted");
        lightEnemyAttack.StopAttack();
    }

    private void UpdateDetectionRadius()
    {
        if (detectionShape == null)
            return;

        CircleShape2D shape = (CircleShape2D)detectionShape.Shape;
        shape.Radius = detectionRadius;
    }

    private void OnHitPlayer() {
        GD.Print("Hit Player!");
        Player p = (Player) player;
        p.TakeDamage(attackDamage);
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
        if (health <= 0) {
            QueueFree();
        }
    }

}
