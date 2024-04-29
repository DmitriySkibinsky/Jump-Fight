using Godot;
using System;

public partial class menu : Node2D
{
	public AudioStreamPlayer click;

    public menu_music Music;

    public level1_music stop;

    public override void _Ready()
    {
        click = GetNode<AudioStreamPlayer>("Buttons");
        Music = GetNode<menu_music>("/root/MenuMusic");
        stop = GetNode<level1_music>("/root/Level1PlatformerMusic");
        stop.musicp.Stop();
        stop.musicp.Stop();
        if (!Music.music.Playing)
        {
            Music.music.Play();
        }
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



