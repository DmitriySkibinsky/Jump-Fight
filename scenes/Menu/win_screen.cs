using Godot;
using System;

public partial class win_screen : Control
{
	
	public static Button button;
	public static Button button_s;
    public AudioStreamPlayer click;
    public AudioStreamPlayer mute;

    public override void _Ready()
	{
        click = GetNode<AudioStreamPlayer>("Buttons");
        mute = GetNode<AudioStreamPlayer>("AudioButtons");
    }
	
	public async void _on_exit_pressed()
	{
        if (settings.Sound)
        {
            click.Play();
            await ToSignal(click, AudioStreamPlayer.SignalName.Finished);
        }
        GetTree().ChangeSceneToFile("res://scenes/Menu/menu.tscn");
	}

	public async void _on_new_game_pressed()
	{
        if (settings.Sound)
        {
            click.Play();
            await ToSignal(click, AudioStreamPlayer.SignalName.Finished);
        }
        GetTree().ChangeSceneToFile("res://scenes/game/Level/Level1/level1.tscn");
	}

	
	
}
