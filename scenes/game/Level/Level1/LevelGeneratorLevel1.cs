using Chickensoft.GodotTestDriver.Input;
using Godot;
using System;

public partial class LevelGeneratorLevel1 : LevelGenerator
{
    public level1_music Music;
    public Node2D level;
    public float sec = 0f;
    public override void _Ready()
    {
        BattleSection = new Godot.Collections.Array<PackedScene>{
        GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level1/Room1.tscn"),
        GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level1/Room2.tscn")
        };

        EndRooms = new Godot.Collections.Array<PackedScene>{
        GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level1/EndRooms/EndRoom1.tscn")
        };

        Enemies = new Godot.Collections.Array<PackedScene>{
        GD.Load<PackedScene>("res://scenes/game/entities/FloatingEye/FloatingEye.tscn")
        };

        BossRoom = GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level1/BossRoom.tscn");

        NextScenePath = "res://scenes/game/Level/Level2/level2.tscn";

        Player = GetParent().GetNode<player>("Player");

        Music = GetNode<level1_music>("/root/Level1PlatformerMusic");
        level = GetNode<Node2D>("../");
        if (settings.Audio)
        {
            if (!(bool)level.Get("isBattleSection"))
            {
                Music.musicp.Play();
            }
        }
        _spawn_levels();
    }

    public override void _Process(double delta)
    {
        if (settings.Audio)
        {
            if ((bool)level.Get("isBattleSection"))
            {
                if (Music.musicp.Playing)
                {
                    sec = Music.musicp.GetPlaybackPosition();
                    stopper(Music.musicp);
                }
                if (!Music.musicb.Playing)
                {
                    starter(Music.musicb);
                } else if (Music.musicb.VolumeDb == -80)
                {
                    Music.musicb.VolumeDb = -20;
                }
            }

            else
            {
                stopper(Music.musicb);
                if ((!Music.musicp.Playing) && !(bool)level.Get("game_paused"))
                {
                    starter(Music.musicp);
                    Music.musicp.Seek(sec);
                } else if (Music.musicp.VolumeDb == -80)
                {
                    Music.musicp.VolumeDb = -20;
                }
            }
        }
        else
        {
            Music.musicp.VolumeDb = -80;
            Music.musicb.VolumeDb = -80;
        }
    }

    public async void stopper(AudioStreamPlayer music)
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(music, "volume_db", -80, 2f);
        tween.TweenCallback(Callable.From(music.Stop));
        await ToSignal(tween, Tween.SignalName.Finished);
    }
    public async void starter(AudioStreamPlayer music)
    {
        Tween tween = GetTree().CreateTween();
        music.Play();
        tween.TweenProperty(music, "volume_db", -20, 2f);
        await ToSignal(tween, Tween.SignalName.Finished);
    }

}
