using Godot;
using System;
using System.Threading.Tasks;

public partial class LaserBeam : Node2D
{
	public AnimationPlayer AnimPlayer;
	public ColorRect LaserIndicator;
	public CollisionShape2D HitBox;
	public player Player;
	public Node2D Sounds;
	public SoundSettings Switcher = SoundSettings.ON;

	public enum SoundSettings
	{
		ON,
		OFF
	}


	public void turn_on()
	{
		foreach(AudioStreamPlayer audio in Sounds.GetChildren()){
				audio.VolumeDb = -22;
		}
	}

	public void turn_off()
	{
		foreach(AudioStreamPlayer audio in Sounds.GetChildren()){
			audio.VolumeDb = -80;
		}
	}

	public int Damage = 10;
	
    public override void _Ready()
    {
		Sounds = GetNode<Node2D>("Sounds");
        AnimPlayer = GetNode<AnimationPlayer>("AnimPlayer");
		LaserIndicator = GetNode<ColorRect>("LaserIndicator");
		HitBox = GetNode<CollisionShape2D>("HitBox/CollisionShape2D");
		Player = (player)GetTree().GetFirstNodeInGroup("Player");
    }
    public async void Prepare(){
		Tween tween = CreateTween();
		tween.TweenProperty(LaserIndicator, "color", new Color((float)0.263, 1, 1, (float)0.235), 0.5);
		AnimPlayer.Play("laser_prepare");
		await ToSignal(AnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}

	public async void Attack(){
		Tween tween = CreateTween();
		tween.TweenProperty(LaserIndicator, "color", new Color((float)0.263, 1, 1, 0), 0.1);
		AnimPlayer.Play("laser_attack");
		HitBox.Disabled = false;
		await ToSignal(AnimPlayer, AnimationPlayer.SignalName.AnimationFinished);
		HitBox.Disabled = true;
	}

	public void _on_hit_box_body_entered(Node2D body){
		if (body == Player){
			Player.GetDamaged(Damage);
		}
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
}
