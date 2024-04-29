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

public class IdleTests : TestClass
{

    public IdleTests(Node testScene) : base(testScene) { }

    private Idle Idle;

    [Setup]
    public void Setup()
    {
        Idle = new Idle();
    }

    [Test]
    public void Test_Enter()
    {
        // Arrange
		Idle.IsAwaitEnd = true;
        var anim = Substitute.For<AnimationPlayer>();
        Idle.AnimationPlayer = anim;

		// Act
		Idle.Enter();

		// Assert
		Idle.IsAwaitEnd.ShouldBe(false);
    }

}