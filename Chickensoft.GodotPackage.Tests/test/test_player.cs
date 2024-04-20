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
using NUnit.Framework;

public class PlayerTests : TestClass
{

     public PlayerTests(Node testScene) : base(testScene) { }
    private player player;

    [Setup]
    public void Setup()
    {
        player = new player();
    }


    [Test]
	public void Test_teleport_to()
	{
		// Arrange
		float target_posX = 100.0f;

		// Act
		player.teleport_to(target_posX);

		// Assert
		player.GlobalPosition.X.ShouldBe(100);
	}

    [Test]
	public void Test_attack_boost()
	{
		// Act
		player.attack_boost();

		// Assert
		player.damage_basic.ShouldBe(20);
	}

    [Test]
	public void Test_jump_boost()
	{
		// Act
		player.jump_boost();

		// Assert
		player.jump_multiplier.ShouldBe(1.8f);
	}
}
