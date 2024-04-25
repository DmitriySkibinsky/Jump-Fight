using Godot;
using System;

public partial class settings : Control
{
	public static bool Audio = true;
	public static bool Sound = true;
	public static Button button;
	public static Button button_s;
    public AudioStreamPlayer click;
    public AudioStreamPlayer mute;

    public override void _Ready()
	{
		button_s = GetNode<Button>("Panel/Sounds/Sounds");
		if (!Sound){
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
		}else{
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button.png");
		}
		AddChild(button_s);
		button = GetNode<Button>("Panel/Audio/Audio");
		if (!Audio){
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button Off.png");
		}else{
			button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button.png");
		}
		AddChild(button);

        click = GetNode<AudioStreamPlayer>("Buttons");
        mute = GetNode<AudioStreamPlayer>("AudioButtons");
    }
	
	public async void _on_quit_pressed()
	{
		if (Sound)
		{
            click.Play();
            await ToSignal(click, AudioStreamPlayer.SignalName.Finished);
        }
        GetTree().Quit();
	}
	public async void _on_exit_pressed()
	{
        if (Sound)
        {
            click.Play();
            await ToSignal(click, AudioStreamPlayer.SignalName.Finished);
        }
        GetTree().ChangeSceneToFile("res://scenes/Menu/menu.tscn");
	}
	
	public void _on_audio_pressed()
	{
		if (Audio){
            if (Sound)
            {
                mute.Play();
            }
            button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button Off.png");
			Audio = false;
		}else{
            if (Sound)
            {
                mute.Play();
            }
            button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Music Square Button.png");
			Audio = true;
		}
	}
	public void _on_sounds_pressed()
	{
		if (Sound){
            mute.Play();
            button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
			Sound = false;
		}else{
			button_s.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button.png");
			Sound = true;
		}
	}
}
