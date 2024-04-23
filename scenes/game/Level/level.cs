using Godot;
using System;
using System.Collections.Generic;

public partial class level : Node2D
{	
	public bool isBattleSection = false;
	public Camera2D camera;
	public player player;
	public List<Node2D> ActivePlatforms = new List<Node2D>();
	public override void _Ready()
	{
		this.player = GetNode<player>("Player");
		this.camera = GetNode<Camera2D>("PlayerCamera");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		
		if (isBattleSection){
			camera.Position = new Vector2(camera.Position.X, player.Position.Y + 20);
		}else if (player.Position.Y < camera.Position.Y){
			camera.Position = new Vector2(camera.Position.X, player.Position.Y);
		}
	}

	public void _on_cleaner_area_body_entered(Node2D body){
		if (body.Name == "Player"){
			player player = (player)body;
			ActivePlatforms.Sort(SortByPlayerDistance);
			for(int i = 0; i < ActivePlatforms.Count; i++){
				if (ActivePlatforms[i].Position.Y < player.Position.Y){
					ActivePlatforms[i].Set("JumpForce", 800);
					player.Position = new Vector2(ActivePlatforms[i].Position.X, ActivePlatforms[i].Position.Y - 80);
					player.Velocity = new Vector2(0, 0);
					break;
				}
			}
			player.GetDamaged(20);
		}else{
			UnregisterPlatform(body);
			body.QueueFree();
		}
	}

	public void _on_button_pressed()
	{
		GetTree().Quit();
	}
	
	public void RegisterPlatform(Node2D platform){
		ActivePlatforms.Add(platform);
	}
	public void UnregisterPlatform(Node2D platform){
		int index = ActivePlatforms.LastIndexOf(platform);
		if (index != -1){
			ActivePlatforms.Remove(platform);
		}
	}
	public int SortByPlayerDistance(Node2D obj1, Node2D obj2)
	{
		double distance1 = player.GlobalPosition.DistanceTo(obj1.GlobalPosition);
		double distance2 = player.GlobalPosition.DistanceTo(obj2.GlobalPosition);
		if (distance1 > distance2)
		{
			return 1; 
		}
		else if (distance1 < distance2)
		{
			return -1;
		}
		else
		{
			return 0;
		}
	}
}

