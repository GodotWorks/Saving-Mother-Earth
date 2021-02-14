using Godot;
using System;

public class LightEnemyAttack : Area2D
{
    // Attack
    private Timer attackTimer;
    private Timer cooldownTimer;
    private float timeInterval;
    private float timeReset = 0.1f;


    // References
    private AnimatedSprite animatedSprite;
    private Vector2 direction;
    private CollisionShape2D attackShape;

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

        attackShape.Disabled = true;
    }

    private void NextFrame()
    {
        animatedSprite.Frame++;
    }

    // Time To complete animation (attackspeed)
    public void StartAttack(float timeToComplete, Vector2 target)
    {
        GD.Print("Starting Attack");
        timeInterval = timeToComplete / animatedSprite.Frames.GetFrameCount("default");
        GD.Print(timeInterval);
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
        }

        NextFrame();
    }

    private void OnCooldownTimerTimeout()
    {
        StopAttack();
        attackTimer.Start(timeInterval);
    }

    public void StopAttack()
    {
        attackTimer.Stop();
        animatedSprite.Frame = 0;
        CallDeferred(nameof(disableShape));
    }

    private void disableShape()
    {
        attackShape.Disabled = true;
    }

    public void OnBodyEntered(Node bodyEntered)
    {
        EmitSignal(nameof(PlayerHit));
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
