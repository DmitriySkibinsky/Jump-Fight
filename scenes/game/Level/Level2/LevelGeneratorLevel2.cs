using Godot;
using System;

public partial class LevelGeneratorLevel2 : LevelGenerator
{
	public level1_music stop;
	public level2_music Music;
    public Node2D level;
    public BringerOfDeath boss;
    public float sec = 0f;
    public override void _Ready()
	{
		BattleSection = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level2/Room1.tscn")
		};

		EndRooms = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level2/EndRooms/EndRoom1.tscn")
		};

		Enemies = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/entities/FloatingEye/FloatingEye.tscn")
		};

		BossRoom = GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level2/BossRoom.tscn");

		NextScenePath = "res://scenes/game/Level/Level3/level3.tscn";

		Player = GetParent().GetNode<player>("Player");

        stop = GetNode<level1_music>("/root/Level1PlatformerMusic");

        stop.musicp.Stop();

		Music = GetNode<level2_music>("/root/Level2PlatformerMusic");

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
        boss = (BringerOfDeath)GetTree().GetFirstNodeInGroup("Boss");

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
