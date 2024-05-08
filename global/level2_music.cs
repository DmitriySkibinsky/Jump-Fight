using Godot;
using System;

public partial class level2_music : Node
{
    public AudioStreamPlayer musicp;

    public AudioStreamPlayer musicb;

    public AudioStreamPlayer musicboss;
    public override void _Ready()
    {
        musicp = new AudioStreamPlayer();
        AddChild(musicp);
        musicp.Stream = GD.Load<AudioStream>("res://assets/Music/Land with No Dragons.mp3");
        musicp.PitchScale = 1.2f;
        musicb = new AudioStreamPlayer();
        AddChild(musicb);
        musicb.Stream = GD.Load<AudioStream>("res://assets/Music/Admin Rights - Full.mp3");
        musicb.PitchScale = 1.2f;
        musicboss = new AudioStreamPlayer();
        AddChild(musicboss);
        musicboss.Stream = GD.Load<AudioStream>("res://assets/Music/Falling to Earth (Loop).mp3");
        musicp.VolumeDb = -20;
        musicb.VolumeDb = -20;
        musicboss.VolumeDb = -20;
    }
}
