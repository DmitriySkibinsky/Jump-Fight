using Godot;
using System;
using System.Runtime.InteropServices;

public partial class Enemy : CharacterBody2D
{

	private int speed = 50;
	private int direction = 1;
	//private float Gravity = (float)ProjectSettings.GetSetting("physics/2d/deault_gravity");
	private float Gravity = 100;
	private RayCast2D rayCast2D;
	int Damage = 20;
	private CustomSignals _customSignals;
	public override void _Ready()
	{
		RayCast2D rayCast2D = GetNode<RayCast2D>("RayCast2D");
		Area2D HitBoxes = GetNode<Area2D>("HitBoxes");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		HitBoxes.AreaEntered += Attack;
	}

	public override void _Process(double delta)
	{
		Vector2 velocity = Vector2.Zero;
		RayCast2D rayCast2D = this.GetNode<RayCast2D>("RayCast2D");

		if (IsOnFloor())
		{
			if (!rayCast2D.IsColliding() || IsOnWall())
			{
				direction *= -1;
				Vector2 Reverse = new Vector2(-1, 1);
				rayCast2D.Position *= Reverse;
				GetNode<CollisionShape2D>("HitBoxes/Box1").Position *= Reverse;
			}

			velocity.X += direction * speed * (float)delta;
		}
		else
		{
			velocity.Y += Gravity * (float)delta;
		}

		MoveAndCollide(velocity);
		MoveAndSlide();
	}

	public void Attack(Node2D body)
	{
		if (body.GetParent().Name == "player")
		{
			_customSignals.EmitSignal(nameof(CustomSignals.DamagePlayer), Damage);
		}
	}
}
