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

    public override void _PhysicsProcess(float delta)
    {
        if (launch) {
            KinematicCollision2D collision = MoveAndCollide(direction * Speed * delta);
            if (collision != null) {
                if  (collision.Collider is LightEnemy){
                    LightEnemy enemy = (LightEnemy) collision.Collider;
                    enemy.TakeDamage(2);
                }

                QueueFree();
            }
        }
    }

    public void Launch(Vector2 direction) {
        this.direction = direction;
        launch = true;
    }
}
