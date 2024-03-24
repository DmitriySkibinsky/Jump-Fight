using Godot;
using System;

public partial class settings : Node2D
{

	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	public void _on_exit_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Menu/menu.tscn");
	}
}






