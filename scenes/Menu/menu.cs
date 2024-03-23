using Godot;
using System;

public partial class menu : Node2D
{
	public void _on_play_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/game/Level/level.tscn");
	}
	public void _on_quit_pressed(){
		GetTree().Quit();
	}
	public void _on_settings_pressed()
{
	GetTree().ChangeSceneToFile("res://scenes/Menu/settings.tscn");
}
	
}



