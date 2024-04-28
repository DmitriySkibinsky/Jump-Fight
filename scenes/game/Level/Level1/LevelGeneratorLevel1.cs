using Godot;
using System;

public partial class LevelGeneratorLevel1 : LevelGenerator
{
    public level1_music Music;
    public Node2D level;
    public Node2D manager;
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
        manager = GetNode<Node2D>("../Manager");
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
                if (Music.musicp.Playing) sec = Music.musicp.GetPlaybackPosition();
                Music.musicp.Stop();
                if (!Music.musicb.Playing) Music.musicb.Play();
            }

            else
            {
                Music.musicb.Stop();
                if (!Music.musicp.Playing && !(bool)level.Get("game_paused"))
                {
                    Music.musicp.Play();
                    Music.musicp.Seek(sec);
                }
            }
        }
    }

}
