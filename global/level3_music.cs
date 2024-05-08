using Godot;
using System;

public partial class level3_music : Node
{
    public AudioStreamPlayer musicp;

    public AudioStreamPlayer musicb;

    public AudioStreamPlayer musicboss;
    public override void _Ready()
    {
        musicp = new AudioStreamPlayer();
        AddChild(musicp);
        musicp.Stream = GD.Load<AudioStream>("res://assets/Music/Grind.mp3");
        musicb = new AudioStreamPlayer();
        AddChild(musicb);
        musicb.Stream = GD.Load<AudioStream>("res://assets/Music/Electromagnetic Waves.mp3");
        musicb.PitchScale = 1.2f;
        musicboss = new AudioStreamPlayer();
        AddChild(musicboss);
        musicboss.Stream = GD.Load<AudioStream>("res://assets/Music/Unknown Caverns.mp3");
        musicp.VolumeDb = -20;
        musicb.VolumeDb = -20;
        musicboss.VolumeDb = -20;
    }
}
