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

	public Func<SceneTree> GetTree { get; set; }

    [Setup]
    public void Setup()
    {
        player = new player();
		player._Ready();
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
		player.damage_basic.ShouldBe(10);
	}

    [Test]
	public void Test_jump_boost()
	{
		// Act
		player.jump_boost();

		// Assert
		player.jump_multiplier.ShouldBe(1f);
	}

	[Test]
	public void Test_heal()
	{
		// Arrange
		player.health = 99;

		// Act
		player.heal(5);

		// Assert
		player.health.ShouldBe(100);
	}

	[Test]
	public void Test_DoDamage()
    {
        // Arrange
        var area = Substitute.For<Area2D>();
        var areaParent = Substitute.For<Node>();
        area.Name = "SomeOtherArea";
        player.LastAttack = 300ul; // Last attack was 300 milliseconds ago

        // Act
        player.DoDamage(area);

        // Assert
        player.LastAttack.ShouldBe((ulong)300);
    }

	[Test]
	public void Test_super_freeze()
    {
        // Arrange
        player.super_cooldown = false;

        // Act
        player.super_freeze();

        // Assert
        player.super_cooldown.ShouldBe(true);
    }

	[Test]
	public void Test_attack_freeze()
    {
        // Arrange
        player.attack_cooldown = false;

        // Act
        player.attack_freeze();

        // Assert
        player.attack_cooldown.ShouldBe(true);
    }

	[Test]
	public void Test_GetDamaged_Blocking()
	{
		// Arrange
		var initialHealth = player.health;
		var damage = 20;
		player.level = new Node2D();
		player.level.Name = "isBattleSection";

		// Act
		player.GetDamaged(damage);

		// Assert
		player.State.ShouldBe(player.StateMachine.DAMAGE);
		player.health.ShouldBe(initialHealth - damage );
	}

	[Test]
	public void Test_combat()
	{
		// Arrange
		player.combo=false;
		var anim = Substitute.For<AnimationPlayer>();
        player.animPlayer = anim;

		// Act
		player.combat();

		// Assert
		player.combo.ShouldBe(true);
	}

	[Test]
	public void Test_block_state()
	{
		// Arrange
		player.velocity.X=5;
        var anim = Substitute.For<AnimationPlayer>();
        player.animPlayer = anim;

		// Act
		player.block_state();

		// Assert
		player.velocity.X.ShouldBe(0);
	}

	[Test]
	public void Test_damage_state()
	{
		// Arrange
		player.State = player.StateMachine.MOVE;
		var anim = Substitute.For<AnimationPlayer>();
        player.animPlayer = anim;
		player.level = new Node2D();
		player.level.Name = "isBattleSection";

		// Act
		player.damage_state();

		// Assert
		player.State.ShouldBe( player.StateMachine.MOVE);
	}

	[Test]
	public void Test_death_state()
	{
		// Arrange
		player.State = player.StateMachine.MOVE;
		var anim = Substitute.For<AnimationPlayer>();
        player.animPlayer = anim;
		SceneTree tree = GetTree?.Invoke();
		if (tree != null)
		{
			// Получаем текущую сцену
			Node currentScene = tree.CurrentScene;
		
			// Act
			player.death_state();

			// Обновляем текущую сцену после совершения действия
			currentScene = tree.CurrentScene;

			// Assert
			currentScene.GetPath().ToString().ShouldBe("res://scenes/Menu/menu.tscn");
		}
	}

	[Test]
	public void Test_super_state()
	{
		// Arrange
		player.velocity.Y=0;
        var anim = Substitute.For<AnimationPlayer>();
        player.animPlayer = anim;
		player.State = player.StateMachine.MOVE;

		// Act
		player.super_state();

		// Assert
		player.velocity.X.ShouldBe(0);
		player.damage_multiplier.ShouldBe(3);
		player.State.ShouldBe( player.StateMachine.MOVE);
	}

	[Test]
	public void Test_attack3_state()
	{
		// Arrange
        var anim = Substitute.For<AnimationPlayer>();
        player.animPlayer = anim;
		player.State = player.StateMachine.MOVE;

		// Act
		player.attack3_state();

		// Assert
		player.damage_multiplier.ShouldBe(2);
		player.State.ShouldBe( player.StateMachine.MOVE);
	}

	[Test]
	public void Test_attack2_state()
	{
		// Arrange
        var anim = Substitute.For<AnimationPlayer>();
        player.animPlayer = anim;
		player.State = player.StateMachine.MOVE;

		// Act
		player.attack2_state();

		// Assert
		player.damage_multiplier.ShouldBe(1.25f);
		player.State.ShouldBe( player.StateMachine.MOVE);
	}

	[Test]
	public void Test_attack_state()
	{
		// Arrange
		player.velocity.Y=0;
        var anim = Substitute.For<AnimationPlayer>();
        player.animPlayer = anim;
		player.State = player.StateMachine.MOVE;

		// Act
		player.attack_state();

		// Assert
		player.velocity.Y=0;
		player.damage_multiplier.ShouldBe(1);
		player.State.ShouldBe( player.StateMachine.MOVE);
	}
	
}
