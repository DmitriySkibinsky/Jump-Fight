using Godot;
using System;

public partial class LevelGeneratorLevel3 : LevelGenerator
{
	public override void _Ready()
	{
		BattleSection = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level3/Room1.tscn")
		};

		EndRooms = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Level3/EndRooms/EndRoom.tscn")
		};

		Enemies = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/entities/FloatingEye/FloatingEye.tscn")
		};

		NextScenePath = "res://scenes/Menu/menu.tscn";

		Player = GetParent().GetNode<player>("Player");
	
		_spawn_levels();
	}
}
