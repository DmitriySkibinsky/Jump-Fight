using Godot;
using System;

public partial class in_level_ui : CanvasLayer
{
	[Export]
	private float anim_speed = 0.8f;

	private PathFollow2D PathLeft;
	private PathFollow2D PathRight;
	[Export]
	public bool is_animate_out = false;
	[Export]
	public bool is_animate_in = false;

	public override void _Ready()
	{
		this.PathLeft = GetNode<PathFollow2D>("Path2DLeft/PathFollow2D");
		this.PathRight = GetNode<PathFollow2D>("Path2DRight/PathFollow2D");
	}
	public void animate_out(){
		is_animate_out = true;
	}

	public void animate_in(){
		is_animate_in = true;
	}

	public override void _Process(double delta)
	{
		if(is_animate_out && PathLeft.ProgressRatio < 0.5){
			PathLeft.ProgressRatio += (float)delta * anim_speed;
			PathRight.ProgressRatio += (float)delta * anim_speed;
			
		}
		
		if(PathLeft.ProgressRatio >= 0.5 && !is_animate_in){
			is_animate_out = false;
		}
		
		if(is_animate_in && PathLeft.ProgressRatio <= 1 && PathLeft.ProgressRatio >= 0.5){
			PathLeft.ProgressRatio += (float)delta * anim_speed;
			PathRight.ProgressRatio += (float)delta * anim_speed;
		}

		if(PathLeft.ProgressRatio <= 0.5 && !is_animate_out){
			is_animate_in = false;
		}
	}
	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	public void _on_settings_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Menu/settingspause.tscn");
	}
}

