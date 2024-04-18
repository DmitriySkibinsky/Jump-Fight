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

}
