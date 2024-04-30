using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class RangedAttack : Node2D
{

	public Tween tween;
	public ColorRect Indicator;
	public player Player;
	public CollisionShape2D PlayerDetector;
	public AnimationPlayer AnimationPlayer;
	public AudioStreamPlayer SpawnSound;
	public SoundSettings Switcher = SoundSettings.ON;

	public bool IsSpellCast2 = false;
	
	public Node2D Sounds;
	[Export]
	public int Damage = 20;

	public enum SoundSettings
	{
		ON,
		OFF
	}

	public void turn_on()
	{
		foreach(AudioStreamPlayer audio in Sounds.GetChildren()){
			if (IsSpellCast2){
				audio.VolumeDb = -15;
			}else{
				audio.VolumeDb = -10;
			}
		}
	}

	public void turn_off()
	{
		foreach(AudioStreamPlayer audio in Sounds.GetChildren()){
			audio.VolumeDb = -80;
		}
	}

	public override void _Ready()
	{
		Sounds = GetNode<Node2D>("Sounds");
		Indicator = GetNode<ColorRect>("Indicator");
		PlayerDetector = GetNode<CollisionShape2D>("PlayerDetector/CollisionShape2D");
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		Player = (player)GetTree().GetFirstNodeInGroup("Player");
		SpawnSound = GetNode<AudioStreamPlayer>("Sounds/Spawn");

		SpawnSound.Play();
		AnimationPlayer.Play("idle");
	}
    public override void _Process(double delta)
    {
        if (settings.Sound == true)
		{
			Switcher = SoundSettings.ON;
		}
		else
		{
			Switcher = SoundSettings.OFF;
		}

		switch (Switcher)
		{
			case SoundSettings.ON:
				turn_on();
				break;
			case SoundSettings.OFF:
				turn_off();
				break;
		}
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

