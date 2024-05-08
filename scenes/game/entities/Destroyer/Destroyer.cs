using Godot;
using Godot.NativeInterop;
using System;
using System.Diagnostics.Eventing.Reader;

public partial class Destroyer : CharacterBody2D
{

    public enum Statement
    {
        Idle,
        Move,
        BasicAttack,
        Shoot,
        Rush,
        Zoom,
        ArmorBreak,
        Stun,
        SpikeAttack,
    }

    public enum SoundSettings
    {
        ON,
        OFF
    }

    public static Random RNG = new Random();

    // Настройки
    public float Speed = 40;
    public int Damage = 25;
    public int ArmorRushDamage = 30;
    public int RushDamage = 40;
    public int ZoomDamage = 30;
    public int Health = 200;
    public int Armor = 2;
    public double BasicAttackCooldown = 2;
    public double ShootCooldown = 12;
    public double AbilityCooldown = 12;
    public double IntermissionAfterAbility = 6; // Сколько секунд после способности не сможет юзать способность
    public Vector2 Bounce = new Vector2(1000, -600); // Откидывание рашем на 1 стадии
    public double RushTime = 5;
    public float RushSpeedMultiplier = 7; // Множитель скорости раша на 1 стадии
    public float ArmorBreakRushSpeedMultiplier = 5; // Множитель скорости раша во 2 стадии
    public float StepCheckArea = 64; // Для проверки на наличие обрыва между игроком и врагом
    public int ChanceToZoom = 0; // Шанс зума вместо раша на 1 стадии
    public float AddSizeToHitBox = 8; // На сколько увеличится хитбокс во второй стадии(хз на сколько он нужно)
    public double StunTime = 5; // Время стана во 2 стадии после абилок
    public int SpikeAttackDistance = 2048; // Дистанция атаки шипами
    public int SpikeAttackStep = 96;
    public double SpikeAttackDuration = 3; // За сколько секунд закончит атаку с шипами
    public double NewAbilityCooldown = 8; // Кулдаун абилок во 2 стадии

    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    // Используются в процессе
    public Statement State;
    public bool Alive = true;
    public Color DeffaultSpriteModulate;
    public double DamageEffectTime = 0; // Для того что бы враг краснел на время после удара
    public float CloseDistance;
    public bool IsInHitArea = false;
    public bool IsInBattleArea = false;
    public double DeltaBasicAttackCooldown = 0;
    public double DeltaShootCooldown;
    public double DeltaAbilityCooldown;
    public double Intermission;
    public bool IsReadyToRush = false;
    public bool IsCollided = false;
    public double DeltaRushTime = 0;
    public float DeffaultTP;
    public bool IsReadyToZoom = false;
    public float ZoomHeight = 1400;
    public bool IsZoomFinished = false;
    public double DeltaStunTime = 0;
    public bool IsReadyToSpikeAttack = false;
    public double DeltaSpikeAttackDuration = 0;
    public double DeltaSpikeStepDuration = 0;
    public int CurrentSpike = 0;
    public bool IsSpikeAttackEnd = true;
    public bool CanArmorBreak = true;

    // Для кривой Безье
    public Vector2 P0 = new Vector2();
    public Vector2 P1 = new Vector2();
    public Vector2 Ph = new Vector2();
    public float BezierProgress = 0;


    public int Direction = RNG.Next(2) == 1 ? 1 : -1;
    public float GroundPos;


    public CollisionShape2D CollisionShape;
    public RayCast2D RayCast;
    public Marker2D Marker;
    public AnimationPlayer Anim;
    public AnimationPlayer Anim2;
    public AnimatedSprite2D AnimSprite;
    public Area2D HitBoxes;
    public Area2D HurtBoxes;
    public Node2D Triggers;
    public Area2D ExplosionArea;
    public Node2D RayCasts;
    public Node2D ForwardWallDetectors;
    public RayCast2D ForwardWallDetector_1;
    public RayCast2D ForwardWallDetector_2;

