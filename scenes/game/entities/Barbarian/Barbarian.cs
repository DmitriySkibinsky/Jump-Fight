using Godot;
using Microsoft.VisualBasic;
using System;
using System.IO;
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

    public enum SoundSettings
    {
        ON,
        OFF
    }

    public static Random RNG = new Random();

    // Настройки
    public float Speed = 120;
    public int Damage = 20;
    public int Health = 1;//100;
    public double AttackPrepareTime = 1;
    public double AttackFinishTime = 1;
    public double AttackIntermission = 1;


    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    // Нужно для переключений между стейтментами
    public double[] TimeOfState = new double[Enum.GetNames(typeof(Statement)).Length];

    // Используются в процессе
    public Statement State;
    public bool Alive = true;
    public bool IsPursue = false;

    public int Direction = RNG.Next(2) == 1 ? 1 : -1;


    public CollisionShape2D CollisionShape;
    public RayCast2D rayCast2D;
    public AnimatedSprite2D Anim;
    public Area2D FOV;
    public Area2D AttackTrigger;
    public Area2D HitBoxes;
    public Area2D HurtBoxes;

    public Node2D ForwardWallDetectors;
    public RayCast2D ForwardWallDetector_1;
    public RayCast2D ForwardWallDetector_2;

    public Node2D Sounds;
    public AudioStreamPlayer2D Sound_Attack;
    public AudioStreamPlayer2D Sound_Hit;
    public AudioStreamPlayer2D Sound_Hurt;
    public AudioStreamPlayer2D Sound_Death;

    public float DeffaultVolume_Sound_Attack;
    public float DeffaultVolume_Sound_Hit;
    public float DeffaultVolume_Sound_Hurt;
    public float DeffaultVolume_Sound_Death;

    public player Player;
    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        rayCast2D = GetNode<RayCast2D>("RayCast2D");
        Anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        FOV = GetNode<Area2D>("FOV");
        AttackTrigger = GetNode<Area2D>("AttackTrigger");
        HitBoxes = GetNode<Area2D>("HitBoxes");
        HurtBoxes = GetNode<Area2D>("HurtBoxes");

        ForwardWallDetectors = GetNode<Node2D>("ForwardWallDetectors");
        ForwardWallDetector_1 = ForwardWallDetectors.GetNode<RayCast2D>("ForwardWallDetector_1");
        ForwardWallDetector_2 = ForwardWallDetectors.GetNode<RayCast2D>("ForwardWallDetector_2");

        //Звуки
        Sounds = GetNode<Node2D>("Sounds");
        Sound_Attack = Sounds.GetNode<AudioStreamPlayer2D>("Attack");
        Sound_Hit = Sounds.GetNode<AudioStreamPlayer2D>("Hit");
        Sound_Hurt = Sounds.GetNode<AudioStreamPlayer2D>("Hurt");
        Sound_Death = Sounds.GetNode<AudioStreamPlayer2D>("Death");

        DeffaultVolume_Sound_Attack = Sound_Attack.VolumeDb;
        DeffaultVolume_Sound_Hit = Sound_Hit.VolumeDb;
        DeffaultVolume_Sound_Hurt = Sound_Hurt.VolumeDb;
        DeffaultVolume_Sound_Death = Sound_Death.VolumeDb;
        //

        Player = (player)GetTree().GetFirstNodeInGroup("Player");

        TimeOfState[(int)Statement.Idle] = RNG.Next(2, 7);
        State = Statement.Idle;
    }



    public override void _Process(double delta)
    {

        if (TimeOfState[(int)Statement.Damaged] > 0)
        {
            TimeOfState[(int)Statement.Damaged] -= delta;
            if (TimeOfState[(int)Statement.Damaged] <= 0)
            {
                Anim.Modulate = new Color(1, 1f, 1f);
            }
        }

        if (!Alive)
        {
            return;
        }

        for (int i = 0; i < TimeOfState.Length; i++)  // Определяем стейтмент
        {
            if (TimeOfState[i] > 0 && i != (int)Statement.Damaged)
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
            float CurrentSpeed = Speed;
            if (State == Statement.Roam && (!rayCast2D.IsColliding() || ForwardWallDetector_1.IsColliding() || ForwardWallDetector_2.IsColliding()))
            {
                Direction *= -1;
            }
            else if (State == Statement.Run && (!rayCast2D.IsColliding() || ForwardWallDetector_1.IsColliding() || ForwardWallDetector_2.IsColliding()))
            {
                Anim.Play("Idle");
                CurrentSpeed = 0;
            }
            else if (State == Statement.Run)
            {
                Anim.Play("Run");
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

    Vector2 Reverse = new Vector2(-1, 1);
    public void TurnAroundElements(Node2D Obj)
    {
        Obj.Position *= Reverse;
        if (Obj is RayCast2D rayCast2D)
        {
            rayCast2D.TargetPosition *= Reverse;
        }
        Godot.Collections.Array<Node> Children = Obj.GetChildren();
        for (int i = 0; i < Children.Count; i++)
        {
            if (Children[i] is Node2D node2d)
            {
                TurnAroundElements(node2d);
            }
        }
    }

    public void TurnAround()
    {
        if (Alive && (Direction == 1) == Anim.FlipH)
        {
            Anim.FlipH = !Anim.FlipH;
            Godot.Collections.Array<Node> Children = GetChildren();
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is Node2D node2d)
                {
                    TurnAroundElements(node2d);
                }
            }
        }
    }

    public void Attack()
    {
        Anim.Play("Attack");
        Sound_Attack.Play();
        Godot.Collections.Array<Area2D> OverlappingAreas = HitBoxes.GetOverlappingAreas();
        for (int i = 0; i < OverlappingAreas.Count; i++)
        {
            if (Alive && OverlappingAreas[i].Name == "HurtBox" && Player.health > 0)
            {
                Sound_Hit.Play();
                Player.CallDeferred("GetDamaged", Damage);
                break;
            }
        }
    }

    public void PrepareAttack(Node2D Area)
    {
        if (State == Statement.Run && Alive && Area.Name == "HurtBox" && Player.health > 0)
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
        Sound_Hurt.Play();

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

    public void StartPursue(Node2D Area)
    {
        if (Alive && Area.Name == "HurtBox" && Player.health > 0)
        {
            IsPursue = true;
            if (State == Statement.Roam || State == Statement.Idle)
            {
                State = Statement.Run;
                Anim.Play("Run");
            }
        }
    }

    public void FinishPursue(Node2D Area)
    {
        if (Alive && Area.Name == "HurtBox" && Player.health > 0)
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
        Sound_Death.Play();
        await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
        //await ToSignal(GetTree().CreateTimer(3), "timeout");
        QueueFree();
    }

    public void turn_on()
    {
        Sound_Attack.VolumeDb = DeffaultVolume_Sound_Attack;
        Sound_Hit.VolumeDb = DeffaultVolume_Sound_Hit;
        Sound_Hurt.VolumeDb = DeffaultVolume_Sound_Hurt;
        Sound_Death.VolumeDb = DeffaultVolume_Sound_Death;
    }

    public void turn_off()
    {
        Sound_Attack.VolumeDb = -80;
        Sound_Hit.VolumeDb = -80;
        Sound_Hurt.VolumeDb = -80;
        Sound_Death.VolumeDb = -80;
    }
}
