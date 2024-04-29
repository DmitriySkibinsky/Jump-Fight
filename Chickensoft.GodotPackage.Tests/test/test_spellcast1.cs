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

public class SpellCast1Tests : TestClass
{

    public SpellCast1Tests(Node testScene) : base(testScene) { }

    private SpellCast1 SpellCast1;
    [Setup]
    public void Setup()
    {
        SpellCast1 = new SpellCast1();
    }

    [Test]
    public void Test_Enter()
    {
        // Arrange
		SpellCast1.IsAwaitEnd = true;
        var anim = Substitute.For<AnimationPlayer>();
        SpellCast1.AnimationPlayer = anim;

		// Act
		SpellCast1.Enter();

		// Assert
		SpellCast1.IsAwaitEnd.ShouldBe(false);
    }

    [Test]
    public void Test_Process()
    {
        // Arrange
        SpellCast1.Player = new player();
        SpellCast1.Player.GlobalPosition = new Vector2(100,100);
        SpellCast1.IsAwaitEnd = false;
        SpellCast1.Portal = new RangedAttack();
        SpellCast1.Portal.GlobalPosition = new Vector2(0,0);

		// Act
		SpellCast1._Process(0.1f);

		// Assert
		SpellCast1.Portal.GlobalPosition.ShouldBe(new Vector2(25,165));
    }
}