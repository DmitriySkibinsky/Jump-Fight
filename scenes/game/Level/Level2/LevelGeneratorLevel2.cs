using Godot;
using System;

public partial class LevelGeneratorLevel2 : LevelGenerator
{
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

		NextScenePath = "res://scenes/Menu/menu.tscn";

		Player = GetParent().GetNode<player>("Player");
	
		_spawn_levels();
	}
}
