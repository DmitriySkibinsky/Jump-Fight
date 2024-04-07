using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class BreackablePlatform : JumpPlatform
{
	private AnimatedSprite2D Explosion;
	private AnimatedSprite2D Platform;
	public override void _Ready()
	{
		Explosion = GetNode<AnimatedSprite2D>("Explosion");
		Platform = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Platform.Play();
	}

	public override void _Process(double delta)
	{
	}
	

	public override async void _on_area_2d_body_entered(Node2D body){
		if (body.Name == "Player"){
			player Player = (player)body;
			if (Player.Velocity.Y >= 0){
				Player.Velocity = new Vector2(Player.Velocity.X, -JumpForce);
                Player.GetNode<AnimationPlayer>("AnimationPlayer").Play("Jump");
                await ToSignal(Player.GetNode<AnimationPlayer>("AnimationPlayer"), AnimationPlayer.SignalName.AnimationFinished);
                Player.MoveAndSlide();
				Platform.Hide();
				Explosion.Play();
			}
		}
	}

	public void _on_explosion_animation_finished(){
		GetParent().Owner.CallDeferred("UnregisterPlatform", this);
		this.QueueFree();
	}
}
