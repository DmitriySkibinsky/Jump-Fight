using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class player : CharacterBody2D
{

	public const float Speed = 250.0f;
	public const float JumpVelocity = -450.0f;

	public  Vector2 velocity = new Vector2();

	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public int health = 100;

	private CustomSignals _customSignals;
	public override void _Ready()
	{
		AddUserSignal("DamagePlayer");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.DamagePlayer += GetDamaged;
	}

	public override async void _PhysicsProcess(double delta)

	{
		var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		int a = 0;


		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		if (Input.IsActionJustPressed("ui_accept")&& IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			anim.Play("Jump");
		}


		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			if (direction.X == 1)
			{
				anim.FlipH = false;
			}
			else
			{
				anim.FlipH = true;
			}
			if (velocity.Y == 0 && a != 1)
			{
				anim.Play("Run");
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			if (velocity.Y == 0 && a != 1)
			{
				anim.Play("Idle");
			}
		}
		if (velocity.Y > 0)
		{
			anim.Play("Fall");
		}

		if (health <= 0)
		{
			QueueFree();
			GetTree().ChangeSceneToFile("res://scenes/menu/menu.tscn");
		}


		Velocity = velocity;

		MoveAndSlide();

		void GetDamage()
		{
			health -= 10;
			anim.Play("Damage");
		}
	}

	public void GetDamaged(int Damage)
	{
		health -= Damage;
		GD.Print("Health = " + health);
	}

	public void teleport_to(float target_posX){
		Vector2 NewPos = new Vector2(target_posX, GlobalPosition.Y);
		GlobalPosition = NewPos;
	}
}
