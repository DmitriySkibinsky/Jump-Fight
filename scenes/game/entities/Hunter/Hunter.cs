using Godot;
using System;
public partial class Hunter : CharacterBody2D
{

    public enum Statement
    {
        Idle,
        Move,
        Attack,
        Battle,
        Roll,
    }

    public enum SoundSettings
    {
        ON,
        OFF
    }

    public static Random RNG = new Random();

    // Настройки
    public float Speed = 260;
    public int Health = 1;//68;
    public double Reload = 1.5;
    public double RollCooldown = 25;
    public float RollSpeedBoost = 2.5f;
    public int MinIdleTime = 5;
    public int MinMoveTime = 2;
    public int MaxIdleTime = 11;
    public int MaxMoveTime = 7;
    public int ForwardCheckArea = 320; // Для Battle стейтмент с DangerArea
    public int BackCheckArea = 320;
    public int StepCheckArea = 80;


    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    // Нужно для переключений между стейтментами

    // Используются в процессе
    public Statement State;
    public bool Alive = true;
    public bool IsRunAway = false;
    public double DamageEffectTime = 0; // Для того что бы враг красне на время после удара
    public double IdleTime = 0;
    public double MoveTime = 0;
    public double DeltaReload = 0;
    public double DeltaRollCooldown = 0;

    public int Direction = RNG.Next(2) == 1 ? 1 : -1;


    public CollisionShape2D CollisionShape;
    public RayCast2D RayCast2D1;
    public RayCast2D RayCast2D2;
    public Marker2D Marker;
    public AnimationPlayer Anim;
    public AnimatedSprite2D AnimSprite;
    public Area2D HurtBoxes;
    public Node2D Triggers;
    public Node2D RayCasts;
    public Area2D FarArea;
    public Area2D CloseArea;
    public Area2D DangerArea;

    public Node2D ForwardWallDetectors;
    public RayCast2D ForwardWallDetector_1;
    public RayCast2D ForwardWallDetector_2;
    public Node2D BackWallDetectors;
    public RayCast2D BackWallDetector_1;
    public RayCast2D BackWallDetector_2;

    public Node2D Sounds;
    public AudioStreamPlayer2D Sound_Attack;
    public AudioStreamPlayer2D Sound_Shoot;

    public float DeffaultVolume_Sound_Attack;
    public float DeffaultVolume_Sound_Shoot;

    public PackedScene Arrow;

    public player Player;
    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        RayCast2D1 = GetNode<RayCast2D>("RayCast2D1");
        RayCast2D2 = GetNode<RayCast2D>("RayCast2D2");
        Marker = GetNode<Marker2D>("Marker2D");
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HurtBoxes = GetNode<Area2D>("HurtBoxes");
        Triggers = GetNode<Node2D>("Triggers");
        RayCasts = GetNode<Node2D>("RayCasts");
        FarArea = Triggers.GetNode<Area2D>("FarArea");
        CloseArea = Triggers.GetNode<Area2D>("CloseArea");
        DangerArea = Triggers.GetNode<Area2D>("DangerArea");

        ForwardWallDetectors = GetNode<Node2D>("ForwardWallDetectors");
        ForwardWallDetector_1 = ForwardWallDetectors.GetNode<RayCast2D>("ForwardWallDetector_1");
        ForwardWallDetector_2 = ForwardWallDetectors.GetNode<RayCast2D>("ForwardWallDetector_2");
        BackWallDetectors = GetNode<Node2D>("BackWallDetectors");
        BackWallDetector_1 = BackWallDetectors.GetNode<RayCast2D>("BackWallDetector_1");
        BackWallDetector_2 = BackWallDetectors.GetNode<RayCast2D>("BackWallDetector_2");

        //Звуки
        Sounds = GetNode<Node2D>("Sounds");
        Sound_Attack = Sounds.GetNode<AudioStreamPlayer2D>("Attack");
        Sound_Shoot = Sounds.GetNode<AudioStreamPlayer2D>("Shoot");

        DeffaultVolume_Sound_Attack = Sound_Attack.VolumeDb;
        DeffaultVolume_Sound_Shoot = Sound_Shoot.VolumeDb;
        //

        Arrow = GD.Load<PackedScene>("res://scenes/game/entities/Hunter/Etc/Arrow.tscn");

        Player = (player)GetTree().GetFirstNodeInGroup("Player");

