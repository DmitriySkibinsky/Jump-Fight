using Godot;
using System;

public partial class menu : Node2D
{
	public AudioStreamPlayer click;

    public menu_music Music;

    public level1_music stop1;
    public level2_music stop2;
    public level3_music stop3;

    public override void _Ready()
    {
        click = GetNode<AudioStreamPlayer>("Buttons");
        Music = GetNode<menu_music>("/root/MenuMusic");
        stop1 = GetNode<level1_music>("/root/Level1PlatformerMusic");
        stop2 = GetNode<level2_music>("/root/Level2PlatformerMusic");
        stop3 = GetNode<level3_music>("/root/Level3PlatformerMusic");
        stop1.musicp.Stop();
        stop1.musicb.Stop();
        stop2.musicp.Stop();
        stop2.musicb.Stop();
        stop2.musicboss.Stop();
        stop3.musicp.Stop();
        stop3.musicb.Stop();
        stop3.musicboss.Stop();
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



