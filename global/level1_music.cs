using Godot;
using System;

public partial class level1_music : Node
{
    public AudioStreamPlayer musicp;

    public AudioStreamPlayer musicb;
    public override void _Ready()
    {
        musicp = new AudioStreamPlayer();
        AddChild(musicp);
        musicp.Stream = GD.Load<AudioStream>("res://assets/Music/Pixel1.mp3");
        musicp.PitchScale = 1.2f;
        musicb = new AudioStreamPlayer();
        AddChild(musicb);
        musicb.Stream = GD.Load<AudioStream>("res://assets/Music/Pixel6.mp3");
        musicb.PitchScale = 1.2f;
        musicp.VolumeDb = -20;
        musicb.VolumeDb = -20;
    }
}
