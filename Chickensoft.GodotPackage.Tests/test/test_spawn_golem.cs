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

public class SpawnGolemTests : TestClass
{

    public SpawnGolemTests(Node testScene) : base(testScene) { }

    private SpawnGolem Spawn;

    [Test]
    public void Test_Enter()
    {
        // Arrange
        Spawn = new SpawnGolem();
		Spawn.IsAnimFinished = true;
        var anim = Substitute.For<AnimationPlayer>();
        Spawn.AnimationPlayer = anim;

		// Act
		Spawn.Enter();

		// Assert
		Spawn.IsAnimFinished.ShouldBe(true);
    }

}