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

public class SpawnTests : TestClass
{

    public SpawnTests(Node testScene) : base(testScene) { }

    private Spawn Spawn;

    [Test]
    public void Test_Enter()
    {
        // Arrange
        Spawn = new Spawn();
		Spawn.IsAnimFinished = true;
        var anim = Substitute.For<AnimationPlayer>();
        Spawn.AnimationPlayer = anim;

		// Act
		Spawn.Enter();

		// Assert
		Spawn.IsAnimFinished.ShouldBe(true);
    }

}