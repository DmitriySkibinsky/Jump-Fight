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

    }