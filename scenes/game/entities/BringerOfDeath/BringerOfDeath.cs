using Godot;
using System;

public partial class BringerOfDeath : CharacterBody2D
{
	public const float Speed = 200.0f;
   	[Export]
    public int Damage = 10;
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public player Player;
	public Sprite2D Sprite;
	public AnimationPlayer AnimationPlayer;
	public RayCast2D rayCast2D;
	public Area2D HitBox;
	public Area2D HurtBoxes;
	public float Direction;
	private float healthPoints = 500;
	public bool HealthToPlayerDroped = false;
	public PackedScene HealthToPlayer = GD.Load<PackedScene>("res://scenes/game/entities/Collectibles/Heart/heart.tscn");
	public float HealthPoints{get{
        return healthPoints;
    }set{
        healthPoints = value;
        if (healthPoints <= 0){
			GetNode<FiniteStateMachine>("FiniteStateMachine").ChangeState("Death");
		}
		if(healthPoints <= 250 && !HealthToPlayerDroped){
			Node2D healthToPlayer = HealthToPlayer.Instantiate<Node2D>();
			healthToPlayer.GlobalPosition = GlobalPosition;
			Owner.AddChild(healthToPlayer);
			HealthToPlayerDroped = true;
		}
    }}


    public override void _Ready()
    {
		rayCast2D = GetNode<RayCast2D>("RayCast2D");
		Sprite = GetNode<Sprite2D>("Sprite2D");
		HitBox = GetNode<Area2D>("HitBox");
		HurtBoxes = GetNode<Area2D>("HurtBoxes");
		AnimationPlayer = (AnimationPlayer)GetTree().GetFirstNodeInGroup("AnimationPlayer");
		Player = (player)GetTree().GetFirstNodeInGroup("Player");
    	SetPhysicsProcess(false);
	   
    }
	public override void _Process(double delta){
		Direction = Player.GlobalPosition.X - this.GlobalPosition.X;
		if (Direction > 0){ 
			Sprite.FlipH = true;
			Sprite.Position = new Vector2(70,Sprite.Position.Y);
			rayCast2D.Position = new Vector2(25,rayCast2D.Position.Y);
			HitBox.GetChild<CollisionShape2D>(0).Position = new Vector2(142, HitBox.GetChild<CollisionShape2D>(0).Position.Y);
		}else{
			Sprite.FlipH = false;
			Sprite.Position = new Vector2(-70,Sprite.Position.Y);
			rayCast2D.Position = new Vector2(-25,rayCast2D.Position.Y);
			HitBox.GetChild<CollisionShape2D>(0).Position = new Vector2(-142, HitBox.GetChild<CollisionShape2D>(0).Position.Y);
		}
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = new Vector2();
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

	public void Teleport(float x){
		GlobalPosition = new Vector2(x,GlobalPosition.Y);
	}

	public void _on_hit_box_body_entered(Node2D body){
		if (body.Name == "Player"){
			body.CallDeferred("GetDamaged",Damage);
		}
	}

	public void GetDamage (float damage){
		HealthPoints -= damage;
		AnimationPlayer.Play("hurt");
		GD.Print(damage);
	}

}
