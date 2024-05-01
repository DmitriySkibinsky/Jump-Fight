using Godot;
using Shouldly;
using System;

public partial class LevelGenerator : Node2D
{
	public Godot.Collections.Array<PackedScene> BattleSection = new Godot.Collections.Array<PackedScene>{
	};

	public Godot.Collections.Array<PackedScene> Platforms = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Platforms/JumpPlatform/JumpPlatform.tscn"),
		GD.Load<PackedScene>("res://scenes/game/LevelItems/Platforms/BreackablePlatform/BreackablePlatform.tscn")
	};

	public Godot.Collections.Array<PackedScene> EndRooms = new Godot.Collections.Array<PackedScene>{
	};
	public PackedScene BossRoom = new PackedScene();
	public Godot.Collections.Array<PackedScene> Enemies = new Godot.Collections.Array<PackedScene>{
		GD.Load<PackedScene>("res://scenes/game/entities/FloatingEye/FloatingEye.tscn"),
		GD.Load<PackedScene>("res://scenes/game/entities/FloatingEye/FloatingSkull.tscn"),
		GD.Load<PackedScene>("res://scenes/game/entities/FloatingEye/Gargoyle.tscn"),
		GD.Load<PackedScene>("res://scenes/game/entities/FloatingEye/MutatedBat.tscn"),
	};
	public PackedScene JumpBoost = GD.Load<PackedScene>("res://scenes/game/entities/Collectibles/JumpBoost/jump_boost.tscn");
	
	public bool IsBoostSpawned = false;
	public string NextScenePath;
	public int num_levels = 12;
	public int platform_amount = 10;
	public float last_platform_pos_y = 0;
	public Camera2D camera;
	public player Player;
	public level Level;

	public PackedScene TriggerOnEnterBattle = GD.Load<PackedScene>("res://scenes/game/Utils/TriggerOnEnterBattle.tscn");
	public PackedScene TriggerOnExitBattle = GD.Load<PackedScene>("res://scenes/game/Utils/TriggerOnExitBattle.tscn");
	public override void _Ready()
	{
		Player = GetParent().GetNode<player>("Player");
		Level = (level)this.Owner;
	
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
				GeneratePlatform(Player.Position.Y, platform_amount);
				prev_room_name = "platform";
			}else if(prev_room_name == "platform" && i != num_levels-3 && i != num_levels-1){
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
			}else if(i == num_levels-3){
				Node2D bossRoom = BossRoom.Instantiate<Node2D>();
				bossRoom.GlobalPosition = new Vector2(bossRoom.GlobalPosition.X, last_platform_pos_y - 200);
				prev_battle_room = bossRoom;
				Node2D EnterTrigger = TriggerOnEnterBattle.Instantiate<TriggerOnEnterBattle>();
				EnterTrigger.Position = new Vector2(bossRoom.Position.X, bossRoom.Position.Y - 500);

				Node2D ExitTrigger = TriggerOnExitBattle.Instantiate<TriggerOnExitBattle>();
				ExitTrigger.Position = new Vector2(ExitTrigger.Position.X, bossRoom.GetNode<Marker2D>("NextPlatform").GlobalPosition.Y - 200);
				this.AddChild(bossRoom);
				this.AddChild(EnterTrigger);
				this.AddChild(ExitTrigger);
				prev_room_name = "boss";
			}else if( i == num_levels-1){
				Random rand = new Random();
				int randomRoom = rand.Next(EndRooms.Count);
				Node2D EndRoom = EndRooms[randomRoom].Instantiate<Node2D>();
				EndRoom.Position = new Vector2(EndRoom.Position.X, last_platform_pos_y - 50);
				EndRoom.Set("NextScenePath", NextScenePath);
				this.AddChild(EndRoom);
			}else{
				platform_amount += 4;
				float init_pos_y = prev_battle_room.GetNode<Marker2D>("NextPlatform").GlobalPosition.Y;
				GeneratePlatform(init_pos_y, platform_amount);
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
			float enemy_pos_y = initial_pos_y;
			if (rnd.Next(0, 10) >= 8){
				enemy_pos_y -= rnd.Next(75, 200);
				Node2D new_enemy = Enemies[rnd.Next(Enemies.Count)].Instantiate<Node2D>();
				new_enemy.Position = new Vector2(960, enemy_pos_y);
				this.AddChild(new_enemy);
			}

			int random_y = rnd.Next(225, 350);
			int random_x = rnd.Next(700, 1225);
			int random_platform = rnd.Next(Platforms.Count);
			initial_pos_y -= random_y;
			Node2D new_platform = Platforms[random_platform].Instantiate<Node2D>();
			new_platform.Position = new Vector2(random_x, initial_pos_y);
			last_platform_pos_y = initial_pos_y;
			this.Owner.CallDeferred("RegisterPlatform", new_platform);
			this.AddChild(new_platform);
			
			if (rnd.Next(0, 10) > 8 && !IsBoostSpawned){
				Node2D new_boost = JumpBoost.Instantiate<Node2D>();
				new_boost.Position = new Vector2(new_platform.Position.X, new_platform.Position.Y-75);
				this.AddChild(new_boost);
				IsBoostSpawned = true;
			}
		}
	}

}
