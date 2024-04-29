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

    public enum SoundSettings
    {
        ON,
        OFF
    }

    public static Random RNG = new Random();

    public int Speed = 60;
    public int Damage = 30;
    public int Health = 1;//130;
    public float StunAfterDamage = 0.5f; // Сколько враг будет под станом после удара
    public float AttackCooldown = 2;


    public Statement State = Statement.Run;
    public bool Alive = true;

    public float DamagedTime = 0;
    public float IdleTime = 0;
    public ulong LastDamagedTime = Godot.Time.GetTicksMsec();

    //private float Gravity = (float)ProjectSettings.GetSetting("physics/2d/deault_gravity");
    private float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


    public int Direction = RNG.Next(2) == 1 ? 1 : -1;


    public RayCast2D rayCast2D;
    public AnimatedSprite2D Anim;
    public Area2D HitBoxes;
    public Area2D HurtBoxes;
    public CollisionShape2D HitBox1;
    public CollisionShape2D HurtBox1;

    public Node2D ForwardWallDetectors;
    public RayCast2D ForwardWallDetector_1;
    public RayCast2D ForwardWallDetector_2;

    public Node2D Sounds;
    public AudioStreamPlayer2D Sound_Hit;
    public AudioStreamPlayer2D Sound_Death;

    public float DeffaultVolume_Sound_Hit;
    public float DeffaultVolume_Sound_Death;

    public player Player;
    public override void _Ready()
    {
        rayCast2D = GetNode<RayCast2D>("RayCast2D");
        Anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HitBoxes = GetNode<Area2D>("HitBoxes");
        HurtBoxes = GetNode<Area2D>("HurtBoxes");
        HitBox1 = GetNode<CollisionShape2D>("HitBoxes/Box1");
        HurtBox1 = GetNode<CollisionShape2D>("HitBoxes/Box1");

        ForwardWallDetectors = GetNode<Node2D>("ForwardWallDetectors");
        ForwardWallDetector_1 = ForwardWallDetectors.GetNode<RayCast2D>("ForwardWallDetector_1");
        ForwardWallDetector_2 = ForwardWallDetectors.GetNode<RayCast2D>("ForwardWallDetector_2");

        //Звуки
        Sounds = GetNode<Node2D>("Sounds");
        Sound_Hit = Sounds.GetNode<AudioStreamPlayer2D>("Hit");
        Sound_Death = Sounds.GetNode<AudioStreamPlayer2D>("Death");

        DeffaultVolume_Sound_Hit = Sound_Hit.VolumeDb;
        DeffaultVolume_Sound_Death = Sound_Death.VolumeDb;
        //

        Player = (player)GetTree().GetFirstNodeInGroup("Player");
    }



    public override void _Process(double delta)
    {

        if (DamagedTime > 0 && Alive)
        {
            DamagedTime -= (float)delta;
            if (DamagedTime <= 0 && State != Statement.Attack)
            {
                if (IdleTime > 0)
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

        if (Anim.Modulate == new Color(1, 0.5f, 0.5f) && Godot.Time.GetTicksMsec() - LastDamagedTime > 100) //Если в последний раз урон проходил 100 милисекунд назад, то убираем красный цвет
        {
            Anim.Modulate = new Color(1, 1, 1);
        }

        Vector2 velocity = Vector2.Zero;

        if (IsOnFloor() && Alive && State == Statement.Run)
        {
            if (!rayCast2D.IsColliding() || ForwardWallDetector_1.IsColliding() || ForwardWallDetector_2.IsColliding())
            {
                Direction *= -1;
            }

            velocity.X += Direction * Speed * (float)delta;
        }
        else
        {
            velocity.Y += Gravity * (float)delta;
        }

        Godot.Collections.Array<Area2D> OverlappingBodies = HitBoxes.GetOverlappingAreas();
        for (int i = 0; i < OverlappingBodies.Count; i++)
        {
            Attack(OverlappingBodies[i]);
        }

        MoveAndCollide(velocity);
        MoveAndSlide();
        TurnAround();
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


    public bool SecondAttack = false;

    public async void Attack(Node2D Body)
    {
        if (State == Statement.Run && Alive && Body.Name == "HurtBox" && (int)Player.Get("health") > 0)
        {
            Player.CallDeferred("GetDamaged", Damage);
            State = Statement.Idle;
            IdleTime = AttackCooldown;
            if (SecondAttack)
            {
                Anim.Play("Attack2");
            }
            else
            {
                Anim.Play("Attack1");
            }
            SecondAttack = !SecondAttack;
            Sound_Hit.Play(0.25f);
            await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
            if (DamagedTime > 0)
            {
                Anim.Play("Grep");
            }
            else if (IdleTime > 0)
            {
                Anim.Play("Idle");
            }
        }
    }

    public void GetDamage(int Damage)
    {

        Health -= Damage;
        LastDamagedTime = Godot.Time.GetTicksMsec();
        Anim.Modulate = new Color(1, 0.5f, 0.5f);
        if (Health <= 0)
        {
            Death();
        }
        else
        {
            State = Statement.Idle;
            Anim.Play("Grep");
            DamagedTime = StunAfterDamage;
        }
    }

    public async void Death()
    {
        if (Alive)
        {
            Alive = false;
            Anim.Play("Death");
            Sound_Death.Play();
            await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
            QueueFree();
        }
    }

    public void turn_on()
    {
        Sound_Hit.VolumeDb = DeffaultVolume_Sound_Hit;
        Sound_Death.VolumeDb = DeffaultVolume_Sound_Death;
    }

    public void turn_off()
    {
        Sound_Hit.VolumeDb = -80;
        Sound_Death.VolumeDb = -80;
    }
}
