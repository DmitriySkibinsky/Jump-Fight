using Godot;
using System;

public partial class JumpPlatform : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	[Export] 
	public int JumpForce = 950;
	public virtual void _on_area_2d_body_entered(player body)
	{
		if (body.Velocity.Y >= 0){
			body.velocity.Y = -JumpForce;
			body.MoveAndSlide();
		}
	}
}



