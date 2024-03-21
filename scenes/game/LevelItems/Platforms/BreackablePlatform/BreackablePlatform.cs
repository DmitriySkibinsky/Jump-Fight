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
	

	public override void _on_area_2d_body_entered(player body){
		if (body.Velocity.Y >= 0){
			body.velocity.Y = -950;
			body.MoveAndSlide();
			this.QueueFree();
		}
	}
}
