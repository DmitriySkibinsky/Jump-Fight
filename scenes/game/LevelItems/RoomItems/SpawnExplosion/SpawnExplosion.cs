using Godot;
using System;

public partial class SpawnExplosion : AnimatedSprite2D
{
	public override void _Ready()
	{
		this.Play();
	}

	public void _on_animation_finished(){
		this.QueueFree();
	}
}
