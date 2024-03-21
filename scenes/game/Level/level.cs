using Godot;
using System;

public partial class level : Node2D
{	
	private bool isMoveableCamera = false;
	private Camera2D camera;
	private player player;
	public override void _Ready()
	{
		this.player = GetNode<player>("Player");
		this.camera = GetNode<Camera2D>("PlayerCamera");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player.Position.Y < camera.Position.Y && isMoveableCamera){
			camera.Position = new Vector2(camera.Position.X, player.Position.Y + 25);
		}else if (player.Position.Y < camera.Position.Y){
			camera.Position = new Vector2(camera.Position.X, player.Position.Y);
		}
	}

	

}
