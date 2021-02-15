using Godot;
using System;

public class LightEnemyAttack : Area2D
{
    // Attack
    private Timer attackTimer;
    private Timer cooldownTimer;
    private float timeInterval;
    private float timeReset = 0.1f;
    private bool hit = false; // Prevent double hits
    private bool attacking = false;


    // References
    private AnimatedSprite animatedSprite;
    private Vector2 direction;
    private CollisionShape2D attackShape;
    private Area2D attackRegion;

    [Signal]
    public delegate void PlayerEnteredRegion();
    [Signal]
    public delegate void PlayerExitedRegion();
    [Signal]
    public delegate void PlayerHit();

    public override void _Ready()
    {
        // References
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        attackShape = GetNode<CollisionShape2D>("CollisionShape2D");
        attackTimer = GetNode<Timer>("AttackTimer");
        cooldownTimer = GetNode<Timer>("CooldownTimer");
        attackRegion = GetNode<Area2D>("AttackRegion");

        attackShape.Disabled = true;
    }

    public override void _PhysicsProcess(float delta)
    {
        CheckForBodies();
    }

    private void CheckForBodies() {
        if (attackRegion.GetOverlappingBodies().Count <= 0 && attacking) {
            EmitSignal(nameof(PlayerExitedRegion));
        }
    }

    private void NextFrame()
    {
        animatedSprite.Frame++;
    }

    // Time To complete animation (attackspeed)
    public void StartAttack(float timeToComplete, Vector2 target)
    {
        attacking = true;
        timeInterval = timeToComplete / animatedSprite.Frames.GetFrameCount("default");
        attackTimer.Start(timeInterval);
        UpdateDirection(target);
    }

    public void UpdateDirection(Vector2 target)
    {
        this.direction = target - GlobalPosition;
        LookAt(target);
        RotationDegrees += 90;
    }

    // Occurs when timer timesout
    private void OnAttackTimerTimeout()
    {
        if (animatedSprite.Frame >= (animatedSprite.Frames.GetFrameCount("default") - 1))
        {
            // Reached end of animation
            attackShape.Disabled = false;
            cooldownTimer.Start(timeReset);
        }
        else
        {
            attackTimer.Start(timeInterval);
            NextFrame();
        }
    }

    private void OnCooldownTimerTimeout()
    {
        StopAttack();
        attackTimer.Start(timeInterval);
        attacking = true;
    }

    public void StopAttack()
    {
        attackTimer.Stop();
        animatedSprite.Frame = 0;
        CallDeferred(nameof(disableShape));
        attacking = false;
    }

    private void disableShape()
    {
        attackShape.Disabled = true;
        hit = false;
    }

    public void OnBodyEntered(Node bodyEntered)
    {
        if (!hit)
        {
            EmitSignal(nameof(PlayerHit));
            hit = true;
        }

    }

    public void OnAttackRegionEntered(Node bodyEntered)
    {
        EmitSignal(nameof(PlayerEnteredRegion));
    }

    public void OnAttackRegionExitted(Node bodyExitted)
    {
        animatedSprite.Frame = 0;
        EmitSignal(nameof(PlayerExitedRegion));
    }
}
