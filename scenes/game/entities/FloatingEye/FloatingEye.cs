using Godot;
using System;


public static class Trajectory
{
    // Настройки траекторий обозначены комментарием "Настройки", что бы их можно было найти
    private static int GameSpace = 525; // Ширина доступного игрового поля

    private static Random rnd = new Random();

    enum TrajectoryType
    {
        Linear,
        Circular,
        Loop,
        ZigZag,
    }

    // Список функций которые будут определять разную траекторию, выбор делается с помощью случайного числа(да-да, знаю, говнокод)

    /// <summary>
    /// Враг будет двигаться справа на лево и наоборот
    /// </summary>
    private static void SetLinearTrajectory(out Vector2[] WayPoints, Vector2 StartPos)
    {
        WayPoints = new Vector2[2]; // Создаём новый массив
        float Div = GameSpace / 2; // Маленькая оптимизация
        WayPoints[0] = StartPos - new Vector2(Div, 0);
        WayPoints[1] = StartPos + new Vector2(Div, 0);
    }

    /// <summary>
    /// Враг будет двигаться по кругу
    /// </summary>
    private static void SetCircularTrajectory(out Vector2[] WayPoints, Vector2 StartPos)
    {
        // Настройки
        float Angle = Mathf.DegToRad(10); // Угл по которому будем делиться круг !!!! ДОЛЖНО ДЕЛИТЬСЯ НА 360 БЕЗ ОСТАТКА
        int FlatY = rnd.Next(40, 75); // Сплющиваем круг на рандомное значение
        Vector2 Flatten = new Vector2(1, FlatY/100); // Насколько будет приплюснут круг


        Vector2 Vect = new Vector2(GameSpace / 2, 0); // Вектор который будем вращать
        int N = (int)(Mathf.Pi * 2 / Angle); // Определяем кол-во сегментов круга
        WayPoints = new Vector2[N]; // Создаём новый массив

        for (int i = 0; i < N; i++)
        {
            WayPoints[i] = StartPos + Vect.Rotated(Angle * i) * Flatten;
        }
    }

    /// <summary>
    /// Враг будет двигаться по петле(знак бесконечности)
    /// </summary>
    private static void SetLoopTrajectory(out Vector2[] WayPoints, Vector2 StartPos)
    {
        //Настройки
        float DivGS = GameSpace / 4; // Радиус кругов у петли
        float Angle = Mathf.DegToRad(10); // Угл по которому будем делить круг !!!! ДОЛЖНО ДЕЛИТЬСЯ НА 180 БЕЗ ОСТАТКА
        int FlatY = rnd.Next(50, 100); // Сплющиваем круг на рандомное значение
        Vector2 Flatten = new Vector2(1, FlatY/100); // Насколько будет приплюснута петля
        int Side = rnd.Next(1) == 1 ? 1 : -1; // Направление


        int N = (int)(Mathf.Pi / Angle); // Определяем кол-во сегментов у полуокружностей
        Vector2 Vect = new Vector2(0, DivGS); // Вектор который будем вращать

        WayPoints = new Vector2[N * 2]; // Создаём новый массив

        Vector2 Displacement = new Vector2(DivGS, 0);
        for (int i = 0; i < N; i++) // Создаём первую полуокружность
        {
            WayPoints[i] = StartPos + Displacement + Vect.Rotated(Angle * i * Side) * Flatten;
        }

        for (int i = 0; i < N; i++) // Создаём левую полуокружность
        {
            WayPoints[N + i] = StartPos - Displacement + Vect.Rotated(-Angle * i * Side) * Flatten;
        }
    }

    /// <summary>
    /// Враг будет двигаться зигзагом
    /// </summary>
    private static void SetZigZagTrajectory(out Vector2[] WayPoints, Vector2 StartPos)
    {
        // Настройки
        int N = rnd.Next(6, 13);  // Генерируем число [4;11)  Кол-во вершин
        int Side = rnd.Next(1) == 1 ? 1 : -1; // В какую сторону он пойдёт сперва
        int Amplitude = rnd.Next(25, 51); // Высота вершин


        int Step = GameSpace / (N);

        WayPoints = new Vector2[(N + 1) * 2]; // Создаём новый массив, кол-во вершин + начало и ещё такой же обратный путь
        Vector2 Div = new Vector2(GameSpace / 2, 0); // Маленькая оптимизация
        WayPoints[0] = StartPos - Div; // Начало
        WayPoints[N + 1] = StartPos + Div; // Конец(не последний вейпоинт так как нужно ещё обратно)

        for (int i = 0; i < N; i++)
        {
            WayPoints[i + 1] = new Vector2(WayPoints[0].X + (Step * i + Step / 2), StartPos.Y + Amplitude * Side);
            Side *= -1; // Меняем сторону что бы вершина в след. раз в другую сторону смотрела
        }

        Side = 1 == rnd.Next(1) ? 1 : -1;
        for (int i = 0; i < N; i++)
        {
            WayPoints[i + 2 + N] = new Vector2(WayPoints[N + 1].X - (Step * i + Step / 2), StartPos.Y + Amplitude * Side);
            Side *= -1;
        }
    }


