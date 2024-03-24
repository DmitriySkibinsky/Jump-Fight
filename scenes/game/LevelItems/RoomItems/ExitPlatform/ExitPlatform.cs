using Godot;
using System;

public partial class ExitPlatform : Node2D
{
	private AnimatedSprite2D animation;
	private JumpPlatform jumpPlatform;
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animation.Play();
		jumpPlatform = GetNode<JumpPlatform>("JumpPlatform");
	}

	public void _on_animated_sprite_2d_animation_finished(){
		animation.QueueFree();
	}
}
