using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class BreackablePlatform : JumpPlatform
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	

	public override void _on_area_2d_body_entered(Node2D body){
		if (body.Name == "Player"){
			player Player = (player)body;
			if (Player.Velocity.Y >= 0){
				Player.velocity.Y = -JumpForce;
				Player.MoveAndSlide();
				this.QueueFree();
			}
		}
	}
}
