using Godot;
using System;

public class World : Node2D
{
    private Player player;
    public Player Player
    {
        get { return player; }
    }

    [Signal]
    public delegate void WorldReady();

    public override void _Ready()
    {
        player = GetNode<Player>("Player");


        EmitSignal(nameof(WorldReady));
    }


}
