using Godot;
using System;

public partial class Manager : Node2D
{
	public bool game_paused = false;
	public Control pause_menu;
	public static Button button;
	public static Button button_s;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pause_menu = GetNode<Control>("../InLevelUI/Pause");
		button_s = GetNode<Button>("../InLevelUI/Pause/Panel/Sounds/Sounds");
		if (!settings.Sound){
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
		}else{
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button.png");
		}
		AddChild(button_s);
		button = GetNode<Button>("../InLevelUI/Pause/Panel/Audio/Audio");
		if (!settings.Audio){
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button Off.png");
		}else{
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button.png");
		}
		AddChild(button);
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

	public void _on_resum_pressed()
	{
		game_paused = !game_paused;
	}
	public void _on_new_game_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/game/Level/Level1/level1.tscn");
	}
	public void _on_exit_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Menu/menu.tscn");
	}
	
	public void _on_audio_pressed()
	{
		if (settings.Audio){
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button Off.png");
			settings.Audio = false;
		}else{
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button.png");
			settings.Audio = true;
		}
	}
	public void _on_sounds_pressed()
	{
		if (settings.Sound){
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
			settings.Sound = false;
		}else{
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button.png");
			settings.Sound = true;
		}
	}
	
}
