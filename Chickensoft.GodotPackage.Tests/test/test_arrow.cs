using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;
using NSubstitute;

public partial class ArrowTests : TestClass
{
    private Arrow Arrow;

    public ArrowTests(Node testScene) : base(testScene) { }

    [Setup]
    public void SetUp()
    {
        Arrow = new Arrow();
    }

    [Test]
    public void Test_Process()
    {
        //Arrange
        Arrow.Speed = 10;
        Arrow.Direction = 1;

        //Act
        Arrow._Process(0.1);

        //Assert
        Arrow.Position.ShouldBe(new Vector2(1,0));
        Arrow.LiveTime.ShouldBe(9,9);
    }

    [Test]
    public void Test_TurnAround()
    {
        //Arrange
        Arrow.Direction = 1;
        Arrow.AnimSprite = Substitute.For<Sprite2D>();
        Arrow.AnimSprite.FlipH = true;

        //Act
        Arrow.TurnAround();

        //Assert
        Arrow.AnimSprite.FlipH.ShouldBe(false);
    }


}