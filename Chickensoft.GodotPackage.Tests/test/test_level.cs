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

public class LevelTest : TestClass {
	private readonly ILog _log = new GDLog(nameof(PlayerTest));

	private player player;
	private level level;
    private Camera2D camera;

	public LevelTest(Node testScene) : base(testScene) { }


	[Setup]

	public void SetUp()
	{
		player = new player();
		level = new level();
        camera = new Camera2D();
        level.player = player;
        level.camera = camera;
		_log.Print("Setup");
	}

	[Test]
    public void Process_WhenPlayerBelowCameraAndMoveableCameraIsTrue_YCoordinateIsAdjusted()
    {
        // Arrange
        player.Position = new Vector2(0, -10); 
        level.isBattleSection = true;

        // Act
        level._Process(0.1); 

        // Assert
        camera.Position.Y.ShouldBe(player.Position.Y+20);
		camera.Position.X.ShouldBe(player.Position.X);
    }

    [Test]
    public void Process_WhenPlayerBelowCameraAndMoveableCameraIsFalse_YCoordinateIsNotAdjusted()
    {
        // Arrange
        player.Position = new Vector2(0, -10); // Player below the camera
        level.isBattleSection = false;

        // Act
        level._Process(0.1); // Assuming delta value

        // Assert
        camera.Position.X.ShouldBe(player.Position.X);
        camera.Position.Y.ShouldBe(player.Position.Y);
    }

    [Test]
    public void Process_WhenPlayerAboveCamera_YCoordinateIsNotAdjusted()
    {
        // Arrange
        player.Position = new Vector2(0, 0); 

        // Act
        level._Process(0.1); 

        // Assert
        camera.Position.X.ShouldBe(player.Position.X);
        camera.Position.Y.ShouldBe(player.Position.Y);
    }

    [Test]
    public void CleanerAreaBodyEntered_WhenPlayerCollides_PlayerGetDamagedAndPositionIsAdjusted()
    {
        // Arrange
        var body = new Node2D();
        body.Name = "Player";
        var initialPlayerPosition = player.Position;

        // Act
        level._on_cleaner_area_body_entered(body);

        // Assert
        player.Position.X.ShouldBe(initialPlayerPosition.X);
        player.Position.Y.ShouldBe(camera.Position.Y);
        //player.GetDamageCalled.ShouldBe(20);
    }

    /*[Test]
	public async Task CleanerAreaBodyEntered_WhenNonPlayerBodyCollides_BodyIsFreed()
{
    // Arrange
    var body = new Node2D();
  
    // Act
    level._on_cleaner_area_body_entered(body);

    // Wait for one frame
    await Task.Delay(1);

    // Assert
    body.GetParent().ShouldBeNull();
}*/
}
