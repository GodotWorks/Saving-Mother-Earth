using Godot;
using System;

public class LightEnemyAttack : Area2D
{

    // References
    private AnimatedSprite animatedSprite;
    private Vector2 direction;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    private void NextFrame() {
        animatedSprite.Frame++;
    }

    // Time To complete animation (attackspeed)
    public void StartAttack(float timeToComplete, Vector2 target) {
        this.direction = target - GlobalPosition;
        LookAt(target);
        RotationDegrees += 90;
    }

    public void UpdateDirection(Vector2 target) {
        this.direction = target - GlobalPosition;
        LookAt(target);
        RotationDegrees += 90;
    }

    public void StopAttack() {

    }

    public void OnBodyEntered(Node bodyEntered) {
        GD.Print("Hit Something!");
    }
}
