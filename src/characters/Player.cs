using Godot;
using System;

public class Player : KinematicBody2D
{
    // Stats
    private int speed;
    [Export]
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private int health;
    [Export]
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            EmitSignal(nameof(OnPlayerHPChanged), value);
        }
    }

    private int attackDamage;
    [Export]
    public int AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }

    private float attackSpeed;
    [Export]
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set { attackSpeed = value; }
    }

    // States
    private bool canShoot = true;
    private Vector2 velocity = new Vector2();

    // Resources
    [Export(PropertyHint.File, "*.tscn")]
    private string fireBallSceneFile;
    private PackedScene fireBallScene;

    // References
    private AnimatedSprite animatedSprite;
    private Timer attackSpeedTimer;

    // Signals
    [Signal]
    public delegate void OnPlayerHPChanged(int currentHP);

    public override void _Ready()
    {
        // References
        animatedSprite = GetNode<AnimatedSprite>(new NodePath("AnimatedSprite"));
        fireBallScene = GD.Load<PackedScene>(fireBallSceneFile);
        attackSpeedTimer = GetNode<Timer>("AttackSpeedTimer");
    }

    public override void _Process(float delta)
    {
        GetInput();
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = MoveAndSlide(velocity * speed);
    }

    private void GetInput()
    {
        velocity = new Vector2();

        if (Input.IsActionPressed("right"))
        {
            animatedSprite.FlipH = false;
            velocity.x += 1;
        }

        if (Input.IsActionPressed("left"))
        {
            animatedSprite.FlipH = true;
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("down"))
            velocity.y += 1;

        if (Input.IsActionPressed("up"))
            velocity.y -= 1;

        if (Input.IsActionPressed("shoot"))
            Shoot();

        velocity = velocity.Normalized();
    }

    private void Shoot()
    {
        if (canShoot)
        {
            Vector2 direction = GetGlobalMousePosition() - GlobalPosition;
            direction = direction.Normalized();

            Fireball spawnedFireBall = (Fireball)fireBallScene.Instance();
            GetTree().Root.AddChild(spawnedFireBall);
            spawnedFireBall.GlobalPosition = GlobalPosition;
            spawnedFireBall.Launch(direction);

            canShoot = false;
            attackSpeedTimer.Start(attackSpeed);
        }
    }

    private void OnAttackSpeedTimerTimeout()
    {
        canShoot = true;
    }

    public void TakeDamage(int dmg)
    {
        Health -= dmg;
        if (health <= 0)
        {
            // GD.Print("Player Died!");
        }
    }
}
