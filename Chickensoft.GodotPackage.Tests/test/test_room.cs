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
using NSubstitute;
public class RoomTests: TestClass
{
    public RoomTests(Node testScene) : base(testScene) { }

    [Test]
    public void Test_on_enemy_killed()
    {
        // Arrange
		Room Room = new Room();
        Room.num_enemies = 10;
		
		// Act
		Room._on_enemy_killed();

		// Assert
		Room.num_enemies.ShouldBe(9);
    }


}