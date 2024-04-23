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
        AttackIntermission,
        Run,
    }
    public static Random RNG = new Random();

    // Настройки
    public int Speed = 100;
    public int Damage = 20;
    public int Health = 100;
    public double AttackPrepareTime = 1;
    public double AttackFinishTime = 1;
    public double AttackIntermission = 1;


    public float Gravity = 200;

    // Нужно для переключений между стейтментами
    public double[] TimeOfState = new double[Enum.GetNames(typeof(Statement)).Length];

    // Используются в процессе
    public Statement State = Statement.Idle;
    public bool Alive = true;
    public bool IsPursue = false;

    public int Direction = RNG.Next(2) == 1 ? 1 : -1;


    public RayCast2D rayCast2D;
    public AnimatedSprite2D Anim;
    public Area2D FOV;
    public Area2D AttackTrigger;
    public Area2D HitBoxes;
    public Area2D HurtBoxes;
    public CollisionShape2D HitBox1;
    public CollisionShape2D HurtBox1;

    public player Player;
    public override void _Ready()
    {
        rayCast2D = GetNode<RayCast2D>("RayCast2D");
        Anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        FOV = GetNode<Area2D>("FOV");
        AttackTrigger = GetNode<Area2D>("AttackTrigger");
        HitBoxes = GetNode<Area2D>("HitBoxes");
        HurtBoxes = GetNode<Area2D>("HurtBoxes");
        HitBox1 = GetNode<CollisionShape2D>("HitBoxes/Box1");
        HurtBox1 = GetNode<CollisionShape2D>("HitBoxes/Box1");

        Player = (player)GetTree().GetFirstNodeInGroup("Player");

        TimeOfState[(int)Statement.Idle] = RNG.Next(2, 7);
        State = Statement.Idle;
    }



    public override void _Process(double delta)
    {
        if (!Alive)
        {
            return;
        }

        for (int i = 0; i < TimeOfState.Length; i++)  // Определяем стейтмент
        {
            if (TimeOfState[i] > 0)
            {
                TimeOfState[i] -= delta;
                if (TimeOfState[i] <= 0)
                {
                    switch ((Statement)i)
                    {
                        case Statement.Idle:
                            if (!IsPursue)
                            {
                                TimeOfState[(int)Statement.Roam] = RNG.Next(2, 12);
                                State = Statement.Roam;
                                Direction = RNG.Next(2) == 1 ? 1 : -1;
                                Anim.Play("Run");
                            }
                            break;
                        case Statement.Roam:
                            if (!IsPursue)
                            {
                                TimeOfState[(int)Statement.Idle] = RNG.Next(2, 7);
                                State = Statement.Idle;
                                Anim.Play("Idle");
                            }
                            break;
                        case Statement.Damaged:
                            Anim.Modulate = new Color(1, 1f, 1f);
                            break;
                        case Statement.PrepareAttack:
                            Attack();
                            TimeOfState[(int)Statement.FinishAttack] = AttackFinishTime;
                            State = Statement.FinishAttack;
                            break;
                        case Statement.FinishAttack:
                            TimeOfState[(int)Statement.AttackIntermission] = AttackIntermission;
                            State = Statement.AttackIntermission;
                            Anim.Play("Idle");
                            break;
                        case Statement.AttackIntermission:
                            if (IsPursue)
                            {
                                State = Statement.Run;
                                Anim.Play("Run");
                            }
                            else
                            {
                                TimeOfState[(int)Statement.Roam] = RNG.Next(2, 12);
                                State = Statement.Roam;
                                Direction = RNG.Next(2) == 1 ? 1 : -1;
                                Anim.Play("Run");
                            }
                            break;
                    }
                }
            }
        }

        if (IsPursue)
        {
            Direction = Math.Sign(Player.GlobalPosition.X - GlobalPosition.X);
        }

        Vector2 velocity = Vector2.Zero;

        if (IsOnFloor() && (State == Statement.Run || State == Statement.Roam))
        {
            int CurrentSpeed = Speed;
            if (State == Statement.Roam && (!rayCast2D.IsColliding() || IsOnWall()))
            {
                Direction *= -1;
            }
            else if (State == Statement.Run && (!rayCast2D.IsColliding() || IsOnWall()))
            {
                CurrentSpeed = 0;
            }

            velocity.X += Direction * CurrentSpeed * (float)delta;
        }
        else if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
        }

        MoveAndCollide(velocity);
        MoveAndSlide();

        TurnAround();

        Godot.Collections.Array<Area2D> OverlappingAreas = AttackTrigger.GetOverlappingAreas();
        for (int i = 0; i < OverlappingAreas.Count; i++)
        {
            PrepareAttack(OverlappingAreas[i]);
        }
    }

    public void TurnAround()
    {
        if (Alive && (Direction == 1) == Anim.FlipH)
        {
            Anim.FlipH = !Anim.FlipH;
            Vector2 Reverse = new Vector2(-1, 1);
            Anim.Position *= Reverse;
            GetNode<CollisionShape2D>("CollisionShape2D").Position *= Reverse;
            rayCast2D.Position *= Reverse;
            Godot.Collections.Array<Node> Children = GetChildren();
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is Area2D area2d)
                {
                    area2d.Position *= Reverse;
                    Godot.Collections.Array<Node> CollisionShapes = area2d.GetChildren();
                    for (int j = 0; j < CollisionShapes.Count; j++)
                    {
                        if (CollisionShapes[j] is CollisionShape2D CollisionShape)
                        {
                            CollisionShape.Position *= Reverse;
                        }
                    }
                }
            }
        }
    }

    public bool SecondAttack = false;

    public void Attack()
    {
        Anim.Play("Attack");
        Godot.Collections.Array<Area2D> OverlappingAreas = HitBoxes.GetOverlappingAreas();
        for (int i = 0; i < OverlappingAreas.Count; i++)
        {
            if (Alive && OverlappingAreas[i].Name == "HurtBox" && (int)Player.Get("health") > 0)
            {
                Player.CallDeferred("GetDamaged", Damage);
                break;
            }
        }
    }

    public void PrepareAttack(Node2D Body)
    {
        if (State == Statement.Run && Alive && Body.Name == "HurtBox" && (int)Player.Get("health") > 0)
        {
            TimeOfState[(int)Statement.PrepareAttack] = AttackPrepareTime;
            State = Statement.PrepareAttack;
            Anim.Play("PrepareToAttack");
        }
    }

    public void GetDamage(int Damage)
    {

        Health -= Damage;
        TimeOfState[(int)Statement.Damaged] = 0.1;
        Anim.Modulate = new Color(1, 0.5f, 0.5f);

        if (Health <= 0)
        {
            Death();
        }
        else if (State == Statement.Idle || State == Statement.AttackIntermission)
        {
            Anim.Play("GetDamage");
        }
    }

    public void AnimationFinished()
    {
        if (Anim.Animation == "GetDamage")
        {
            Anim.Play("Idle");
        }
    }

    public void StartPursue(Node2D Body)
    {
        if (Alive && Body.Name == "HurtBox" && (int)Player.Get("health") > 0)
        {
            IsPursue = true;
            if (State == Statement.Roam || State == Statement.Idle)
            {
                State = Statement.Run;
                Anim.Play("Run");
            }
        }
    }

    public void FinishPursue(Node2D Body)
    {
        if (Alive && Body.Name == "HurtBox" && (int)Player.Get("health") > 0)
        {
            IsPursue = false;
            if (State == Statement.Run)
            {
                TimeOfState[(int)Statement.Idle] = RNG.Next(4, 7);
                State = Statement.Idle;
                Anim.Play("Idle");
            }
        }
    }
    public async void Death()
    {
        Alive = false;
        IsPursue = false;
        Anim.Play("Death");
        await ToSignal(GetTree().CreateTimer(0.1), "timeout");
        Anim.Modulate = new Color(1, 1f, 1f);
        await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
        //await ToSignal(GetTree().CreateTimer(3), "timeout");
        QueueFree();
    }
}
