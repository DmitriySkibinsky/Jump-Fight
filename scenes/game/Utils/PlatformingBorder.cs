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
	
	public void _on_area_left_body_entered(Node2D body){
		if (body.Name == "Player"){
			body.CallDeferred("teleport_to", GetNode<Area2D>("AreaRight").Position.X-40);
		}
		
	}
	public void _on_area_right_body_entered(Node2D body){
		if (body.Name == "Player"){
			body.CallDeferred("teleport_to", GetNode<Area2D>("AreaLeft").Position.X+40);
		}
	}

	public void turn_off(){
		GetNode<Area2D>("AreaLeft").SetCollisionMaskValue(2, false);
		GetNode<Area2D>("AreaRight").SetCollisionMaskValue(2, false);
	}

	public void turn_on(){
		GetNode<Area2D>("AreaLeft").SetCollisionMaskValue(2, true);
		GetNode<Area2D>("AreaRight").SetCollisionMaskValue(2, true);
	}
}
