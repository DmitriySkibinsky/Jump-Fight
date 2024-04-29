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

public class BringerOfDeathTest : TestClass {

	public BringerOfDeathTest(Node testScene) : base(testScene) { }

    private BringerOfDeath bd;

    [Setup]
    public void Setup()
    {
        bd = new BringerOfDeath();
    }

    [Test]
    public void Test_Teleport()
    {
        // Arrange


        // Act
        bd.Teleport(10);

        // Assert
        bd.GlobalPosition.X.ShouldBe(10);
    }

    [Test]
    public void Test_PhysicsProcess1()
    {
        // Arrange
        bd.Direction = 1;

        // Act
        bd._PhysicsProcess(0.1);

        // Assert
        bd.velocity.X.ShouldBe(20);
    }

    [Test]
    public void Test_PhysicsProcess2()
    {
        // Arrange
        bd.Direction = 0;

        // Act
        bd._PhysicsProcess(0.1);

        // Assert
        bd.velocity.X.ShouldBe(-20);
    }

    [Test]
    public void Test_PhysicsProcess3()
    {
        // Arrange

        // Act
        bd._PhysicsProcess(0.1);

        // Assert
        bd.velocity.Y.ShouldBe(98);


    }

}