    public Node2D Sounds;
    public AudioStreamPlayer2D Sound_ArmorHurt;
    public AudioStreamPlayer2D Sound_BlockHurt;
    public AudioStreamPlayer2D Sound_Hurt;
    public AudioStreamPlayer2D Sound_Hit;

    public float DeffaultVolume_Sound_ArmorHurt;
    public float DeffaultVolume_Sound_BlockHurt;
    public float DeffaultVolume_Sound_Hurt;
    public float DeffaultVolume_Sound_Hit;

    public PackedScene Plasma;
    public PackedScene Spike;

    public player Player;
    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        RayCast = GetNode<RayCast2D>("RayCast2D");
        Marker = GetNode<Marker2D>("Marker2D");
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        Anim2 = GetNode<AnimationPlayer>("AnimationPlayer2");
        AnimSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HitBoxes = GetNode<Area2D>("HitBoxes");
        HurtBoxes = GetNode<Area2D>("HurtBoxes");
        Triggers = GetNode<Node2D>("Triggers");
        ExplosionArea = GetNode<Area2D>("ExplosionArea");
        RayCasts = GetNode<Node2D>("RayCasts");

        ForwardWallDetectors = GetNode<Node2D>("ForwardWallDetectors");
        ForwardWallDetector_1 = ForwardWallDetectors.GetNode<RayCast2D>("ForwardWallDetector_1");
        ForwardWallDetector_2 = ForwardWallDetectors.GetNode<RayCast2D>("ForwardWallDetector_2");

        //Звуки
        Sounds = GetNode<Node2D>("Sounds");

        Sound_ArmorHurt = Sounds.GetNode<AudioStreamPlayer2D>("ArmorHurt");
        Sound_BlockHurt = Sounds.GetNode<AudioStreamPlayer2D>("BlockHurt");
        Sound_Hurt = Sounds.GetNode<AudioStreamPlayer2D>("Hurt");
        Sound_Hit = Sounds.GetNode<AudioStreamPlayer2D>("Hit");

        DeffaultVolume_Sound_ArmorHurt = Sound_ArmorHurt.VolumeDb;
        DeffaultVolume_Sound_BlockHurt = Sound_BlockHurt.VolumeDb;
        DeffaultVolume_Sound_Hurt = Sound_Hurt.VolumeDb;
        DeffaultVolume_Sound_Hit = Sound_Hit.VolumeDb;
        //

        Plasma = GD.Load<PackedScene>("res://scenes/game/entities/Destroyer/Etc/Plasma/Plasma.tscn");
        Spike = GD.Load<PackedScene>("res://scenes/game/entities/Destroyer/Etc/Spike/Spike.tscn");

        Player = (player)GetTree().GetFirstNodeInGroup("Player");

