using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class HomingMissile : Area2D
{
    public float speed = 325;
    public float steerForce = 75.0f;

	public bool IsHoming = true;
    public player Player;
    public int Damage = 10;
    public Vector2 velocity = Vector2.Zero;
    public Vector2 acceleration = Vector2.Zero;
    public Node2D target;
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
			audio.VolumeDb = -10;
		}
	}

	public void turn_off()
	{
		foreach(AudioStreamPlayer audio in Sounds.GetChildren()){
			audio.VolumeDb = -80;
		}
	}
	public async override void _Ready(){
        Sounds = GetNode<Node2D>("Sounds");
		target = (Node2D)GetTree().GetFirstNodeInGroup("Player");
		GetNode<AnimationPlayer>("AnimationPlayer").Play("idle");
        Player = (player)GetTree().GetFirstNodeInGroup("Player");
        
		await ToSignal(GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);
		IsHoming = false;
		speed = 750;
	}
    public void Start(Transform2D _transform, Node2D _target)
    {
        GlobalTransform = _transform;
        Rotation += (float)GD.RandRange(-0.09, 0.09);
        velocity = Transform.X * speed;
        target = _target;
    }

    private Vector2 Seek()
    {
        Vector2 steer = Vector2.Zero;
        if (target != null)
        {
            Vector2 desired = (target.GlobalPosition - GlobalPosition).Normalized() * speed;
            steer = (desired - velocity).Normalized() * steerForce;
        }
        return steer;
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
    public override void _PhysicsProcess(double delta)
    {
		if (IsHoming){
			acceleration += Seek();
        	velocity += acceleration * (float)delta;
		}else{
			velocity = velocity.Normalized() * speed;
		}
        
        velocity = velocity.LimitLength(speed);
        Rotation = velocity.Angle();
        Position += velocity * (float)delta;
    }

    public void _on_body_entered(Node2D body)
    {
		if (body == Player){
			Explode();
			Player.GetDamaged(Damage);
		}else if(body.Name != "MechaStoneGolem"){
			Explode();
		}
    }

    public void OnLifetimeTimeout()
    {
        Explode();
    }

    public async void Explode()
    {
        SetPhysicsProcess(false);
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		GetNode<Sprite2D>("Missile").Hide();
        animationPlayer.Play("explode");
        await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		QueueFree();
    }
}