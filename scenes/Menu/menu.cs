using Godot;
using System;

public partial class menu : Node2D
{	
	public void _on_play_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/Game/game.tscn");
	}
	public void _on_quit_pressed(){
		GetTree().Quit();
	}
}
