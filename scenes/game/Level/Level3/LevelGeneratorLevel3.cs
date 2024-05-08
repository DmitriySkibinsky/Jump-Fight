using Godot;
using System;

public partial class LevelGeneratorLevel3 : LevelGenerator
{
    public level2_music stop;
    public level3_music Music;
    public Node2D level;
    public MechaStoneGolem boss;
    public float sec = 0f;
    public override void _Ready()
	{
		BattleSection = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level3/Room1.tscn"),
        GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level3/Room2.tscn"),
        GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level3/Room3.tscn"),
        GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level3/Room4.tscn"),
        GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level3/Room5.tscn"),
		};

		EndRooms = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level3/EndRooms/EndRoom.tscn")
		};
        num_levels = 10;
		Enemies = new Godot.Collections.Array<PackedScene>{
        GD.Load<PackedScene>("res://scenes/game/entities/FloatingEye/Gargoyle.tscn"),
        GD.Load<PackedScene>("res://scenes/game/entities/FloatingEye/MutatedBat.tscn")
        };
		BossRoom = GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level3/BossRoom.tscn");
		NextScenePath = "res://scenes/Menu/win_screen.tscn";

		Player = GetParent().GetNode<player>("Player");

        stop = GetNode<level2_music>("/root/Level2PlatformerMusic");

        stop.musicp.Stop();

        Music = GetNode<level3_music>("/root/Level3PlatformerMusic");

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
        boss = (MechaStoneGolem)GetTree().GetFirstNodeInGroup("Boss");

        if (settings.Audio)
        {
            if ((bool)level.Get("isBattleSection") && boss == null)
            {
                Music.musicboss.Stop();
                if (Music.musicp.Playing)
                {
                    sec = Music.musicp.GetPlaybackPosition();
                    stopper(Music.musicp);
                }
                if (!Music.musicb.Playing)
                {
                    starter(Music.musicb);
                }
                else if (Music.musicb.VolumeDb == -80)
                {
                    Music.musicb.VolumeDb = -20;
                }
            }
            else if ((bool)level.Get("isBattleSection") && boss != null)
            {
                if (Music.musicp.Playing)
                {
                    sec = Music.musicp.GetPlaybackPosition();
                    stopper(Music.musicp);
                }
                if (!Music.musicboss.Playing)
                {
                    starter(Music.musicboss);
                }
                else if (Music.musicboss.VolumeDb == -80)
                {
                    Music.musicboss.VolumeDb = -20;
                }
            }
            else
            {
                stopper(Music.musicb);
                stopper(Music.musicboss);
                if ((!Music.musicp.Playing) && !(bool)level.Get("game_paused"))
                {
                    starter(Music.musicp);
                    Music.musicp.Seek(sec);
                }
                else if (Music.musicp.VolumeDb == -80)
                {
                    Music.musicp.VolumeDb = -20;
                }
            }

        }
        else
        {
            Music.musicp.VolumeDb = -80;
            Music.musicb.VolumeDb = -80;
            Music.musicboss.VolumeDb = -80;
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
