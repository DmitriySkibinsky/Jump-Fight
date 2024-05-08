using Godot;
using System;
using System.Collections.Generic;

public partial class Room : Node2D
{
	public PackedScene SpawnExplosionScene = GD.Load<PackedScene>("res://scenes/game/LevelItems/RoomItems/SpawnExplosion/SpawnExplosion.tscn");
	public PackedScene ExitPlatformScene = GD.Load<PackedScene>("res://scenes/game/LevelItems/RoomItems/ExitPlatform/ExitPlatform.tscn");

	public Dictionary<string, PackedScene> Enemies = new Dictionary<string, PackedScene>{
		{"Scav", GD.Load<PackedScene>("res://scenes/game/entities/Scav/Scav.tscn")},
		{"ScavLv2", GD.Load<PackedScene>("res://scenes/game/entities/Scav/ScavLv2.tscn")},
		{"ScavLv3", GD.Load<PackedScene>("res://scenes/game/entities/Scav/ScavLv3.tscn")},
        {"Barbarian", GD.Load<PackedScene>("res://scenes/game/entities/Barbarian/Barbarian.tscn")},
		{"BarbarianLv2", GD.Load<PackedScene>("res://scenes/game/entities/Barbarian/BarbarianLv2.tscn")},
		{"BarbarianLv3", GD.Load<PackedScene>("res://scenes/game/entities/Barbarian/BarbarianLv3.tscn")},
        {"Hunter", GD.Load<PackedScene>("res://scenes/game/entities/Hunter/Hunter.tscn")},
		{"HunterLv1", GD.Load<PackedScene>("res://scenes/game/entities/Hunter/HunterLv1.tscn")},
		{"HunterLv2", GD.Load<PackedScene>("res://scenes/game/entities/Hunter/HunterLv2.tscn")},
		{"HunterLv3", GD.Load<PackedScene>("res://scenes/game/entities/Hunter/HunterLv3.tscn")},
        {"Destroyer", GD.Load<PackedScene>("res://scenes/game/entities/Destroyer/Destroyer.tscn")},
    };
	public Node2D EnemyPositionsContainer;
	public Area2D PlayerDetector;
	public Marker2D ExitPlatformPosition;
	public Marker2D PlayerSpawn;
	public Area2D DamageBox;

	public int num_enemies;
	public override void _Ready()
	{
		EnemyPositionsContainer = GetNode<Node2D>("EnemyPositions");
		PlayerDetector = GetNode<Area2D>("PlayerDetector");
		DamageBox = GetNode<Area2D>("DamageBox");
		PlayerSpawn = GetNode<Marker2D>("PlayerSpawn");
		ExitPlatformPosition = GetNode<Marker2D>("ExitPlatformPosition");
		num_enemies = EnemyPositionsContainer.GetChildCount();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async void SpawnEnemies(){
		foreach(Marker2D EnemyPosition in EnemyPositionsContainer.GetChildren()){
			string EnemyName = EnemyPosition.Name.ToString();
			EnemyName = EnemyName.Substr(0, EnemyName.IndexOf("_"));
			Node2D enemy = Enemies[EnemyName].Instantiate<Node2D>();
			enemy.Connect("tree_exited", Callable.From(_on_enemy_killed));
			enemy.Position = EnemyPosition.Position;
			this.AddChild(enemy);

			Node2D SpawnExplosion = SpawnExplosionScene.Instantiate<Node2D>();
			SpawnExplosion.Position = EnemyPosition.Position;
			
			this.AddChild(SpawnExplosion);
			await ToSignal(GetTree().CreateTimer(0.2), SceneTreeTimer.SignalName.Timeout);
			
		}
	}

	public void _on_enemy_killed(){
		num_enemies -= 1;
		if (num_enemies == 0){
			ExitPlatform exitPlatform = ExitPlatformScene.Instantiate<ExitPlatform>();
			exitPlatform.Position = ExitPlatformPosition.Position;
			this.AddChild(exitPlatform);
		}
	}

	public async void _on_player_detector_body_entered(Node2D body){
		if (body.Name == "Player"){
			PlayerDetector.QueueFree();
			await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
			this.CallDeferred("SpawnEnemies");

			if (num_enemies == 0){
				ExitPlatform exitPlatform = ExitPlatformScene.Instantiate<ExitPlatform>();
				exitPlatform.Position = ExitPlatformPosition.Position;
				this.AddChild(exitPlatform);
			}

			DamageBox.SetCollisionMaskValue(2, true);
		}
	}

	public void _on_damage_box_body_entered(Node2D body){
		if (body.Name == "Player"){
			body.GlobalPosition = PlayerSpawn.GlobalPosition;
			body.CallDeferred("GetDamaged", 20);
		}
	}
}
