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

public class IdleGolemTests : TestClass
{

    public IdleGolemTests(Node testScene) : base(testScene) { }

    private IdleGolem Idle;

    [Setup]
    public void Setup()
    {
        Idle = new IdleGolem();
    }

    [Test]
    public void Test_Enter()
    {
        // Arrange
		Idle.IsAwaitEnd = true;
        Idle.AnimationPlayer = Substitute.For<AnimationPlayer>();

		// Act
		Idle.Enter();

		// Assert
		Idle.IsAwaitEnd.ShouldBe(false);
    }

}