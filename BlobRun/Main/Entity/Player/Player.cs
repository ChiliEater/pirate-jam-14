using BlobRun.Main.Inputs;
using Godot;
using System;

namespace BlobRun.Main.Entity.Player;
public partial class Player : CharacterBody2D
{
	[Export]
	private NodePath IdleTimerPath;
	private Timer IdleTimer;

	[Export]
	private NodePath PlayerSpritePath;
	private AnimatedSprite2D PlayerSprite;

	[Export]
	private float Speed = 5.0f;

	[Export]
	private float Gravity = 10.0f;

	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IdleTimer = GetNode<Timer>(IdleTimerPath);
		PlayerSprite = GetNode<AnimatedSprite2D>(PlayerSpritePath);
		IdleTimer.Timeout += () =>
		{
			if (PlayerSprite.Animation == "Idle")
			{
				GD.Print("Timeout");
				PlayerSprite.Frame = 0;
				PlayerSprite.SpeedScale = 0.5f;
				PlayerSprite.Play();
			}
		};
		IdleTimer.Start();
		GD.Print(Actions.Left.ToString());
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Vector2 velocity = Velocity;

		if (Input.IsActionPressed(Actions.Left.ToString())) {
			velocity.X = -Speed;
		} else if (Input.IsActionPressed(Actions.Right.ToString())) {
			velocity.X = Speed;
		} else {
			velocity.X = 0;
		}

		velocity.Y += (float)delta * Gravity;

		Velocity = velocity;

		MoveAndSlide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
