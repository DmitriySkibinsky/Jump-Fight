using Godot;
using System;
using System.Runtime.InteropServices;
using System.Threading;

public partial class Barbarian : CharacterBody2D
{

    public enum Statement
    {
        Idle,
        Roam,
        Damaged,
        PrepareAttack,
        FinishAttack,
        Run,
    }
    public static Random RNG = new Random();

    // Настройки
    public int Speed = 75;
    public int Damage = 20;
    public int Health = 100;

    public float Gravity = 200;

    // Нужно для переключений между стейтментами
    public float[] TimeOfState = new float[Enum.GetNames(typeof(Statement)).Length];
    /*public float DamagedTime = 0;
    public float IdleTime = 0;
    public float RoamTime = 0;
    public float PrepareAttackTime = 0;
    public float FinishAttackTime = 0;*/

    // Используются в процессе
    public Statement State = Statement.Idle;
    public bool Alive = true;

    public int Direction = RNG.Next(2) == 1 ? 1 : -1;
    public Vector2 PointOfInterest = new Vector2(0, 0);


    public RayCast2D rayCast2D;
    public AnimatedSprite2D Anim;
    public Area2D HitBoxes;
    public Area2D HurtBoxes;
    public CollisionShape2D HitBox1;
    public CollisionShape2D HurtBox1;

    public player Player;
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

        for(int i = 0; i < TimeOfState.Length; i++)
        {

        }

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

    public void TurnAround()
    {
        if ((Math.Sign(WayPoints[CurrentWayPoint].X - Position.X) == 1) == Anim.FlipH)
        {
            Anim.FlipH = !Anim.FlipH;
            Vector2 Reverse = new Vector2(-1, 1);
            Anim.Position *= Reverse;
            GetNode<CollisionShape2D>("CollisionShape2D").Position *= Reverse;
            HurtBoxes.Position *= Reverse;
            Godot.Collections.Array<Node> hurtboxes = HurtBoxes.GetChildren();
            for (int i = 0; i < hurtboxes.Count; i++)
            {
                ((CollisionShape2D)hurtboxes[i]).Position *= Reverse;
            }
            HitBoxes.Position *= Reverse;
            Godot.Collections.Array<Node> hitboxes = HitBoxes.GetChildren();
            for (int i = 0; i < hitboxes.Count; i++)
            {
                ((CollisionShape2D)hitboxes[i]).Position *= Reverse;
            }
        }
    }

    public bool SecondAttack = false;

    public async void Attack(Node2D Body)
    {
        if (State == Statement.Run && Alive && Body.Name == "HurtBox" && (int)Player.Get("health") > 0)
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
        State = Statement.Idle;
        DamagedTime = 1;
        Anim.Play("Grep");

        if (Health <= 0)
        {
            Death();
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