        State = Statement.Idle;
        IdleTime = RNG.Next(MinIdleTime, MaxIdleTime);
    }



    public override void _Process(double delta) // НЕ МЕНЯТЬ НА _PhysicsProcess!
    {
        //GD.Print(State, "\t", AnimSprite.Animation);
        if (DamageEffectTime > 0)
        {
            DamageEffectTime -= delta;
            if (DamageEffectTime <= 0)
            {
                AnimSprite.Modulate = new Color(1, 1f, 1f);
            }
        }

        if (DeltaReload > 0 && State != Statement.Attack)
        {
            DeltaReload -= delta;
        }

        if (DeltaRollCooldown > 0)
        {
            DeltaRollCooldown -= delta;
        }

        if (!Alive)
        {
            return;
        }

        switch (State)
        {
            case Statement.Idle:
                Idle(delta);
                break;
            case Statement.Move:
                Move(delta);
                break;
            case Statement.Roll:
                Roll(delta);
                break;
            case Statement.Attack:
                Attack(delta);
                break;
            case Statement.Battle:
                Battle(delta);
                break;
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
        if (Alive && (Direction == 1) == AnimSprite.FlipH)
        {
            AnimSprite.FlipH = !AnimSprite.FlipH;
            Godot.Collections.Array<Node> Children = GetChildren();
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].Name == "RayCasts")
                {
                    continue;
                }
                if (Children[i] is Node2D node2d)
                {
                    TurnAroundElements(node2d);
                }
            }
        }
    }

    public void Idle(double delta)
    {
        if (State == Statement.Idle)
        {
            IdleTime -= delta;
            if (IdleTime <= 0)
            {
                IdleTime = 0;
                MoveTime = RNG.Next(MinMoveTime, MaxMoveTime);
                State = Statement.Move;
            }
        }
        if (AnimSprite.Animation != "Idle")
        {
            Anim.Play("Idle");
        }
    }

    public void Move(double delta, bool ChangeDirection = true) // ChangeDirection - костыль нужный для 1 случая
    {
        if (State == Statement.Move)
        {
            MoveTime -= delta;
            if (MoveTime <= 0)
            {
                MoveTime = 0;
                IdleTime = RNG.Next(MinIdleTime, MaxIdleTime);
                State = Statement.Idle;
            }
        }

        if (AnimSprite.Animation != "Run" && State != Statement.Roll)
        {
            Anim.Play("Run");
        }

        Vector2 velocity = Vector2.Zero;

        if (IsOnFloor())
        {
            if ((!RayCast2D1.IsColliding() || IsOnWall()) && ChangeDirection)
            {
                Direction *= -1;
            }

            velocity.X += Direction * Speed * (float)delta;
        }

        MoveAndCollide(velocity);
        MoveAndSlide();
    }

    public async void Roll(double delta)
    {
        if (DeltaRollCooldown <= 0)
        {
            DeltaRollCooldown = RollCooldown;
            Speed *= RollSpeedBoost;
            Anim.Play("Roll");
            uint CollisionMask = HurtBoxes.CollisionMask;
            HurtBoxes.CollisionMask = 0;
            await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
            HurtBoxes.CollisionMask = CollisionMask;
            Anim.Play("Run");
            Speed /= RollSpeedBoost;
            State = Statement.Battle;
        }
        else
        {
            Move(delta);
        }
    }

    public async void Attack(double delta)
    {
        if (DeltaReload <= 0)
        {
            DeltaReload = Reload;
            Anim.Play("Attack");
            Sound_Attack.Play(0);
            await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
            FinishAttack();
        }
    }

    public void FinishAttack()
    {
        if (State == Statement.Attack && AnimSprite.Animation == "Attack")
        {
            Anim.Play("Idle");
            bool InArea = false;
            Godot.Collections.Array<Area2D> OverlappingAreas = FarArea.GetOverlappingAreas();
            for (int i = 0; i < OverlappingAreas.Count; i++)
            {
                if (OverlappingAreas[i].Name == "HurtBox")
                {
                    InArea = true;
                    break;
                }
            }
            if (InArea)
            {
                State = Statement.Battle;
            }
            else
            {
                IdleTime = RNG.Next(MinIdleTime, MaxIdleTime);
                State = Statement.Idle;
            }
        }
    }

    public int PlayerSide = 1; // Позиция(сторона) врагаотносительно игрока
    public void Battle(double delta)
    {
        if (Player.health <= 0)
        {
            IdleTime = RNG.Next(MinIdleTime, MaxIdleTime);
            State = Statement.Idle;
            return;
        }
        Godot.Collections.Array<Area2D> OverlappingAreas;

        bool InFarArea = false;
        OverlappingAreas = FarArea.GetOverlappingAreas();
        for (int i = 0; i < OverlappingAreas.Count; i++)
        {
            if (OverlappingAreas[i].Name == "HurtBox")
            {
                InFarArea = true;
                break;
            }
        }

        bool InCloseArea = false;
        OverlappingAreas = CloseArea.GetOverlappingAreas();
        for (int i = 0; i < OverlappingAreas.Count; i++)
        {
            if (OverlappingAreas[i].Name == "HurtBox")
            {
                InCloseArea = true;
                break;
            }
        }

        bool InDangerArea = false;
        OverlappingAreas = DangerArea.GetOverlappingAreas();
        for (int i = 0; i < OverlappingAreas.Count; i++)
        {
            if (OverlappingAreas[i].Name == "HurtBox")
            {
                InDangerArea = true;
                break;
            }
        }

        if (!InDangerArea && IsRunAway)
        {
            IsRunAway = false;
            float DeffaultTP = ((CapsuleShape2D)CollisionShape.Shape).Radius + 8;
            ForwardWallDetector_1.TargetPosition = new Vector2(Math.Sign(ForwardWallDetector_1.TargetPosition.X) * DeffaultTP, ForwardWallDetector_1.TargetPosition.Y);
            ForwardWallDetector_2.TargetPosition = new Vector2(Math.Sign(ForwardWallDetector_2.TargetPosition.X) * DeffaultTP, ForwardWallDetector_2.TargetPosition.Y);
            BackWallDetector_1.TargetPosition = new Vector2(Math.Sign(BackWallDetector_1.TargetPosition.X) * DeffaultTP, BackWallDetector_1.TargetPosition.Y);
            BackWallDetector_2.TargetPosition = new Vector2(Math.Sign(BackWallDetector_2.TargetPosition.X) * DeffaultTP, BackWallDetector_2.TargetPosition.Y);
            Godot.Collections.Array<Node> Children = RayCasts.GetChildren();
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].QueueFree();
            }
        }

        if (InDangerArea)
        {
            if (!IsRunAway) // При заходе игрока в опасную зону это условие будет срабатывать 2 раза, в 1 для создания рейкастов, а во 2 для получения данных
            {
                Godot.Collections.Array<Node> Children = RayCasts.GetChildren();
                if (Children.Count == 0)
                {
                    ForwardWallDetector_1.TargetPosition = new Vector2(Math.Sign(ForwardWallDetector_1.TargetPosition.X) * ForwardCheckArea, ForwardWallDetector_1.TargetPosition.Y);
                    ForwardWallDetector_2.TargetPosition = new Vector2(Math.Sign(ForwardWallDetector_2.TargetPosition.X) * ForwardCheckArea, ForwardWallDetector_2.TargetPosition.Y);
                    BackWallDetector_1.TargetPosition = new Vector2(Math.Sign(BackWallDetector_1.TargetPosition.X) * BackCheckArea, BackWallDetector_1.TargetPosition.Y);
                    BackWallDetector_2.TargetPosition = new Vector2(Math.Sign(BackWallDetector_2.TargetPosition.X) * BackCheckArea, BackWallDetector_2.TargetPosition.Y);
                    PlayerSide = Math.Sign(GlobalPosition.X - Player.GlobalPosition.X); // Для того что бы работало в 2 стороны
                    for (int x = -BackCheckArea * PlayerSide; x * PlayerSide <= ForwardCheckArea; x += StepCheckArea * PlayerSide)
                    {
                        if (x == 0) continue;
                        RayCast2D rayCast = (RayCast2D)RayCast2D1.Duplicate();
                        rayCast.Name = "RayCast2D_" + (x * PlayerSide).ToString();
                        RayCasts.AddChild(rayCast);
                        rayCast.Position = new Vector2(x, rayCast.Position.Y);
                    }
                }
                else
                {
                    int Forward = 0;
                    int Back = 0;
                    if (ForwardWallDetector_1.IsColliding() || ForwardWallDetector_2.IsColliding())
                    {
                        Forward = -3;
                    }
                    if (BackWallDetector_1.IsColliding() || BackWallDetector_2.IsColliding())
                    {
                        Back = -3;
                    }
                    for (int x = -StepCheckArea; ; x -= StepCheckArea)
                    {
                        RayCast2D rayCast = RayCasts.GetNodeOrNull<RayCast2D>("RayCast2D_" + x.ToString());
                        if (rayCast != null && rayCast.IsColliding())
                        {
                            ++Back;
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int x = StepCheckArea; ; x += StepCheckArea)
                    {
                        RayCast2D rayCast = RayCasts.GetNodeOrNull<RayCast2D>("RayCast2D_" + x.ToString());
                        if (rayCast != null && rayCast.IsColliding())
                        {
                            ++Forward;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (Back <= Forward) // -1 потому что это со стороны игрока
                    {
                        Direction = PlayerSide;
                    }
                    else
                    {
                        Direction = -PlayerSide;
                    }
                    IsRunAway = true;
                }
            }
            else
            {
                if (DeltaRollCooldown <= 0)
                {
                    State = Statement.Roll;
                }
                else
                {
                    Move(delta);
                }
            }
        }
        else if (InCloseArea)
        {
            if (DeltaReload <= 0)
            {
                Direction = Math.Sign(Player.GlobalPosition.X - GlobalPosition.X);
                State = Statement.Attack;
            }
            else
            {
                bool IsCanMove = false;
                if (Math.Sign(GlobalPosition.X - Player.GlobalPosition.X) == Direction)
                {
                    if (RayCast2D1.IsColliding() || ForwardWallDetector_1.IsColliding() || ForwardWallDetector_2.IsColliding())
                    {
                        IsCanMove = true;
                    }
                }
                else
                {
                    if (RayCast2D2.IsColliding() || BackWallDetector_1.IsColliding() || BackWallDetector_2.IsColliding())
                    {
                        IsCanMove = true;
                    }
                }
                if (IsCanMove)
                {
                    Direction = Math.Sign(GlobalPosition.X - Player.GlobalPosition.X); // Поворачиваем от игрока
                    TurnAround();
                    Move(delta, false);
                }
                else
                {
                    Direction = Math.Sign(Player.GlobalPosition.X - GlobalPosition.X); // Поворачиваем на игрока
                    TurnAround();
                    Idle(delta);
                }
            }
        }
        else if (InFarArea)
        {
            Direction = Math.Sign(Player.GlobalPosition.X - GlobalPosition.X);
            if (DeltaReload <= 0)
            {
                State = Statement.Attack;
            }
            else
            {
                Idle(delta);
            }
        }
        else
        {
            IdleTime = RNG.Next(MinIdleTime, MaxIdleTime);
            State = Statement.Idle;
        }
    }

    public void Shoot() // Активируется с помощью Anim(AnimationPlayer)
    {
        Node Node_NewArrow = Arrow.Instantiate();
        Area2D NewArrow = Node_NewArrow as Area2D;
        GetParent().GetParent().AddChild(NewArrow);
        NewArrow.GlobalPosition = Marker.GlobalPosition;
        NewArrow.Set("Direction", Direction);
        NewArrow.CallDeferred("TurnAround");
        Sound_Shoot.Play(0);
    }

    public void Target_In_Area(Node2D Area)
    {
        if (Area.Name == "HurtBox" && Player.health > 0 && State != Statement.Battle && State != Statement.Attack)
        {
            IdleTime = 0;
            MoveTime = 0;
            State = Statement.Battle;
        }
    }

    public void Target_Out_Area(Node2D Area)
    {
        if (Area.Name == "HurtBox" && State == Statement.Battle)
        {
            IdleTime = RNG.Next(MinIdleTime, MaxIdleTime);
            State = Statement.Idle;
        }
    }

    public void GetDamage(int Damage)
    {

        Health -= Damage;
        DamageEffectTime = 0.1;
        AnimSprite.Modulate = new Color(1, 0.5f, 0.5f);
        FinishAttack();
        //Sound_Hurt.Play();

        if (Health <= 0)
        {
            Death();
        }
    }

    public async void Death()
    {
        Alive = false;
        IsRunAway = false;
        Anim.Play("Death");
        //Sound_Death.Play();
        await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
        QueueFree();
    }

    public void turn_on()
    {
        Sound_Attack.VolumeDb = DeffaultVolume_Sound_Attack;
        Sound_Shoot.VolumeDb = DeffaultVolume_Sound_Shoot;
    }

    public void turn_off()
    {
        Sound_Attack.VolumeDb = -80;
        Sound_Shoot.VolumeDb = -80;
    }
}
