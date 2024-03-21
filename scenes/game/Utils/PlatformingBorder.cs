using Godot;
using System;

public partial class PlatformingBorder : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_area_left_body_entered(player body){
		body.teleport_to(GetNode<Area2D>("AreaRight").Position.X);
	}
	public void _on_area_right_body_entered(player body){
		body.teleport_to(GetNode<Area2D>("AreaLeft").Position.X);
	}

	public void turn_off(){
		GetNode<Area2D>("AreaLeft").SetCollisionMaskValue(8, false);
		GetNode<Area2D>("AreaRight").SetCollisionMaskValue(8, false);
	}

	public void turn_on(){
		GetNode<Area2D>("AreaLeft").SetCollisionMaskValue(8, true);
		GetNode<Area2D>("AreaRight").SetCollisionMaskValue(8, true);
	}
}
