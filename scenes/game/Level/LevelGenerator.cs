using Godot;
using System;

public partial class LevelGenerator : Node2D
{
	public Godot.Collections.Array<PackedScene> BattleSection = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Room1.tscn"),
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/Room2.tscn")
	};

	public Godot.Collections.Array<PackedScene> Platforms = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Platforms/JumpPlatform/JumpPlatform.tscn"),
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Platforms/BreackablePlatform/BreackablePlatform.tscn")
	};

	public Godot.Collections.Array<PackedScene> EndRooms = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Rooms/EndRooms/EndRoom.tscn")
	};

	public int num_levels = 4;

	public float last_platform_pos_y = 0;
	public Camera2D camera;
	public player Player;

	public PackedScene TriggerOnEnterBattle = GD.Load<PackedScene>("res://scenes/game/Utils/TriggerOnEnterBattle.tscn");
	public PackedScene TriggerOnExitBattle = GD.Load<PackedScene>("res://scenes/game/Utils/TriggerOnExitBattle.tscn");
	public override void _Ready()
	{
		Player = GetParent().GetNode<player>("Player");
	
		_spawn_levels();
	}

	public override void _Process(double delta)
	{
	}
	// Памагити
	public void _spawn_levels(){
		Node2D prev_battle_room = new Node2D();
		Node2D cur_battle_room = new Node2D();
		string prev_room_name = "";
		for (int i = 0; i < num_levels; i++){
			if (i == 0){
				GeneratePlatform(Player.Position.Y, 10);
				prev_room_name = "platform";
			}else if(prev_room_name == "platform" && i != num_levels-1){
				Random rand = new Random();
				int randomRoom = rand.Next(BattleSection.Count);
				cur_battle_room = BattleSection[randomRoom].Instantiate<Room>();
				cur_battle_room.Position = new Vector2(cur_battle_room.Position.X, last_platform_pos_y - 200);
				prev_battle_room = cur_battle_room;
				prev_room_name = "battle";

				Node2D EnterTrigger = TriggerOnEnterBattle.Instantiate<TriggerOnEnterBattle>();
				EnterTrigger.Position = new Vector2(cur_battle_room.Position.X, cur_battle_room.Position.Y - 500);

				Node2D ExitTrigger = TriggerOnExitBattle.Instantiate<TriggerOnExitBattle>();
				ExitTrigger.Position = new Vector2(ExitTrigger.Position.X, prev_battle_room.GetNode<Marker2D>("NextPlatform").GlobalPosition.Y - 200);
				this.AddChild(cur_battle_room);
				this.AddChild(EnterTrigger);
				this.AddChild(ExitTrigger);
			}else if(i == num_levels-1){
				Random rand = new Random();
				int randomRoom = rand.Next(EndRooms.Count);
				Node2D EndRoom = EndRooms[randomRoom].Instantiate<Node2D>();
				EndRoom.Position = new Vector2(EndRoom.Position.X, last_platform_pos_y - 50);
				this.AddChild(EndRoom);
			}else{
				float init_pos_y = prev_battle_room.GetNode<Marker2D>("NextPlatform").GlobalPosition.Y;
				GeneratePlatform(init_pos_y, 30);
				prev_room_name = "platform";
			}
		}
	}

	private void _physics_process(float delta)
	{
		
	}

	public void GeneratePlatform(float initial_pos_y, int amount){
		Random rnd = new Random();
		for (int i = 0; i < amount; i++){
			int random_y = rnd.Next(150, 225);
			int random_x = rnd.Next(700, 1225);
			int random_platform = rnd.Next(Platforms.Count);
			initial_pos_y -= random_y;
			Node2D new_platform = Platforms[random_platform].Instantiate<Node2D>();
			new_platform.Position = new Vector2(random_x, initial_pos_y);
			last_platform_pos_y = initial_pos_y;
			this.AddChild(new_platform);
		}
	}

}
