using Godot;
using System;

public partial class RangedAttack : Node2D
{

	public Tween tween;
	public ColorRect Indicator;
	public player Player;
	public CollisionShape2D PlayerDetector;
	public AnimationPlayer AnimationPlayer;

	[Export]
	public int Damage = 20;
	public override void _Ready()
	{
		Indicator = GetNode<ColorRect>("Indicator");
		PlayerDetector = GetNode<CollisionShape2D>("PlayerDetector/CollisionShape2D");
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		Player = (player)GetTree().GetFirstNodeInGroup("Player");

		AnimationPlayer.Play("idle");
	}

	public async void TurnAttack(){
		tween = CreateTween();
		tween.TweenProperty(Indicator, "color", new Color((float)0.745, (float)0.553, (float)0.984, (float)0.745), 0.4);
		tween.TweenProperty(Indicator, "color", new Color((float)0.643, (float)0.333, (float)0.953, 0), 0.4);
		await ToSignal(GetTree().CreateTimer(0.8), SceneTreeTimer.SignalName.Timeout);

		AnimationPlayer.Play("attack");
		PlayerDetector.Set("disabled", false);

		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);

		PlayerDetector.Set("disabled", true);
		AnimationPlayer.Play("idle");
	}

	public void _on_player_detector_body_entered(Node2D Body){
		if (Body == Player){
			Player.GetDamaged(Damage);
		}
	}
}

