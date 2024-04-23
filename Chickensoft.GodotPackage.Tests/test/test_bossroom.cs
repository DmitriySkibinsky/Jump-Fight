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
public class BossRoomTests: TestClass
{
    public BossRoomTests(Node testScene) : base(testScene) { }

    [Test]
    public void Test_on_enemy_killed()
    {
        // Arrange
		BossRoom BossRoom = new BossRoom();
        BossRoom.num_enemies = 10;
		
		// Act
		BossRoom._on_enemy_killed();

		// Assert
		BossRoom.num_enemies.ShouldBe(9);
    }


}