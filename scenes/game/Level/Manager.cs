using Godot;
using System;

public partial class Manager : Node2D
{
	public bool game_paused = false;
	public Control pause_menu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pause_menu = GetNode<Control>("../InLevelUI/Pause");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			game_paused = !game_paused;
		}

		if (game_paused)
		{
			GetTree().Paused = true;
			pause_menu.Visible = true;
			pause_menu.SetProcessInput(true);
		}
		else
		{
			GetTree().Paused = false;
			pause_menu.Visible = false;
			pause_menu.SetProcessInput(false);
		}
		}

	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	public void _on_resum_pressed()
	{
		game_paused = !game_paused;
	}
	public void _on_new_game_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/game/Level/Level1/level1.tscn");
	}
	
}
