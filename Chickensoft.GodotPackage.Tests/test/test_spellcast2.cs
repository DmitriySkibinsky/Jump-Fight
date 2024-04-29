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

public class SpellCast2Tests : TestClass
{

    public SpellCast2Tests(Node testScene) : base(testScene) { }

    private SpellCast2 SpellCast2;
    [Setup]
    public void Setup()
    {
        SpellCast2 = new SpellCast2();
    }

    [Test]
    public void Test_Enter()
    {
        // Arrange
		SpellCast2.IsSpellCastFinished = true;
        var anim = Substitute.For<AnimationPlayer>();
        SpellCast2.AnimationPlayer = anim;

		// Act
		SpellCast2.Enter();

		// Assert
		SpellCast2.IsSpellCastFinished.ShouldBe(false);
    }

    [Test]
    public void Test_Attack()
    {
        // Arrange
        SpellCast2.portal = Substitute.For<Node2D>();

		// Act
		SpellCast2.Attack();

		// Assert
		SpellCast2.portal.GlobalPosition.ShouldBe(new Vector2(0,0));
    }

    [Test]
    public void Test_Attack1()
    {
		// Act
		SpellCast2.Attack();

		// Assert
		SpellCast2.Portals.ShouldNotBeNull();
    }
}