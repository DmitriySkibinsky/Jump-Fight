using Godot;
using System;

public partial class LevelGeneratorLevel1 : LevelGenerator
{
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
	
		_spawn_levels();
	}

}
