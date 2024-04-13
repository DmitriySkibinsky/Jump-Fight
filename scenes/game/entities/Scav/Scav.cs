using Godot;
using System;
using System.Runtime.InteropServices;
using System.Threading;

public partial class Scav : CharacterBody2D
{

    enum Statement
    {
        Run,
        Idle, // Включается после того как он ударил
        Damaged,
    }
    private static Random RNG = new Random();

    private int Speed = 50;
    private int Damage = 20;
    private int Health = 100;

    private Statement State = Statement.Run;
    private bool Alive = true;

    private float AttackTime = 0;
    private float DamagedTime = 0;
    private float IdleTime = 0;

    //private float Gravity = (float)ProjectSettings.GetSetting("physics/2d/deault_gravity");
    private float Gravity = 100;


    private int Direction = RNG.Next(2) == 1 ? 1 : -1;


    private RayCast2D rayCast2D;
    private AnimatedSprite2D Anim;
    private Area2D HitBoxes;
    private Area2D HurtBoxes;
    private CollisionShape2D HitBox1;
    private CollisionShape2D HurtBox1;


    public override void _Ready()
    {
        rayCast2D = GetNode<RayCast2D>("RayCast2D");
        Anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HitBoxes = GetNode<Area2D>("HitBoxes");
        HurtBoxes = GetNode<Area2D>("HurtBoxes");
        HitBox1 = GetNode<CollisionShape2D>("HitBoxes/Box1");
        HurtBox1 = GetNode<CollisionShape2D>("HitBoxes/Box1");

        if (Direction == -1)
        {
            Anim.FlipH = true;

            Vector2 Reverse = new Vector2(-1, 1);
            Anim.Position *= Reverse;
            rayCast2D.Position *= Reverse;
            HitBox1.Position *= Reverse;
        }


        HitBoxes.AreaEntered += Attack;
    }



    public override void _Process(double delta)
    {

        GD.Print("Proc");

        Vector2 velocity = Vector2.Zero;

        if (IsOnFloor() && Alive && State == Statement.Run)
        {
            if (!rayCast2D.IsColliding() || IsOnWall())
            {
                Direction *= -1;
                Vector2 Reverse = new Vector2(-1, 1);
                Anim.Position *= Reverse;
                rayCast2D.Position *= Reverse;
                HitBox1.Position *= Reverse;
            }

            velocity.X += Direction * Speed * (float)delta;
        }
        else
        {
            velocity.Y += Gravity * (float)delta;
        }

        MoveAndCollide(velocity);
        MoveAndSlide();
    }


    private void StateUpdate()
    {

    }



    private async void Attack(Node2D body)
    {
        if (Alive && body.GetParent() != null && body.GetParent() is CharacterBody2D Player && body.GetParent().Name == "Player")
        {
            Anim.Play("Attack1");
            Player.CallDeferred("GetDamage", Damage);
            State = Statement.Idle;
            IdleTime = 2;
            await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
            Anim.Play("Idle");
        }
    }

    private async void GetDamage(int Damage)
    {

        Health -= Damage;
        State = Statement.Damaged;
        IdleTime = 1;
        Anim.Play("Grep");
        await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
        Anim.Play("Idle");

        if (Health <= 0)
        {
            Death();
        }
    }

    private async void Death()
    {
        Alive = false;
        Anim.Play("Death");
        await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
        Thread.Sleep(1);
        QueueFree();
    }
}
