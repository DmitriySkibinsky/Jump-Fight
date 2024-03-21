using Godot;
using System;

public partial class Room1 : Node2D
{
	private Camera2D camera;
	private player player;

	private PathFollow2D pathLeft;
	private PathFollow2D pathRight;
	[Export]
	private double border_speed = 0.95;
	private bool isBorderAnimate = false;
	private bool isMoveableCamera = false;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
