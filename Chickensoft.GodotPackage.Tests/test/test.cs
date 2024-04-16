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

public class PlayerTest : TestClass {
	private readonly ILog _log = new GDLog(nameof(PlayerTest));

	private player player;

	public PlayerTest(Node testScene) : base(testScene) { }


	[Setup]

	public void SetUp()
	{
		player = new player();
		_log.Print("Setup");
	}



	[Test]
	public void Test_OnArea2DBodyEntered()
	{
		// Arrange
		player body = new player();
		body.velocity.Y = 100;
		BreackablePlatform pl = new BreackablePlatform();
		// Act
		pl._on_area_2d_body_entered(body);

		// Assert
		body.Velocity.Y.ShouldBe(0f);
	}

	[Test]
	public void OnDetectorBodyEntered_EnemyBody_Damages()
	{
		var enemy = new Node2D();
		enemy.Name = "Enemy";
		var d_area = new damage_area(); 
		bool damaged = false;
		d_area.damaged = damaged;

		// Act
		d_area.OnDetectorBodyEntered(enemy);

		// Assert
		d_area.damage.ShouldBe(100);

	}


	[Test]
	public void Test_teleport_to()
	{
		// Arrange
		float target_posX = 100.0f;

		// Act
		player.teleport_to(target_posX);

		// Assert
		player.GlobalPosition.X.ShouldBe(100);
	}


	[Test]   
	public void Test__PhysicsProcess_UpdatesVelocity()
	{
		// Arrange
		double delta = 0.016;

		// Act
		player._PhysicsProcess(delta);

		// Assert
		player.Velocity.Y.ShouldBe(player.velocity.Y + player.gravity * (float)delta);
	}
}
