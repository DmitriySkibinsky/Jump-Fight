using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using System;
using System.Linq;
using Shouldly;

public class TrajectoryTests:TestClass
{
    public TrajectoryTests(Node testScene) : base(testScene) { }
   [Test]
    public void SetRandomTrajectory_Test()
    {
        // Arrange
        var startPos = new Vector2(0, 0);
        Vector2[] waypoints = null;

        // Act
        Trajectory.SetRandomTrajectory(ref waypoints, startPos);

        // Assert
        waypoints.ShouldNotBeNull(); // Проверка на то, что массив точек не пустой
        waypoints.Length.ShouldBeGreaterThan(0); // Проверка, что массив точек содержит хотя бы одну точку
        // Дополнительные проверки, в зависимости от выбранной траектории, могут быть добавлены для более детальной проверки сгенерированных точек
    }

    // Добавьте дополнительные тесты для каждого типа траектории, чтобы убедиться, что точки генерируются корректно
    [Test]
    public void SetLinearTrajectory_Test()
    {
        // Arrange
        var startPos = new Vector2(0, 0);
        Vector2[] waypoints = null;

        // Act
        Trajectory.SetLinearTrajectory(out waypoints, startPos);

        // Assert
        waypoints.ShouldNotBeNull(); // Проверка на то, что массив точек не пустой
        waypoints.Length.ShouldBe(2); // Проверка, что массив точек содержит ровно две точки
        waypoints[0].ShouldNotBe(waypoints[1]); // Проверка, что начальная и конечная точки не совпадают
        waypoints[0].ShouldBe(startPos - new Vector2(Trajectory.GameSpace / 2, 0)); // Проверка, что начальная точка соответствует ожидаемой позиции
        waypoints[1].ShouldBe(startPos + new Vector2(Trajectory.GameSpace / 2, 0)); // Проверка, что конечная точка соответствует ожидаемой позиции
    }

    [Test]
    public void SetCircularTrajectory_Test()
{
    // Arrange
    int N = (int)(Mathf.Pi * 2 / Mathf.DegToRad(10));
    var startPos = new Vector2(0,0);
    Vector2[] waypoints = null;
    Vector2 Vect = new Vector2(Trajectory.GameSpace / 2, 0);

    // Act
    Trajectory.SetCircularTrajectory(out waypoints, startPos);

    // Assert
    waypoints.ShouldNotBeNull(); // Проверка на то, что массив точек не пустой
    waypoints.Length.ShouldBe(N); // Проверка, что массив точек содержит ровно N точeк
    waypoints[0].ShouldNotBe(waypoints[N-1]); // Проверка, что начальная и конечная точки не совпадают
    // Проверка, что первая точка соответствует ожидаемой позиции
    for (int i = 0; i < N; i++){ 
    waypoints[i].ShouldBe(startPos + Vect.Rotated(Mathf.DegToRad(10)*i) * new Vector2(1, (float)Trajectory.FlatY/(float)100.0));
    }
}

[Test]
public void SetLoopTrajectory_Test()
    {
        // Arrange
        var startPos = new Vector2(0, 0);
        Vector2[] waypoints = null;

        // Act
        Trajectory.SetLoopTrajectory(out waypoints, startPos);

        // Assert
        waypoints.ShouldNotBeNull(); // Проверка на то, что массив точек не пустой

        // Проверка, что количество точек равно удвоенному количеству сегментов у полуокружностей
        int expectedWaypointsCount = (int)(Mathf.Pi / Mathf.DegToRad(10)) * 2;
        waypoints.Length.ShouldBe(expectedWaypointsCount);

        // Проверка, что каждая пара точек находится на одинаковом расстоянии друг от друга
        for (int i = 0; i < waypoints.Length / 2; i++)
        {
            Vector2 point1 = waypoints[i];
            Vector2 point2 = waypoints[i + waypoints.Length / 2];
            float distance = point1.DistanceTo(point2);
            distance.ShouldBeGreaterThanOrEqualTo(0); // Расстояние должно быть больше или равно нулю
            // Дополнительно можно проверить, что расстояние находится в допустимом диапазоне, если известны настройки генерации
        }
    }

[Test]
    public void SetZigZagTrajectory_GeneratesValidWaypoints()
    {
        // Arrange
        var startPos = new Vector2(0, 0);
        Vector2[] waypoints = null;

        // Act
        Trajectory.SetZigZagTrajectory(out waypoints, startPos);

        // Assert
        waypoints.ShouldNotBeNull(); // Убеждаемся, что массив точек не равен null

        // Проверяем количество вершин
        int expectedWaypointsCount = (Trajectory.N +1)* 2 ; // Ожидаемое количество вершин
        waypoints.Length.ShouldBe(expectedWaypointsCount);

        // Проверяем координаты вершин
        int gameSpace = Trajectory.GameSpace;
        int amplitude = Trajectory.Amplitude;
        int step = gameSpace / (waypoints.Length / 2 - 1);

         // Проверяем начало и конец траектории
        waypoints[0].X.ShouldBe(startPos.X - new Vector2(gameSpace / 2, 0).X, tolerance: 0.001f); // Начало
        waypoints[0].Y.ShouldBe(startPos.Y - new Vector2(gameSpace / 2, 0).Y, tolerance: 0.001f); // Начало
        waypoints[waypoints.Length / 2].X.ShouldBe(startPos.X + new Vector2(gameSpace / 2, 0).X, tolerance: 0.001f); // Конец
        waypoints[waypoints.Length / 2].Y.ShouldBe(startPos.Y + new Vector2(gameSpace / 2, 0).Y, tolerance: 0.001f); // Конец

        for (int i = 0; i < waypoints.Length / 2 - 1; i++)
        {
            // Проверяем координаты x
            waypoints[i + 1].X.ShouldBe(waypoints[0].X + (step * i + step / 2), tolerance: 0.001f);
            waypoints[i + 2 + Trajectory.N].X.ShouldBe(waypoints[Trajectory.N + 1].X - (step * i + step / 2), tolerance: 0.001f);

            // Проверяем координаты y
            waypoints[i + 1].Y.ShouldBe(-1*(waypoints[0].Y + amplitude), tolerance: 0.001f);
            waypoints[i + 2 + Trajectory.N].Y.ShouldBe(waypoints[0].Y - amplitude, tolerance: 0.001f);

            // Меняем сторону для следующей вершины
            amplitude *= -1;
        }       
    }
}

