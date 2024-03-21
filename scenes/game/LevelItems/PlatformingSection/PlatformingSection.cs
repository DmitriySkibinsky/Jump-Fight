using Godot;
using System;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;

public partial class PlatformingSection : Node2D
{
	private PlatformContainer platformContainer;
	private float platform_initial_position_y;
	private float platform_initial_position_y_start;

	private player player;
	private Area2D AreaRight;
	private Area2D AreaLeft;
	public PackedScene JumpPlatform_scene = new PackedScene();
	public PackedScene ChestPlatform_scene = new PackedScene();
	public PackedScene BreackablePlatform_scene = new PackedScene();
	private int ChestCounter = 0;
	[Export]
	public int amount;
	public int max_height;
	private float cur_height = 0;
	public override void _Ready()
	{	
		JumpPlatform_scene = GD.Load<PackedScene>("res://scenes/game/LevelItems/Platforms/JumpPlatform/JumpPlatform.tscn");
		ChestPlatform_scene = GD.Load<PackedScene>("res://scenes/game/LevelItems/Platforms/ChestPlatform.tscn");
		BreackablePlatform_scene = GD.Load<PackedScene>("res://scenes/game/LevelItems/Platforms/BreackablePlatform/BreackablePlatform.tscn");
		this.player = GetParent<Node2D>().GetParent<Node2D>().GetNode<player>("Player");
		this.AreaLeft = GetNode<Area2D>("PlatformContainer/AreaLeft");
		this.AreaRight = GetNode<Area2D>("PlatformContainer/AreaRight");
		this.platformContainer = GetNode<PlatformContainer>("PlatformContainer");
		this.platform_initial_position_y = GetNode<JumpPlatform>("PlatformContainer/JumpPlatform").Position.Y;
		this.platform_initial_position_y_start = platform_initial_position_y;

		this.max_height = amount * 162;
		Level_generator(amount);
	}


	public override void _Process(double delta)
	{
		cur_height = player.GlobalPosition.Y-platform_initial_position_y_start;
		if (-cur_height >= max_height && IsInstanceValid(AreaRight)){
			AreaLeft.QueueFree();
			AreaRight.QueueFree();
		}
	}
	private void Level_generator(int amount)
	{
		Random rnd = new Random();
		for (int i = 0; i < amount; i++)
		{	
			int random_dist = rnd.Next(120, 170);
			platform_initial_position_y -= random_dist;
			int temp = rnd.Next(0, 100);
			if (temp <= 70)
			{
				JumpPlatform new_platform = JumpPlatform_scene.Instantiate<JumpPlatform>();
				new_platform.Position = new Vector2(rnd.Next((int)GetNode<Area2D>("PlatformContainer/AreaLeft").Position.X, (int)GetNode<Area2D>("PlatformContainer/AreaRight").Position.X), platform_initial_position_y);
				platformContainer.AddChild(new_platform);
			}
			else if (temp >= 95 && ChestCounter < 1)
			{
				StaticBody2D new_platform = ChestPlatform_scene.Instantiate<StaticBody2D>();
				new_platform.Position = new Vector2(rnd.Next((int)GetNode<Area2D>("PlatformContainer/AreaLeft").Position.X, (int)GetNode<Area2D>("PlatformContainer/AreaRight").Position.X), platform_initial_position_y);
				platformContainer.AddChild(new_platform);
				ChestCounter++;
			}
			else if (temp > 70 && temp < 95)
			{
				BreackablePlatform new_platform = BreackablePlatform_scene.Instantiate<BreackablePlatform>();
				new_platform.Position = new Vector2(rnd.Next((int)GetNode<Area2D>("PlatformContainer/AreaLeft").Position.X, (int)GetNode<Area2D>("PlatformContainer/AreaRight").Position.X), platform_initial_position_y);
				platformContainer.AddChild(new_platform);
			}
		}
	}

	private void _on_platorm_deleter_body_entered(Node2D body)
	{	
		body.QueueFree();
	}

	private void _physics_process(float delta)
	{
		if (IsInstanceValid(player) && IsInstanceValid(AreaRight) && IsInstanceValid(AreaLeft))
		{
			if (player.Position.Y < AreaRight.Position.Y)
			{
				AreaRight.Position = new Vector2(AreaRight.Position.X, player.Position.Y);
				AreaLeft.Position = new Vector2(AreaLeft.Position.X, player.Position.Y);
			}
		}
	}	

}