    /// <summary>
    /// Случайно выбираает путь/траекторию по которому движется враг
    /// </summary>
    public static void SetRandomTrajectory(ref Vector2[] WayPoints, Vector2 StartPos)
    {
        TrajectoryType trajectory = (TrajectoryType)rnd.Next(0,3); // Генерируем число [0;4)
        switch (trajectory)
        {
            case TrajectoryType.Linear:
                SetLinearTrajectory(out WayPoints, StartPos);
                break;
            case TrajectoryType.Circular:
                SetCircularTrajectory(out WayPoints, StartPos);
                break;
            case TrajectoryType.Loop:
                SetLoopTrajectory(out WayPoints, StartPos);
                break;
            case TrajectoryType.ZigZag:
                SetZigZagTrajectory(out WayPoints, StartPos);
                break;
        }
    }

}
public partial class FloatingEye : CharacterBody2D
{
    private int Speed = 125;
    private int Damage = 5;
    private int direction = 1;
    private bool Alive = true;

    private Vector2[] WayPoints; // Путь/Траектория по которому движеться враг
    private Vector2 StartPos; // Стартовая позиция
    private int CurrentWayPoint; // Указывает на индекс Вейпоинта к которому он движется

    private CustomSignals _customSignals;

    private AnimatedSprite2D Anim;

    private player Player;

    public override void _Ready()
    {
        Area2D HitBoxes = GetNode<Area2D>("HitBoxes");
        Area2D HurtBoxes = GetNode<Area2D>("HurtBoxes");
        _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        Anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        Player = (player)GetTree().GetFirstNodeInGroup("Player");
        //CollisionShape2D CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        StartPos = this.Position; // Сохраняем стартовую позицию

        Trajectory.SetRandomTrajectory(ref WayPoints, StartPos);

        Random rnd = new Random();
        CurrentWayPoint = rnd.Next(WayPoints.Length);  // Генерируем число [0;`Кол-во Вейпоинтов`)
        this.Position = WayPoints[CurrentWayPoint]; // Ставим врага на рандомную позицию его маршрута

        //HurtBoxes.AreaEntered += GetDamage;
        //HitBoxes.BodyEntered += Attack; Оно не работает
    }

    public override void _Process(double delta)
    {
        if (!Alive)
        {
            return;
        }

        Vector2 Direction = WayPoints[CurrentWayPoint] - Position; // Ради оптимизации и читабильности вынес за формулу
        Vector2 NewPos = Position + Direction / Direction.Length() * Speed * (float)delta; // Текущая позиция + Еденичный вектор направления * Скорость

        if (Direction.Length() == 0 || Math.Sign(WayPoints[CurrentWayPoint].X - Position.X) != Math.Sign(WayPoints[CurrentWayPoint].X - NewPos.X)) //Если враг уже добрался до вейпоинта
        {
            Position = WayPoints[CurrentWayPoint];

            ++CurrentWayPoint;
            if (CurrentWayPoint == WayPoints.Length)
            {
                CurrentWayPoint = 0;
            }

            if (Position.X - WayPoints[CurrentWayPoint].X > 0)
            {
                Anim.FlipH = true;
            }
            else
            {
                Anim.FlipH = false;
            }
        }
        else //Если враг не добрался до вейпоинта
        {
            Position = NewPos; // Применяем новую позицию
        }
    }

    public void _on_hurt_boxes_body_entered(Node2D Body)
    {
        if (Body == Player && Alive && Player.Velocity.Y >= 0)
        {
            Player.Velocity = new Vector2(Player.Velocity.X, -500);
            Player.MoveAndSlide();
            death();
            
        }
    }

    public void Attack(Node2D Body)
    {
        if (Body == Player && Alive)
        {
            //_customSignals.EmitSignal(nameof(CustomSignals.DamagePlayer), Damage);
            Player.GetDamaged(Damage);
        }
    }

    public async void death()
    {
        Alive = false;
        Anim.Play("Death");
        await ToSignal(Anim, AnimatedSprite2D.SignalName.AnimationFinished);
        QueueFree();
    }
}
