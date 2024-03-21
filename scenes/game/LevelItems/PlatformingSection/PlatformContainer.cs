using Godot;
using System;

public partial class PlatformContainer : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_area_right_body_entered(player body)
	{
		Area2D AreaLeft = GetNode<Area2D>("AreaLeft");
		body.teleport_to(AreaLeft.GlobalPosition.X);

	}

	private void _on_area_left_body_entered(player body)
	{
		Area2D AreaRight = GetNode<Area2D>("AreaRight");
		body.teleport_to(AreaRight.GlobalPosition.X);
	}


}