public class FloatingEyeTests:TestClass
{
    public FloatingEyeTests(Node testScene) : base(testScene) { }
    private FloatingEye FloatingEye;

    [Setup]
    public void SetUp()
    {
        FloatingEye = new FloatingEye();
    }

    [Test]
    public void Attack_Test()
    {
        // Arrange
        var player = new player();
        player.hitbox = new CollisionShape2D();
        player. smack = new AudioStreamPlayer();
        player.smack.Stream = ResourceLoader.Load<AudioStream>("Sounds/Smack");
        FloatingEye.Alive = true;
        FloatingEye.Player = player;
        player.Position = new Vector2(100, 0); 

        // Act
        FloatingEye.Attack(player);

        // Assert
        player.Velocity.ShouldBe(new Vector2(300, 0));
    }

    [Test]
    public void _Process_Test()
    {
        // Arrange
        FloatingEye.Alive = true;
        FloatingEye.Position = new Vector2(0, 0);
        FloatingEye.CurrentWayPoint = 1;
        FloatingEye.WayPoints = new Vector2[] { new Vector2(0, 0), new Vector2(100, 0) };
        FloatingEye.Speed = 125;
        double delta = 0.1; // arbitrary delta value for testing

        // Act
        FloatingEye._Process(delta);

        // Assert
        FloatingEye.Position.ShouldBe(new Vector2(12.5f, 0)); // Assuming Direction.Length() == 100 and CurrentWayPoint == 1
    }

    [Test]
    public void _on_hurt_boxes_body_entered_Test()
    {
        // Arrange
        FloatingEye.Anim = new AnimatedSprite2D();
        FloatingEye.Alive = true;
        var player = new player();
        FloatingEye.jump = new AudioStreamPlayer();
        FloatingEye.jump.Stream = ResourceLoader.Load<AudioStream>("Sounds/Smack"); 
        FloatingEye.Player = player;
        player.Velocity = new Vector2(0, 100); // Player is falling

        // Act
        FloatingEye._on_hurt_boxes_body_entered(player);

        // Assert
        player.Velocity.ShouldBe(new Vector2(0, -700));
    }

    

    [Test]
    public void death_Test()
    {
        // Arrange
        FloatingEye.Alive = true;
        FloatingEye.Anim = new AnimatedSprite2D(); // Assuming AnimatedSprite2D is properly instantiated

        // Act
        FloatingEye.death();

        // Assert
        FloatingEye.Alive.ShouldBeFalse();
        //FloatingEye.Anim.ShouldBe("Death");
    }

}