        State = Statement.Idle;
        DeffaultSpriteModulate = AnimSprite.Modulate;
        CloseDistance = ((RectangleShape2D)HitBoxes.GetNode<CollisionShape2D>("Shape1").Shape).Size.X / 2;
        DeltaShootCooldown = 20;
        DeltaAbilityCooldown = 10;
        DeffaultTP = ((CapsuleShape2D)CollisionShape.Shape).Radius + 8;
        GroundPos = ((CapsuleShape2D)CollisionShape.Shape).Height / 2 + CollisionShape.Position.Y;
    }



    public override void _Process(double delta)
    {

        if (DamageEffectTime > 0)
        {
            DamageEffectTime -= delta;
            if (DamageEffectTime <= 0)
            {
                AnimSprite.Modulate = DeffaultSpriteModulate;
            }
        }

        if (!Alive)
        {
            return;
        }

        if (DeltaBasicAttackCooldown > 0 && (State == Statement.Idle || State == Statement.Move && State != Statement.Stun))
        {
            DeltaBasicAttackCooldown -= delta;
        }

        if (DeltaShootCooldown > 0 && State != Statement.Shoot)
        {
            DeltaShootCooldown -= delta;
        }

        if (DeltaAbilityCooldown > 0 && State != Statement.Rush && State != Statement.Zoom && State != Statement.Stun)
        {
            DeltaAbilityCooldown -= delta;
        }

        if (Intermission > 0 && (State == Statement.Idle || State == Statement.Move))
        {
            Intermission -= delta;
        }

        switch (State)
        {
            case Statement.Idle:
                Idle();
                break;
            case Statement.Move:
                Move(delta);
                break;
            case Statement.BasicAttack:
                BasicAttack();
                break;
            case Statement.Shoot:
                Shoot();
                break;
            case Statement.Rush:
                Rush(delta);
                break;
            case Statement.Zoom:
                Zoom(delta);
                break;
            case Statement.ArmorBreak:
                ArmorBreak();
                break;
            case Statement.Stun:
                Stun(delta);
                break;
            case Statement.SpikeAttack:
                SpikeAttack(delta);
                break;
        }

        if (State == Statement.Zoom && !IsZoomFinished)
        {
            return;
        }

        Vector2 velocity = Vector2.Zero;
        if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
        }

        MoveAndCollide(velocity);
        MoveAndSlide();
        TurnAround();
    }

    Vector2 Reverse = new Vector2(-1, 1);
    public void TurnAroundElements(Node2D Obj)
    {
        Obj.Position *= Reverse;
        if (Obj is RayCast2D RayCast)
        {
            RayCast.TargetPosition *= Reverse;
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

    public void AfterZoom()
    {
        GetNode<AnimatedSprite2D>("Effects2").FlipH = !AnimSprite.FlipH;
    }

    public void TurnAround()
    {
        if (Alive && (Direction == 1) == AnimSprite.FlipH)
        {
            GetNode<AnimatedSprite2D>("Effects").FlipH = !GetNode<AnimatedSprite2D>("Effects").FlipH;
            GetNode<AnimatedSprite2D>("Effects2").FlipH = !GetNode<AnimatedSprite2D>("Effects2").FlipH;
            AnimSprite.FlipH = !AnimSprite.FlipH;
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

    public void Idle()
    {
        Direction = Math.Sign(Player.GlobalPosition.X - GlobalPosition.X);

        if (Armor > 0)
        {
            if (AnimSprite.Animation != "ArmorIdle")
            {
                Anim.Play("ArmorIdle");
            }
        }
        else
        {
            if (AnimSprite.Animation != "Idle1" && ((AnimSprite.Animation != "Hit1" && AnimSprite.Animation != "Hit2") || !Anim.IsPlaying()))
            {
                if (State == Statement.Rush)
                {
                    Anim.Play("Idle2");
                }
                else
                {
                    Anim.Play("Idle1");
                }
            }
        }

        if (IsInHitArea && State == Statement.Idle && DeltaBasicAttackCooldown <= 0)
        {
            State = Statement.BasicAttack;
        }
        TryToUseAbility();
    }

    public void Move(double delta)
    {
        Direction = Math.Sign(Player.GlobalPosition.X - GlobalPosition.X);

        bool IsTargetClose = Math.Abs(GlobalPosition.X - Player.GlobalPosition.X) < CloseDistance * GlobalScale.X;

        Vector2 velocity = Vector2.Zero;

        if (!IsOnFloor())
        {
            return;
        }

        float CurrentSpeed = Speed;
        if ((!RayCast.IsColliding() || ForwardWallDetector_1.IsColliding() || ForwardWallDetector_2.IsColliding()) || IsTargetClose)
        {
            Idle();
            CurrentSpeed = 0;
        }
        else
        {
            Anim.Play("ArmorRun");
        }

        velocity.X += Direction * CurrentSpeed * (float)delta;

        TryToUseAbility();

        if (DeltaShootCooldown <= 0 && Intermission <= 0 && !IsTargetClose)
        {
            State = Statement.Shoot;
        }

        MoveAndCollide(velocity);
        MoveAndSlide();
    }

    public void TryToUseAbility()
    {
        bool IsTargetClose = Math.Abs(GlobalPosition.X - Player.GlobalPosition.X) < CloseDistance * GlobalScale.X && IsInBattleArea;
        Godot.Collections.Array<Node> Children = RayCasts.GetChildren();
        if (DeltaAbilityCooldown <= 0 && Intermission <= 0 && !IsTargetClose && (Player.IsOnFloor() || Children.Count != 0))
        {
            if (Children.Count == 0)
            {
                float PlayerDistance = Math.Abs(Player.GlobalPosition.X - GlobalPosition.X);
                ForwardWallDetector_1.TargetPosition = new Vector2(Math.Sign(ForwardWallDetector_1.TargetPosition.X) * PlayerDistance / GlobalScale.X, ForwardWallDetector_1.TargetPosition.Y); // Для того что бы проверить есть ли стена между врагом и игроком
                ForwardWallDetector_2.TargetPosition = new Vector2(Math.Sign(ForwardWallDetector_2.TargetPosition.X) * PlayerDistance / GlobalScale.X, ForwardWallDetector_2.TargetPosition.Y);
                for (float x = 0; PlayerDistance >= Math.Abs(x); x += Direction * StepCheckArea) // Для того что бы проверить есть ли обрыв между врагом и игроком
                {
                    RayCast2D rayCast = (RayCast2D)RayCast.Duplicate();
                    rayCast.Name = "RayCast2D_" + (x).ToString();
                    RayCasts.AddChild(rayCast);
                    rayCast.Position = new Vector2(x, rayCast.Position.Y);
                }
            }
            else
            {
                bool IsCanReach = true;
                for (int i = 0; i < Children.Count; i++)
                {
                    RayCast2D raycast2d = Children[i] as RayCast2D;
                    if (!raycast2d.IsColliding())
                    {
                        IsCanReach = false;
                    }
                    raycast2d.QueueFree();
                }

                if (RNG.Next(1, 101) <= ChanceToZoom || ForwardWallDetector_1.IsColliding() || ForwardWallDetector_2.IsColliding() || !IsCanReach || !IsInBattleArea)
                {
                    State = Statement.Zoom;
                }
                else
                {
                    if (Armor > 0)
                    {
                        State = Statement.Rush;
                    }
                    else
                    {
                        if (RNG.Next(2) == 0)
                        {
                            State = Statement.Rush;
                        }
                        else
                        {
                            State = Statement.SpikeAttack;
                        }
                    }
                }
                ForwardWallDetector_1.TargetPosition = new Vector2(Math.Sign(ForwardWallDetector_1.TargetPosition.X) * DeffaultTP, ForwardWallDetector_1.TargetPosition.Y);
                ForwardWallDetector_2.TargetPosition = new Vector2(Math.Sign(ForwardWallDetector_2.TargetPosition.X) * DeffaultTP, ForwardWallDetector_2.TargetPosition.Y);
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].QueueFree();
                }
            }
        }
    }

    public async void BasicAttack()
    {
        if (DeltaBasicAttackCooldown <= 0)
        {
            DeltaBasicAttackCooldown = BasicAttackCooldown;
            Intermission = BasicAttackCooldown;
            Player.CallDeferred("GetDamaged", Damage);
            if (Armor > 0)
            {
                Anim.Play("ArmorBasicAttack");
            }
            else
            {
                Anim.Play("BasicAttack");
            }
            await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
            if (IsInHitArea || !IsInBattleArea || Armor <= 0)
            {
                State = Statement.Idle;
            }
            else if (IsInBattleArea)
            {
                State = Statement.Move;
            }
        }
    }

    public async void Shoot()
    {
        if (DeltaShootCooldown <= 0)
        {
            DeltaShootCooldown = ShootCooldown;
            DeltaBasicAttackCooldown = BasicAttackCooldown;
            Intermission = IntermissionAfterAbility;
            Anim.Play("Shoot");
            await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
            if (IsInHitArea || !IsInBattleArea || Armor <= 0)
            {
                State = Statement.Idle;
            }
            else if (IsInBattleArea)
            {
                State = Statement.Move;
            }
        }
    }

    public void FireShoot() // Запускает анимация выстрела
    {
        Node Node_NewPlasma = Plasma.Instantiate();
        Area2D NewPlasma = Node_NewPlasma as Area2D;
        GetParent().GetParent().AddChild(NewPlasma);
        NewPlasma.GlobalScale = GlobalScale;
        NewPlasma.GlobalPosition = Marker.GlobalPosition;
        NewPlasma.Set("Direction", Direction);
        NewPlasma.CallDeferred("TurnAround");
    }

    public async void Rush(double delta)
    {
        if (IsCollided && Armor <= 0)
        {
            return;
        }
        if (DeltaAbilityCooldown <= 0 && !IsReadyToRush)
        {
            DeltaAbilityCooldown = AbilityCooldown;
            DeltaBasicAttackCooldown = BasicAttackCooldown;
            Intermission = IntermissionAfterAbility;
            if (Armor > 0)
            {
                Anim.Play("ArmorPrepareToRush");
            }
            else
            {
                Anim.Play("PrepareToRush");
                AnimSprite.SelfModulate = new Color(0.5f, 0.5f, 0.5f);
            }
            await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
            IsReadyToRush = true;
            IsCollided = false;
            DeltaRushTime = RushTime;

        }
        else if (IsReadyToRush)
        {
            if (DeltaRushTime > 0 && RayCast.IsColliding() && !(ForwardWallDetector_1.IsColliding() || ForwardWallDetector_2.IsColliding()))
            {
                DeltaRushTime -= delta;
                if (Armor > 0)
                {
                    Anim.Play("RushRun");
                }
                else
                {
                    Direction = Math.Sign(Player.GlobalPosition.X - GlobalPosition.X);
                    if (IsInHitArea)
                    {
                        IsCollided = true;
                        Player.CallDeferred("GetDamaged", RushDamage);
                        Anim.Play("RushAttack");
                        await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
                        FinishRush();
                        return;
                    }
                    if (Math.Abs(GlobalPosition.X - Player.GlobalPosition.X) < CloseDistance * GlobalScale.X || !IsInBattleArea)
                    {
                        Idle();
                        return;
                    }
                    else
                    {
                        Anim.Play("Run");
                    }
                }
                Vector2 velocity = Vector2.Zero;
                velocity.X += Direction * Speed * RushSpeedMultiplier * (float)delta;
                MoveAndCollide(velocity);
                MoveAndSlide();
            }
            else
            {
                Anim.Play("RESET");
                if (Armor <= 0)
                {
                    IsCollided = true;
                    Anim.Play("FinishRush");
                    await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
                }
                FinishRush();
            }
        }
    }

    public void Collided(Node2D Area)
    {
        if (Armor > 0 && Area.Name == "HurtBox" && Player.health > 0 && !IsCollided && State == Statement.Rush)
        {
            IsCollided = true;
            Player.CallDeferred("GetDamaged", ArmorRushDamage);
            Player.Velocity = new Vector2(Direction, 1) * Bounce;
        }
    }

    public void FinishRush(bool Stun = false)
    {
        IsReadyToRush = false;
        if (Armor <= 0)
        {
            AnimSprite.SelfModulate = new Color(1, 1, 1);
        }
        IsCollided = false;
        if (Armor <= 0 && Stun)
        {
            State = Statement.Stun;
            DeltaStunTime = StunTime - 1;
        }
        else if (Armor <= 0 || IsInHitArea || !IsInBattleArea)
        {
            State = Statement.Idle;
        }
        else if (IsInBattleArea)
        {
            State = Statement.Move;
        }
    }

    private Vector2 BezierCurve(float t)
    {
        Vector2 Q0 = P0.Lerp(Ph, t);
        Vector2 Q1 = Ph.Lerp(P1, t);
        Vector2 r = Q0.Lerp(Q1, t);
        return r;
    }

    public async void Zoom(double delta)
    {
        if (IsZoomFinished)
        {
            return;
        }

        if (!IsReadyToZoom)
        {
            DeltaAbilityCooldown = AbilityCooldown;
            DeltaBasicAttackCooldown = BasicAttackCooldown;
            Intermission = IntermissionAfterAbility;
            IsZoomFinished = false;
            BezierProgress = 0;
            HurtBoxes.SetCollisionLayerValue(3, false);
            SetCollisionMaskValue(1, false);
            Anim2.Play("Zoom");
            if (Armor > 0)
            {
                Anim.Play("ArmorZoom");
            }
            P0 = GlobalPosition;
            P1 = Player.GlobalPosition;
            Ph = new Vector2((P0.X + P1.X) / 2, Math.Min((P0.Y + P1.Y) / 2 - ZoomHeight, Math.Min(P0.Y, P1.Y) - ZoomHeight / 2));
            IsReadyToZoom = true;
        }
        else
        {
            BezierProgress += (float)delta / 3;
            Vector2 Pos = BezierCurve(BezierProgress);
            Rotation = GlobalPosition.AngleToPoint(Pos) + (float)Math.PI / 2;
            GlobalPosition = Pos;
            if (BezierProgress >= 1)
            {
                Anim2.Play("Explosion");
                AfterZoom();
                IsZoomFinished = true;
                HurtBoxes.SetCollisionLayerValue(3, true);
                SetCollisionMaskValue(1, true);
                IsReadyToZoom = false;
                Rotation = 0;
                Position -= new Vector2(0, 64);
                Godot.Collections.Array<Area2D> OverlappingAreas = ExplosionArea.GetOverlappingAreas();
                for (int i = 0; i < OverlappingAreas.Count; i++)
                {
                    if (OverlappingAreas[i].Name == "HurtBox" && Player.health > 0)
                    {
                        Player.CallDeferred("GetDamaged", ZoomDamage);
                        break;
                    }
                }
                if (Armor > 0)
                {
                    Anim.Play("ArmorFinishZoom");
                    await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
                }
                IsZoomFinished = false;
                if (Armor <= 0 || IsInHitArea || !IsInBattleArea)
                {
                    State = Statement.Idle;
                }
                else if (IsInBattleArea)
                {
                    State = Statement.Move;
                }
            }
        }
    }

    public void Stun(double delta)
    {
        if (DeltaStunTime > 0)
        {
            DeltaStunTime -= delta;
            AnimSprite.SelfModulate = new Color(0.5f, 0.5f, 0.5f);
            Idle();
        }
        else
        {
            AnimSprite.SelfModulate = new Color(1, 1, 1);
            State = Statement.Idle;
        }
    }

    public void SpikeAttack(double delta)
    {
        if (!IsReadyToSpikeAttack)
        {
            DeltaAbilityCooldown = AbilityCooldown;
            DeltaBasicAttackCooldown = BasicAttackCooldown;
            Intermission = IntermissionAfterAbility;
            for (float x = 0; Math.Abs(SpikeAttackDistance) >= Math.Abs(x); x += Direction * SpikeAttackStep)
            {
                RayCast2D rayCast = (RayCast2D)RayCast.Duplicate();
                rayCast.Name = "SpikeRoot_" + (x).ToString();
                RayCasts.AddChild(rayCast);
                rayCast.Position = new Vector2(x, rayCast.Position.Y);
            }
            Anim.Play("SpikeAttack");
            DeltaSpikeAttackDuration = SpikeAttackDuration;
            CurrentSpike = 0;
            DeltaSpikeStepDuration = 0;
            IsSpikeAttackEnd = false;
            IsReadyToSpikeAttack = true;
        }
        else
        {
            DeltaSpikeAttackDuration -= delta;
            DeltaSpikeStepDuration -= delta;
            if (DeltaSpikeStepDuration <= 0)
            {
                RayCast2D SpikeRoot = RayCasts.GetNodeOrNull<RayCast2D>("SpikeRoot_" + (CurrentSpike).ToString());
                if (SpikeRoot != null && SpikeRoot.IsColliding() && !IsSpikeAttackEnd)
                {
                    Node Node_NewSpike = Spike.Instantiate();
                    Area2D NewSpike = Node_NewSpike as Area2D;
                    GetParent().GetParent().AddChild(NewSpike);
                    NewSpike.GlobalScale = GlobalScale;
                    NewSpike.GlobalPosition = (NewSpike.Position * GlobalScale) + GlobalPosition + new Vector2(CurrentSpike, GroundPos) * GlobalScale;
                }
                else
                {
                    IsSpikeAttackEnd = true;
                }
                if (SpikeRoot != null)
                {
                    SpikeRoot.QueueFree();
                }
                CurrentSpike += Direction * SpikeAttackStep;
                DeltaSpikeStepDuration += SpikeAttackDuration / (SpikeAttackDistance / SpikeAttackStep);
            }
            if (DeltaSpikeAttackDuration <= 0)
            {
                IsReadyToSpikeAttack = false;
                State = Statement.Stun;
                DeltaStunTime = StunTime;
            }
        }
    }

    public void InBattleArea(Node2D Area)
    {
        if (Area.Name == "HurtBox" && Player.health > 0)
        {
            IsInBattleArea = true;
            if (Armor > 0 && !IsInHitArea && State == Statement.Idle)
            {
                State = Statement.Move;
            }
        }
    }

    public void OutBattleArea(Node2D Area)
    {
        if (Area.Name == "HurtBox" && Player.health > 0)
        {
            IsInBattleArea = false;
            if (State == Statement.Move)
            {
                State = Statement.Idle;
            }
        }
    }

    public void InHitArea(Node2D Area)
    {
        if (Area.Name == "HurtBox" && Player.health > 0)
        {
            IsInHitArea = true;
            if (State == Statement.Move)
            {
                State = Statement.Idle;
            }
        }
    }

    public void OutHitArea(Node2D Area)
    {
        if (Area.Name == "HurtBox" && Player.health > 0)
        {
            IsInHitArea = false;
            if (Armor > 0 && IsInBattleArea && State == Statement.Idle)
            {
                State = Statement.Move;
            }
        }
    }

    public void SetArmorBreak()
    {
        State = Statement.ArmorBreak;
    }

    public async void ArmorBreak()
    {
        if (Armor > 0 && CanArmorBreak)
        {
            CanArmorBreak = false;
            IsReadyToRush = false;
            DamageEffectTime = 0.1;
            AnimSprite.Modulate = new Color(1, 0.5f, 0.5f);
            --Armor;
            if (Armor <= 0)
            {
                ChanceToZoom = 0;
                RushSpeedMultiplier = ArmorBreakRushSpeedMultiplier;
                AbilityCooldown = NewAbilityCooldown;
                ((RectangleShape2D)HitBoxes.GetNode<CollisionShape2D>("Shape1").Shape).Size += new Vector2(AddSizeToHitBox, 0);
                Anim.Play("ArmorBreak");
                await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
                State = Statement.Idle;
            }
            else
            {
                Anim.Play("ArmorHit");
                await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
                if (IsInHitArea || !IsInBattleArea)
                {
                    State = Statement.Idle;
                }
                else if (IsInBattleArea)
                {
                    State = Statement.Move;
                }
            }
            CanArmorBreak = true;
        }
    }

    public async void GetDamage(int Damage)
    {
        if (Armor <= 0 && (State == Statement.Stun || State == Statement.Rush))
        {
            Sound_Hurt.Play();
            Health -= Damage;
            DamageEffectTime = 0.1;
            AnimSprite.Modulate = new Color(1, 0.5f, 0.5f);
            if (State == Statement.Rush && IsReadyToRush)
            {
                IsCollided = true;
                Anim.Play("Hit2");
                await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
                FinishRush(true);
            }
            else if (State == Statement.Stun)
            {
                Anim.Play("Hit1");
            }

            if (Health <= 0)
            {
                Death();
            }
        }
        else if (Armor > 0)
        {
            Sound_ArmorHurt.Play();
        }
        else
        {
            Sound_BlockHurt.Play();
        }
    }

    public async void Death()
    {
        Alive = false;
        Anim.Play("Death");
        await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
        QueueFree();
    }

    public void turn_on()
    {
    }

    public void turn_off()
    {
    }
}
