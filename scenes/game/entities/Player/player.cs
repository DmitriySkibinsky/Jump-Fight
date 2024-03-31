using Godot;
using Godot.NativeInterop;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;

public partial class player : CharacterBody2D
{

    [Signal]
    public delegate void HealthChangedEventHandler(int new_health);

    enum StateMachine
    {
        MOVE,
        DAMAGE,
        ATTACK,
        ATTACK2,
        ATTACK3,
        BLOCK,
        DEATH
    }

    StateMachine State = StateMachine.MOVE;

    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public const float speed = 100.0f;

    public const float jump_velocity = -250f;

    public float run_speed = 1.0f;

    public int health = 100;

    public bool combo = false;

    public bool attack_cooldown = false;

     public Vector2 velocity = new Vector2();

    public override void _PhysicsProcess(double delta)
    {
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

        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

        if (health <= 0)
        {
            health = 0;
            State = StateMachine.DEATH;
        }

        Velocity = velocity;

        MoveAndSlide();
    }
    public void move_state()
    {
        var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        var animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
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
            velocity.Y = jump_velocity;
            animPlayer.Play("Jump");
        }

        if (velocity.Y > 0)
        {
            animPlayer.Play("Fall");
        }
        if (Input.IsActionJustPressed("attack") && !attack_cooldown)
        {
            State = StateMachine.ATTACK;
        }
        if (Input.IsActionPressed("block"))
        {
            State = StateMachine.BLOCK;
        }
        MoveAndSlide();
    }

    public async void attack_state()
    {
        if (Input.IsActionJustPressed("attack") && combo)
        {
            State = StateMachine.ATTACK2;
        }
        velocity = Velocity;
        velocity.X = 0;
        if (velocity.Y == 0)
        {
            var animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
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
        if (Input.IsActionJustPressed("attack") && combo)
        {
            State = StateMachine.ATTACK3;
        }
        var animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("Attack2");
        await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
        State = StateMachine.MOVE;
    }

    public async void attack3_state()
    {
        var animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("Attack3");
        await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
        State = StateMachine.MOVE;
    }

    public async void death_state()
    {
        var animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("Death");
        await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
        QueueFree();
        GetTree().ChangeSceneToFile("res://scenes/Menu/menu.tscn");
    }

    public async void damage_state()
    {
        var animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("Damage");
        await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
        State = StateMachine.MOVE;
    }
    public void block_state()
    {
        velocity = Velocity;
        velocity.X = 0;
        var animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
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
        var animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        await ToSignal(animPlayer, AnimationPlayer.SignalName.AnimationFinished);
        combo = false;
    }

    public void GetDamaged(int Damage)
    {
        if (Input.IsActionPressed("block"))
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
        await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
        attack_cooldown = false;
    }

    public void teleport_to(float target_posX)
    {
        Vector2 NewPos = new Vector2(target_posX, GlobalPosition.Y);
        GlobalPosition = NewPos;
    }
}
