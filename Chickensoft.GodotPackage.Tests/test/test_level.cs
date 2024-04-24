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

    [Test]
    public void Test_OnCleanerAreaBodyEntered()
    {
        // Arrange
        var player = new player();
        player.Name = "Player";
        player.health = 100;
        player. smack = new AudioStreamPlayer();
        player.smack.Stream = ResourceLoader.Load<AudioStream>("Sounds/Smack");

        // Act
        level._on_cleaner_area_body_entered(player);

        // Assert
        player.Velocity.ShouldBe(new Vector2(0, 0));
        player.health.ShouldBe(80);
    }

    [Test]
    public void Test_OnCleanerAreaBodyEntered_2()
    {
        // Arrange
        var nonPlayerBody = new Node2D();

        // Act
        level._on_cleaner_area_body_entered(nonPlayerBody);

        // Assert
        level.ActivePlatforms.ShouldBeEmpty();
    }

    [Test]
    public void Test_RegisterPlatform()
    {
        // Arrange
        var platform = new Node2D();

        // Act
        level.RegisterPlatform(platform);

        // Assert
        level.ActivePlatforms.ShouldNotBeEmpty();
    }

    [Test]
    public void Test_UnregisterPlatform()
    {
        // Arrange
        var platform = new Node2D();

        // Act
        level.RegisterPlatform(platform);
        level.ActivePlatforms.ShouldNotBeEmpty();
        level.UnregisterPlatform(platform);

        // Assert
        level.ActivePlatforms.ShouldBeEmpty();
    }

    [Test]
    public void Test_SortByPlayerDistance()
    {
        // Arrange
        var obj1 = new Node2D();
        var obj2 = new Node2D();
        obj1.GlobalPosition = new Vector2(0,100);
        obj2.GlobalPosition = new Vector2(0,100);

        // Act
        var res = level.SortByPlayerDistance(obj1,obj2);
 
        // Assert
        res.ShouldBe(0);
    }

    [Test]
    public void Test_SortByPlayerDistance_2()
    {
        // Arrange
        var obj1 = new Node2D();
        var obj2 = new Node2D();
        obj1.GlobalPosition = new Vector2(0,100);
        obj2.GlobalPosition = new Vector2(0,110);

        // Act
        var res = level.SortByPlayerDistance(obj1,obj2);
 
        // Assert
        res.ShouldBe(-1);
    }

    [Test]
    public void Test_SortByPlayerDistance_3()
    {
        // Arrange
        var obj1 = new Node2D();
        var obj2 = new Node2D();
        obj1.GlobalPosition = new Vector2(0,110);
        obj2.GlobalPosition = new Vector2(0,100);

        // Act
        var res = level.SortByPlayerDistance(obj1,obj2);
 
        // Assert
        res.ShouldBe(1);
    }
}