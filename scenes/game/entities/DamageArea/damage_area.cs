using Godot;
using System;

public partial class damage_area : Area2D
{
	public bool damaged = false;
	public int damage = 0;
	public override void _Ready()
	{
		var detector = GetNode<Area2D>(".");
		detector.BodyEntered += OnDetectorBodyEntered;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void OnDetectorBodyEntered(Node2D Body)
	{
		if (Body.Name == "Enemy")
		{
			damage = 100;
			damaged = false;
		}
	}

}
