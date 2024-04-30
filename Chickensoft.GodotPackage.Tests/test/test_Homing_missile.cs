using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;
using NSubstitute;

public partial class HomingMissileTests : TestClass
{
    private HomingMissile HomingMissile;

    public HomingMissileTests(Node testScene) : base(testScene) { }

    [Setup]
    public void SetUp()
    {
        HomingMissile = new HomingMissile();
    }
    
    [Test]
    public void Test_Start()
    {
        //Arrange
        var _target = new Node2D();
        var _transform = new Transform2D();

        //Act
        HomingMissile.Start(_transform,_target);

        //Assert
        HomingMissile.GlobalTransform.ShouldBe(_transform);
        HomingMissile.target.ShouldBe(_target);
    }

    [Test]
    public void Test_Seek()
    {
        //Arrange
        HomingMissile.target = new Node2D();

        //Act
        var seek = HomingMissile.Seek();

        //Assert
        seek.ShouldBe(new Vector2(0,0));
    }

    [Test]
    public void Test_Process()
    {
        //Arrange
        HomingMissile.IsHoming = true;
        HomingMissile.target = new Node2D();

        //Act
        HomingMissile._PhysicsProcess(0.1);

        //Assert
        HomingMissile.acceleration.ShouldBe(new Vector2(0,0));

    }

    [Test]
    public void Test_Process2()
    {
        //Arrange
        HomingMissile.IsHoming = false;
        HomingMissile.target = new Node2D();

        //Act
        HomingMissile._PhysicsProcess(0.1);

        //Assert
        HomingMissile.acceleration.ShouldBe(new Vector2(0,0));
        HomingMissile.Rotation.ShouldBe(0);

    }

}