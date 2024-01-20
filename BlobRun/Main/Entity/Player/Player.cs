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
	private float JumpSpeed = 5.0f;

	[Export]
	private float Gravity = 10.0f;

	[Export]
	private int MaxJump = 6;

	private Vector2 RawPosition = Vector2.Zero;
	private int JumpCounter = 0;

	private Label DebugJumpLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IdleTimer = GetNode<Timer>(IdleTimerPath);
		PlayerSprite = GetNode<AnimatedSprite2D>(PlayerSpritePath);
		IdleTimer.Timeout += () =>
		{
			if (PlayerSprite.Animation == "Idle")
			{
				PlayerSprite.Frame = 0;
				PlayerSprite.SpeedScale = 0.5f;
				PlayerSprite.Play();
			}
		};
		IdleTimer.Start();
		RawPosition = Position;

		if (Constants.Debug) {
			DebugJumpLabel = GetNode<Label>("JumpCounter");
			DebugJumpLabel.Visible = true;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Position = RawPosition; // Restore position

		Vector2 velocity = Velocity;

		if (Input.IsActionPressed(Actions.Left.ToString())) {
			velocity.X = -Speed;
		} else if (Input.IsActionPressed(Actions.Right.ToString())) {
			velocity.X = Speed;
		} else {
			velocity.X = 0;
		}

		if (Input.IsActionPressed(Actions.Jump.ToString()) && JumpCounter < MaxJump) {
			JumpCounter++;
			velocity.Y = -JumpSpeed;
		}

		velocity.Y += (float)delta * Gravity;
		Velocity = velocity;

		bool collided = MoveAndSlide();
		RawPosition = Position; // Store acutal position

		if (collided) {
			KinematicCollision2D collision = GetLastSlideCollision();
			if (collision.GetAngle() == 0.0f && JumpCounter > 0) {
				JumpCounter = 0;
			}
			
		}

		if (Constants.Debug) {
			DebugJumpLabel.Text = JumpCounter.ToString();
		}

		Position = Position.Round(); // Round for rendering
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
