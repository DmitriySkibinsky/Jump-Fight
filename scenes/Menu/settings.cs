using Godot;
using System;

public partial class settings : Control
{
	public static bool Audio = true;
	public static bool Sound = true;
	public static Button button;
	public static Button button_s;
	
	public override void _Ready()
	{
		button_s = GetNode<Button>("Panel/Sounds/Sounds");
		if (!Sound){
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button Off.png");
		}else{
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button.png");
		}
		AddChild(button_s);
		button = GetNode<Button>("Panel/Audio/Audio");
		if (!Audio){
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
		}else{
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button.png");
		}
		AddChild(button);
		
	}
	
	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	public void _on_exit_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Menu/menu.tscn");
	}
	
	public void _on_audio_pressed()
	{
		if (Audio){
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
			Audio = false;
		}else{
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button.png");
			Audio = true;
		}
	}
	public void _on_sounds_pressed()
	{
		if (Sound){
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button Off.png");
			Sound = false;
		}else{
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button.png");
			Sound = true;
		}
	}
}
