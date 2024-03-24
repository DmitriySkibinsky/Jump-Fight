using Godot;
using System;

public partial class settingspause : Node2D
{

	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	public void _on_resum_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/game/Level/level.tscn");
	}
	private void _on_new_game_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/game/Level/level.tscn");
	}
}




