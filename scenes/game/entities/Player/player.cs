using Godot;
using Godot.NativeInterop;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;

public partial class player : CharacterBody2D
{

	[Signal]
	public delegate void HealthChangedEventHandler(int new_health);

	[Signal]
	public delegate void SuperReloadEventHandler(int new_super);

	public enum StateMachine
	{
		MOVE,
		DAMAGE,
		ATTACK,
		ATTACK2,
		ATTACK3,
		SUPER,
		BLOCK,
		DEATH
	}

	public enum SoundSettings
	{
		ON,
		OFF
	}

	public StateMachine State = StateMachine.MOVE;

	public SoundSettings Switcher = SoundSettings.ON;

	public Node2D level;

	public CollisionShape2D hitbox;

	Vector2 player_pos;

	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public const float speed = 120.0f;

	public float run_speed = 1.0f;

	public const float jump_velocity = -450f;

	public float jump_multiplier = 1f;

	public int health = 100;

	public bool combo = false;

	public bool attack_cooldown = false;

	public float attack_cooldown_multiplier = 1f;

	public bool super_cooldown = false;

	public float damage_basic = 10;

	public float damage_multiplier = 1;

	public float damage_current;

	public AnimationPlayer animPlayer; 

	public Vector2 velocity = new Vector2();

	public AudioStreamPlayer smack;

	public AudioStreamPlayer hurt;

	public AudioStreamPlayer steps;

	public AudioStreamPlayer run_steps;

	public AudioStreamPlayer swings;

   public  AudioStreamPlayer super_attack;

	public AudioStreamPlayer death;

	public AudioStreamPlayer jump;

	public AudioStreamPlayer collect;

	public AudioStreamPlayer collect2;

