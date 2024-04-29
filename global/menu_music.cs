using Godot;
using System;

public partial class menu_music : Node
{
    public AudioStreamPlayer music;
    public override void _Ready()
    {
        music = new AudioStreamPlayer();
        AddChild(music);
        music.Stream = GD.Load<AudioStream>("res://assets/Music/Pixel2.mp3");
        music.Play();
        music.VolumeDb = -20;
    }
    public override void _Process(double delta)
    {
        if (settings.Audio)
        {
            music.VolumeDb = -20;
        }
        else
        {
            music.VolumeDb = -80;
        }
    }
}
