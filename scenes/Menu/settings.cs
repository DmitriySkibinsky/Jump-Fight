using Godot;
using System;

public partial class settings : Control
{
	public static bool audio = true;
	public static bool sound = true;
	public static Button button;
	public static Button button_s;
	
	public override void _Ready()
	{
		button_s = GetNode<Button>("Panel/Sounds/Sounds");
		if (!sound){
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button Off.png");
		}else{
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button.png");
		}
		AddChild(button_s);
		button = GetNode<Button>("Panel/Audio/Audio");
		if (!audio){
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
	
	private void _on_audio_pressed()
	{
		if (audio){
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
			audio = false;
		}else{
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button.png");
			audio = true;
		}
	}
	private void _on_sounds_pressed()
	{
		if (sound){
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button Off.png");
			sound = false;
		}else{
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button.png");
			sound = true;
		}
	}
}
