using Godot;
using System;

public partial class MechaStoneGolem : CharacterBody2D
{
	public const float Speed = 200.0f;
   	[Export]
    public int Damage = 10;
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public player Player;
	public Sprite2D Sprite;
	public Sprite2D MeleeAttack;
	public AnimationPlayer AnimationPlayer;
	public RayCast2D rayCast2D;
	public Area2D HitBox;
	public Area2D HurtBoxes;
	public ColorRect TakeDamageEffect;
	public float Direction;
	public bool Alive = true;
	private float healthPoints = 840;
	public bool HealthToPlayerDroped = false;
	public SoundSettings Switcher = SoundSettings.ON;
	public Node2D Sounds;
	Tween tween;
	public float HealthPoints{get{
        return healthPoints;
    }set{
        healthPoints = value;
        if (healthPoints <= 0){
			Alive = false;
			GetNode<FiniteStateMachine>("FiniteStateMachine").ChangeState("Death");
		}
    }}

	public enum SoundSettings
	{
		ON,
		OFF
	}

	public void turn_on()
	{
		foreach(AudioStreamPlayer audio in Sounds.GetChildren()){
			audio.VolumeDb = -10;
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
		rayCast2D = GetNode<RayCast2D>("RayCast2D");
		Sprite = GetNode<Sprite2D>("Golem");
		MeleeAttack = GetNode<Sprite2D>("MeleeAttack");
		HitBox = GetNode<Area2D>("HitBox");
		HurtBoxes = GetNode<Area2D>("HurtBoxes");
		AnimationPlayer = (AnimationPlayer)GetTree().GetFirstNodeInGroup("AnimationPlayer");
		Player = (player)GetTree().GetFirstNodeInGroup("Player");
		TakeDamageEffect = GetNode<ColorRect>("Golem/TakeDamageEffect");
		Sounds = GetNode<Node2D>("Sounds");
    	SetPhysicsProcess(false);
	   
    }
	public override void _Process(double delta){
		Node2D RocketSpawn = GetNode<Node2D>("FiniteStateMachine/Rocket/RocketSpawn");
		Direction = Player.GlobalPosition.X - this.GlobalPosition.X;
		if (Direction > 0){ 
			Sprite.FlipH = false;
			rayCast2D.Position = new Vector2(40,rayCast2D.Position.Y);
			RocketSpawn.Position = new Vector2(50, RocketSpawn.Position.Y);
			MeleeAttack.FlipV = false;
			MeleeAttack.Position = new Vector2(86, GetNode<Sprite2D>("MeleeAttack").Position.Y);
			HitBox.GetChild<CollisionShape2D>(0).Position = new Vector2(81, HitBox.GetChild<CollisionShape2D>(0).Position.Y);
		}else{
			Sprite.FlipH = true;
			rayCast2D.Position = new Vector2(-40,rayCast2D.Position.Y);
			RocketSpawn.Position = new Vector2(-50, RocketSpawn.Position.Y);
			MeleeAttack.FlipV = true;
			MeleeAttack.Position = new Vector2(-86, GetNode<Sprite2D>("MeleeAttack").Position.Y);
			HitBox.GetChild<CollisionShape2D>(0).Position = new Vector2(-81, HitBox.GetChild<CollisionShape2D>(0).Position.Y);
		}

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

	public Vector2 velocity;
    public override void _PhysicsProcess(double delta)
	{
		velocity = new Vector2();
		if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;
		else{
			velocity.Y *= (float)delta;
		}

		if(Direction > 0){
			velocity.X = Speed * (float)delta;
		}else{
			velocity.X = -Speed * (float)delta;
		}

		velocity = new Vector2(velocity.X, velocity.Y);
		Velocity = velocity;
		MoveAndCollide(Velocity);
		MoveAndSlide();
	}

	public void _on_hit_box_body_entered(Node2D body){
		if (body.Name == "Player"){
			body.CallDeferred("GetDamaged",Damage);
		}
	}

	public void GetDamage (float damage){
		HealthPoints -= damage;
		tween = CreateTween();
		tween.TweenProperty(TakeDamageEffect, "color", new Color(1, 1, 1, (float)0.196), 0.1);
		tween.TweenProperty(TakeDamageEffect, "color", new Color(1, 1, 1, 0), 0.1);
	}

}
