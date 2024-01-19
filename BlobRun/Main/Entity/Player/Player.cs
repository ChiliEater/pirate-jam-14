using Godot;
using System;

namespace BlobRun.Main.Entity.Player;
public partial class Player : Node2D
{
    [Export]
    private Timer IdleTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
