using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class BreackablePlatform : JumpPlatform
{
	private AnimatedSprite2D Explosion;
	private AnimatedSprite2D Platform;
	public AudioStreamPlayer breaker;
	public override void _Ready()
	{
		Explosion = GetNode<AnimatedSprite2D>("Explosion");
		Platform = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Platform.Play();
		breaker = GetNode<AudioStreamPlayer>("Break");
	}

	public override void _Process(double delta)
	{
	}
	

	public override async void _on_area_2d_body_entered(Node2D body){
		if (body.Name == "Player"){
			player Player = (player)body;
			if (Player.Velocity.Y >= 0){
				Player.Velocity = new Vector2(Player.Velocity.X, -JumpForce);
				Platform.Hide();
				Explosion.Play();
                Player.GetNode<AnimationPlayer>("AnimationPlayer").Play("Jump");
                if (settings.Sound)
                {
                    breaker.Play();
                }
                await ToSignal(Player.GetNode<AnimationPlayer>("AnimationPlayer"), AnimationPlayer.SignalName.AnimationFinished);
                Player.MoveAndSlide();
			}
		}
	}

	public void _on_explosion_animation_finished(){
        GetParent().Owner.CallDeferred("UnregisterPlatform", this);
		this.QueueFree();
	}
}