	public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		smack = GetNode<AudioStreamPlayer>("Sounds/Smack");
		hurt = GetNode<AudioStreamPlayer>("Sounds/Hurt");
		steps = GetNode<AudioStreamPlayer>("Sounds/Steps");
		run_steps = GetNode<AudioStreamPlayer>("Sounds/Run steps");
		swings = GetNode<AudioStreamPlayer>("Sounds/Swings");
		super_attack = GetNode<AudioStreamPlayer>("Sounds/Super attack");
		death = GetNode<AudioStreamPlayer>("Sounds/Death");
		jump = GetNode<AudioStreamPlayer>("Sounds/Jump");
		collect = GetNode<AudioStreamPlayer>("Sounds/Collect");
		collect2 = GetNode<AudioStreamPlayer>("Sounds/Collect2");
		hitbox = GetNode<CollisionShape2D>("AttackDirection/DamageBox/HitBox/CollisionShape2D");
	}
	public override void _PhysicsProcess(double delta)
	{

		velocity = Velocity;
		switch (State)
		{
			case StateMachine.MOVE:
				move_state();
				break;
			case StateMachine.ATTACK:
				attack_state();
				break;
			case StateMachine.ATTACK2:
				attack2_state();
				break;
			case StateMachine.ATTACK3:
				attack3_state();
				break;
			case StateMachine.SUPER:
				super_state();
				break;
			case StateMachine.DAMAGE:
				damage_state();
				break;
			case StateMachine.DEATH:
				death_state();
				break;
			case StateMachine.BLOCK:
				block_state();
				break;
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

		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		if (health <= 0)
		{
			health = 0;
			State = StateMachine.DEATH;
		}

		Velocity = velocity;

		damage_current = damage_basic * damage_multiplier;

		MoveAndSlide();

	}

	public AnimatedSprite2D anim;
	public Node2D attackDirection;
	public void move_state()
	{
		level = GetNode<Node2D>("../");

		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		attackDirection = GetNode<Node2D>("AttackDirection");

		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		if ((bool)level.Get("isBattleSection"))
		{
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * speed * run_speed;
				if (direction.X == 1)
				{
					anim.FlipH = false;
					attackDirection.RotationDegrees = 0;
				}
				else
				{
					anim.FlipH = true;
					attackDirection.RotationDegrees = 180;
				}
				if (velocity.Y == 0)
				{
					if (run_speed == 1)
					{
						animPlayer.Play("Walk");
					}
					else
					{
						animPlayer.Play("Run");
					}

				}
				if (Input.IsActionPressed("run"))
				{
					run_speed = 2;
				}
				else
				{
					run_speed = 1;
				}
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
				if (velocity.Y == 0)
				{
					animPlayer.Play("Idle");
				}
			}
			if (Input.IsActionJustPressed("jump") && IsOnFloor())
			{
				velocity.Y = jump_velocity * jump_multiplier;
				animPlayer.Play("Jump");
			}

			if (velocity.Y > 0)
			{
				animPlayer.Play("Fall");
			}
			if (Input.IsActionJustPressed("attack") && !attack_cooldown && IsOnFloor())
			{
				State = StateMachine.ATTACK;
			}
			if (Input.IsActionPressed("block") && IsOnFloor())
			{
				State = StateMachine.BLOCK;
			}
			if (Input.IsActionJustPressed("super") && !super_cooldown && IsOnFloor())
			{
				State = StateMachine.SUPER;
			}
		}
		else
		{
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * speed * run_speed;
				if (direction.X == 1)
				{
					anim.FlipH = false;
				}
				else
				{
					anim.FlipH = true;
				}
				if (velocity.Y == 0)
				{
					if (run_speed == 1)
					{
						animPlayer.Play("Walk");
					}
					else
					{
						animPlayer.Play("Run");
					}

				}
				run_speed = 2;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
				if (velocity.Y == 0)
				{
					animPlayer.Play("Idle");
				}
			}
			if (velocity.Y > 0)
			{
				animPlayer.Play("Fall");
			}
		}

		if (settings.Sound == true)
		{
			Switcher = SoundSettings.ON;
		}
		else
		{
			Switcher = SoundSettings.OFF;
		}
		
		MoveAndSlide();
	}

	public async void attack_state()
	{
		damage_multiplier = 1;
		if (Input.IsActionJustPressed("attack") && combo)
		{
			State = StateMachine.ATTACK2;
		}
		velocity = Velocity;
		velocity.X = 0;
		if (velocity.Y == 0)
		{
			animPlayer.Play("Attack");
			Velocity = velocity;
			await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
			attack_freeze();
			State = StateMachine.MOVE;
			MoveAndSlide();
		}
	}

	public async void attack2_state()
	{
		damage_multiplier = 1.25f;
		if (Input.IsActionJustPressed("attack") && combo)
		{
			State = StateMachine.ATTACK3;
		}
		animPlayer.Play("Attack2");
		await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
		attack_freeze();
		State = StateMachine.MOVE;
	}

	public async void attack3_state()
	{
		damage_multiplier = 2;
		animPlayer.Play("Attack3");
		await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
		State = StateMachine.MOVE;
	}

	public async void super_state()
	{
		if (velocity.Y == 0)
		{
			damage_multiplier = 3;
			velocity = Velocity;
			velocity.X = 0;
			animPlayer.Play("Super_Attack");
			await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
			super_freeze();
			State = StateMachine.MOVE;
		}
	}

	public async void death_state()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animPlayer.Play("Death");
		await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
		QueueFree();
		GetTree().ChangeSceneToFile("res://scenes/Menu/lose_screen.tscn");
	}

	public async void damage_state()
	{
		if ((bool)level.Get("isBattleSection"))
		{
			animPlayer.Play("Damage");
			await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
		}
		else
		{
			animPlayer.Play("Fall_Damage");
			await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
		}
		State = StateMachine.MOVE;
	}
	public void block_state()
	{
		velocity = Velocity;
		velocity.X = 0;
		animPlayer.Play("Block");
		if (Input.IsActionJustReleased("block"))
		{
			State = StateMachine.MOVE;
		}
		MoveAndSlide();
	}

	public async void combat()
	{
		combo = true;
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
		combo = false;
	}

	public void GetDamaged(int Damage)
	{
		hitbox.Disabled = true;
		smack.Play();
	   if (Input.IsActionPressed("block") && (bool)level.Get("isBattleSection"))
		{
			State = StateMachine.BLOCK;
			health -= Damage / 2;
			EmitSignal(SignalName.HealthChanged, health);
		}
		else
		{
			State = StateMachine.DAMAGE;
			health -= Damage;
			EmitSignal(SignalName.HealthChanged, health);
		}
	}

	public async void attack_freeze()
	{
		attack_cooldown = true;
		await ToSignal(GetTree().CreateTimer(0.5f * attack_cooldown_multiplier), SceneTreeTimer.SignalName.Timeout);
		attack_cooldown = false;
	}

	public async void super_freeze()
	{
		EmitSignal(SignalName.SuperReload, 0);
		super_cooldown = true;
		await ToSignal(GetTree().CreateTimer(30), SceneTreeTimer.SignalName.Timeout);
		super_cooldown = false;
		EmitSignal(SignalName.SuperReload, 100);
	}

	public ulong LastAttack = Godot.Time.GetTicksMsec();
	public void DoDamage(Area2D Area)
	{
		if (Area.Name == "HurtBoxes" && Godot.Time.GetTicksMsec() - LastAttack > 200 && (bool)Area.GetParent().Get("Alive"))
		{
			LastAttack = Godot.Time.GetTicksMsec();
			Area.GetParent().CallDeferred("GetDamage", damage_current);
		}
	}

	public void turn_on()
	{
		smack.VolumeDb = -10;
		hurt.VolumeDb = -10;
		steps.VolumeDb = -10;
		run_steps.VolumeDb = -10;
		swings.VolumeDb = -10;
		super_attack.VolumeDb = -10;
		death.VolumeDb = -10;
		jump.VolumeDb = -10;
		collect.VolumeDb = -10;
		collect2.VolumeDb = -10;
	}

	public void turn_off()
	{
		smack.VolumeDb = -80;
		hurt.VolumeDb = -80;
		steps.VolumeDb = -80;
		run_steps.VolumeDb = -80;
		swings.VolumeDb = -80;
		super_attack.VolumeDb = -80;
		death.VolumeDb = -80;
		jump.VolumeDb = -80;
		collect.VolumeDb = -80;
		collect2.VolumeDb = -80;
	}

    public void heal(int hp, heart HealingOrb=null)
    {
        collect.Play();
        health += hp;
        if (health > 100)
        {
            health = 100;
        }
        EmitSignal(SignalName.HealthChanged, health);
        if (HealingOrb != null){
            HealingOrb.Exit();
        }
    }

	public async void jump_boost(jump_boost boost = null)
	{
		if (boost != null){
            boost.Exit();
        }
		collect2.Play();
		jump_multiplier = 1.2f;
		await ToSignal(GetTree().CreateTimer(10), SceneTreeTimer.SignalName.Timeout);
		jump_multiplier = 1f;
	}

	public async void attack_boost(attack_boost boost = null)
	{
		if (boost != null){
            boost.Exit();
        }
		collect2.Play();
		damage_basic = 20;
		await ToSignal(GetTree().CreateTimer(15), SceneTreeTimer.SignalName.Timeout);
		damage_basic = 10;
	}

	public async void reload_boost(reload_boost boost = null)
	{
        if (boost != null)
        {
            boost.Exit();
        }
        collect2.Play();
		attack_cooldown_multiplier = 0.2f;
        await ToSignal(GetTree().CreateTimer(15), SceneTreeTimer.SignalName.Timeout);
		attack_cooldown_multiplier = 1f;
    }

	public void teleport_to(float target_posX)
	{
		Vector2 NewPos = new Vector2(target_posX, GlobalPosition.Y);
		GlobalPosition = NewPos;
	}
}
