using Godot;
using System;

public partial class Sections : Node2D
{
	private PackedScene BattleSection;
	private PackedScene PlatformingSection = GD.Load<PackedScene>("res://scenes/game/LevelItems/PlatformingSection/PlatformingSection.tscn");

	[Export]
	private int platform_count = 0;
	public int num_levels = 6;
	private Camera2D camera;
	private player Player;
	public override void _Ready()
	{
		BattleSection = GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Room1.tscn");
		this.Player = GetParent<Node2D>().GetNode<player>("Player");
		this.camera = GetParent<Node2D>().GetNode<Camera2D>("Camera2D");
	
		_spawn_levels();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _spawn_levels(){
		Node2D prev_room = new Node2D();
		String prev_room_name = "";
		for (int i = 0; i < 2; i++){
			Node2D room;
			if (i == 0){
				room = PlatformingSection.Instantiate<Node2D>();
				room.Set("amount", 30);
				room.Position = new Vector2(room.Position.X, Player.Position.Y - 950);
				prev_room = room;
				prev_room_name = "PlatformingSection";
				AddChild(room);
				
			}else if (prev_room_name == "PlatformingSection"){
				room = BattleSection.Instantiate<Node2D>();
				room.Position = new Vector2(room.Position.X, -(float)prev_room.Get("max_height"));
				prev_room = room;
				prev_room_name = "BattleSection";
				AddChild(room);
			}else if(prev_room_name == "BattleSection"){
				room = PlatformingSection.Instantiate<Node2D>();
				room.Set("amount", 20);
				room.Position = new Vector2(room.Position.X, prev_room.GlobalPosition.Y);
				prev_room = room;
				prev_room_name = "PlatformingSection";
				AddChild(room);
			}
		}
	}

	private void _physics_process(float delta)
	{
		
	}
}
