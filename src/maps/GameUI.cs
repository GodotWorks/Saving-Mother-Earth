using Godot;
using System;

public class GameUI : CanvasLayer
{
    private Label playerHP;
    private World world;

    public override void _Ready()
    {
        // References
        playerHP = GetNode<Label>("PlayerHP");
        world = GetParent<World>();
        world.Connect(nameof(World.WorldReady), this, nameof(OnWorldReady));
    }

    private void OnWorldReady() {
        // Connect Player UI
        playerHP.Text = world.Player.Health.ToString();
        world.Player.Connect(nameof(Player.OnPlayerHPChanged), this, nameof(OnPlayerChangeHP));
    }


    private void OnPlayerChangeHP(int newHP) {
        playerHP.Text = newHP.ToString();
    }

}
