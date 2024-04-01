using Godot;
using System;

public partial class LevelGenerator1 : LevelGenerator
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BattleSection = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Room2.tscn")
		};
		Player = GetParent().GetNode<player>("Player");
	
		_spawn_levels();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
