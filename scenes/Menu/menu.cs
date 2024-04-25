using Godot;
using System;

public partial class menu : Node2D
{
	public AudioStreamPlayer click;
	public override void _Ready()
	{
        click = GetNode<AudioStreamPlayer>("Buttons");
    }
    public async void _on_play_pressed()
	{
		if (settings.Sound)
		{
            click.Play();
            await ToSignal(click, AudioStreamPlayer.SignalName.Finished);
        }
		GetTree().ChangeSceneToFile("res://scenes/game/Level/Level1/level1.tscn");
	}
	public async void _on_quit_pressed()
	{
        if (settings.Sound)
        {
            click.Play();
            await ToSignal(click, AudioStreamPlayer.SignalName.Finished);
        }
        GetTree().Quit();
	}
	public async void _on_settings_pressed()
	{
        if (settings.Sound)
        {
            click.Play();
            await ToSignal(click, AudioStreamPlayer.SignalName.Finished);
        }
        GetTree().ChangeSceneToFile("res://scenes/Menu/settings.tscn");
	}
	
}



