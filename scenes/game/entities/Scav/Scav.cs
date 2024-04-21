using Godot;
using System;
using System.Runtime.InteropServices;
using System.Threading;

public partial class Scav : CharacterBody2D
{

    public enum Statement
    {
        Run,
        Idle, // Включается после того как он ударил
        Damaged,
        Attack,
    }
    private static Random RNG = new Random();

    public int Speed = 50;
    public int Damage = 20;
    public int Health = 100;


    public Statement State = Statement.Run;
    public bool Alive = true;

    public float DamagedTime = 0;
    public float IdleTime = 0;
    public ulong LastDamagedTime = Godot.Time.GetTicksMsec();

    //private float Gravity = (float)ProjectSettings.GetSetting("physics/2d/deault_gravity");
    private float Gravity = 200;


    private int Direction = RNG.Next(2) == 1 ? 1 : -1;


    private RayCast2D rayCast2D;
    public AnimatedSprite2D Anim;
    private Area2D HitBoxes;
    private Area2D HurtBoxes;
    private CollisionShape2D HitBox1;
    private CollisionShape2D HurtBox1;

    private player Player;
    public override void _Ready()
    {
        rayCast2D = GetNode<RayCast2D>("RayCast2D");
        Anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HitBoxes = GetNode<Area2D>("HitBoxes");
        HurtBoxes = GetNode<Area2D>("HurtBoxes");
        HitBox1 = GetNode<CollisionShape2D>("HitBoxes/Box1");
        HurtBox1 = GetNode<CollisionShape2D>("HitBoxes/Box1");

        Player = (player)GetTree().GetFirstNodeInGroup("Player");

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

        //GD.Print("Proc");

        if (DamagedTime > 0 && Alive)
        {
            DamagedTime -= (float)delta;
            if (DamagedTime <= 0 && State != Statement.Attack)
            {
                if(IdleTime > 0)
                {
                    Anim.Play("Idle");
                }
                else
                {
                    Anim.Play("Run");
                    State = Statement.Run;
                }
            }
        }

        if (IdleTime > 0 && Alive)
        {
            IdleTime -= (float)delta;
            if (IdleTime <= 0 && State != Statement.Attack)
            {
                if (DamagedTime > 0)
                {
                    Anim.Play("Grep");
                }
                else
                {
                    Anim.Play("Run");
                    State = Statement.Run;
                }
            }
        }

        if (Anim.Modulate == new Color(1, 0.5f, 0.5f) &&  Godot.Time.GetTicksMsec() - LastDamagedTime > 100) //Если в последний раз урон проходил 100 милисекунд назад, то убираем красный цвет
        {
            Anim.Modulate = new Color(1, 1, 1);
        }

        Vector2 velocity = Vector2.Zero;

        if (IsOnFloor() && Alive && State == Statement.Run)
        {
            if (!rayCast2D.IsColliding() || IsOnWall())
            {
                Anim.FlipH = !Anim.FlipH;
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

        Godot.Collections.Array<Area2D> OverlappingBodies = HitBoxes.GetOverlappingAreas();
        for (int i = 0; i < OverlappingBodies.Count; i++) {
            Attack(OverlappingBodies[i]);
        }
    }

    private bool SecondAttack = false;

    public async void Attack(Node2D body)
    {
        if (State == Statement.Run && Alive && body.Name == "HurtBox" && (int)Player.Get("health") > 0)
        {
            if (SecondAttack)
            {
                Anim.Play("Attack2");
            }
            else
            {
                Anim.Play("Attack1");
            }
            SecondAttack = !SecondAttack;
            Player.CallDeferred("GetDamaged", Damage);
            State = Statement.Idle;
            IdleTime = 2;
            await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
            if (DamagedTime > 0)
            {
                Anim.Play("Grep");
            }
            else if(IdleTime > 0)
            {
                Anim.Play("Idle");
            }
        }
    }

    public void GetDamage(int Damage)
    {

        Health -= Damage;
        if (Health <= 0)
        {
            Death();
        }
        else
        {
            State = Statement.Idle;
            DamagedTime = 1;
            Anim.Play("Grep");
            LastDamagedTime = Godot.Time.GetTicksMsec();
            Anim.Modulate = new Color(1, 0.5f, 0.5f);
        }
    }

    public async void Death()
    {
        Alive = false;
        Anim.Play("Death");
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        QueueFree();
    }
}
