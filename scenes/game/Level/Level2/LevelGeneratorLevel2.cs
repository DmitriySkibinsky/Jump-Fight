using Godot;
using System;

public partial class LevelGeneratorLevel2 : LevelGenerator
{
	public level1_music stop;
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

		NextScenePath = "res://scenes/game/Level/Level3/level3.tscn";

		Player = GetParent().GetNode<player>("Player");

        stop = GetNode<level1_music>("/root/Level1PlatformerMusic");
        stop.musicp.Stop();

        _spawn_levels();
	}
}
