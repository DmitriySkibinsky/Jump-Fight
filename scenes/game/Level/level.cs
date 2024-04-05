using Godot;
using System;

public partial class level : Node2D
{	
	public bool isMoveableCamera = false;
	public Camera2D camera;
	public player player;
	public override void _Ready()
	{
		this.player = GetNode<player>("Player");
		this.camera = GetNode<Camera2D>("PlayerCamera");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player.Position.Y < camera.Position.Y && isMoveableCamera){
			camera.Position = new Vector2(camera.Position.X, player.Position.Y + 20);
		}else if (player.Position.Y < camera.Position.Y){
			camera.Position = new Vector2(camera.Position.X, player.Position.Y);
		}
	}

	public void _on_cleaner_area_body_entered(Node2D body){
		if (body.Name == "Player"){
			player player = (player)body;
			player.GetDamaged(20);
			player.Position = new Vector2(body.Position.X, camera.Position.Y);
			player.Velocity = new Vector2(0, 0);
		}else{
			body.QueueFree();
		}
	}

	public void _on_button_pressed()
	{
		GetTree().Quit();
	}
	
}

