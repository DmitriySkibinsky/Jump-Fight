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
		this.player = GetParent<Node2D>().GetParent<Node2D>().GetNode<player>("Player");
		this.camera = GetParent<Node2D>().GetParent<Node2D>().GetNode<Camera2D>("Camera2D");	
		this.pathLeft = GetParent<Node2D>().GetParent<Node2D>().GetNode<PathFollow2D>("CanvasLayer/Path2DLeft/PathFollow2D");
		this.pathRight = GetParent<Node2D>().GetParent<Node2D>().GetNode<PathFollow2D>("CanvasLayer/Path2DRight/PathFollow2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isBorderAnimate == true){
			pathLeft.ProgressRatio += (float)(delta * border_speed);
			pathRight.ProgressRatio += (float)(delta * border_speed);
		}
	}

	private void _physics_process(float delta){
		if (player.Position.Y < camera.Position.Y && isMoveableCamera){
			camera.Position = new Vector2(camera.Position.X, player.Position.Y + 25);
		}else if (player.Position.Y < camera.Position.Y){
			camera.Position = new Vector2(camera.Position.X, player.Position.Y);
		}
	}

	private void _on_border_animate_body_exited(player body){
		isBorderAnimate = true;
		isMoveableCamera = true;
	}
}
