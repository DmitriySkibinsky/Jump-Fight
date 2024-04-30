using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;
using NSubstitute;

public partial class BarbarianTests : TestClass
{
    private Barbarian Barbarian;

    public BarbarianTests(Node testScene) : base(testScene) { }

    [Setup]
    public void SetUp()
    {
        Barbarian = new Barbarian();
        Barbarian.Sound_Attack = new AudioStreamPlayer2D();
        Barbarian.Sound_Hit = new AudioStreamPlayer2D();
        Barbarian.Sound_Hurt = new AudioStreamPlayer2D();
        Barbarian.Sound_Death = new AudioStreamPlayer2D();
        Barbarian.Anim = Substitute.For<AnimatedSprite2D>();
    }

    [Test]
    public void Test_turn_off()
    {
		// Act
		Barbarian.turn_off();

		// Assert
		Barbarian.Sound_Attack.VolumeDb.ShouldBe(-80);
        Barbarian.Sound_Hit.VolumeDb.ShouldBe(-80);
        Barbarian.Sound_Hurt.VolumeDb.ShouldBe(-80);
        Barbarian.Sound_Death.VolumeDb.ShouldBe(-80);
    }

    [Test]
    public void Test_turn_on()
    {

		// Act
		Barbarian.turn_on();

		// Assert
		Barbarian.Sound_Attack.VolumeDb.ShouldBe(0);
        Barbarian.Sound_Hit.VolumeDb.ShouldBe(0);
        Barbarian.Sound_Hurt.VolumeDb.ShouldBe(0);
        Barbarian.Sound_Death.VolumeDb.ShouldBe(0);
    }

    [Test]
    public void Test_Death()
    {
        // Arrange
		Barbarian.Alive = true;

		// Act
		Barbarian.Death();

		// Assert
		Barbarian.Alive.ShouldBe(false);

    }

    [Test]
    public void Test_FinishPursue()
    {
        // Arrange
		Barbarian.IsPursue = true;
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";
        Barbarian.Alive = true;
        Barbarian.Player = new player();
        Barbarian.Player.health = 100;
        

		// Act
		Barbarian.FinishPursue(area);

		// Assert
		Barbarian.IsPursue.ShouldBe(false);

    }

    [Test]
    public void Test_FinishPursue2()
    {
        // Arrange
		Barbarian.IsPursue = true;
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";
        Barbarian.Alive = true;
        Barbarian.Player = new player();
        Barbarian.Player.health = 100;
        Barbarian.Anim = Substitute.For<AnimatedSprite2D>();
        Barbarian.State = Barbarian.Statement.Run;
        

		// Act
		Barbarian.FinishPursue(area);

		// Assert
		Barbarian.State.ShouldBe(Barbarian.Statement.Idle);

    }		

    [Test]
    public void Test_StartPursue()
    {
        // Arrange
		Barbarian.IsPursue = false;
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";
        Barbarian.Alive = true;
        Barbarian.Player = new player();
        Barbarian.Player.health = 100;
        
        

		// Act
		Barbarian.StartPursue(area);

		// Assert
		Barbarian.IsPursue.ShouldBe(true);

    }	

    [Test]
    public void Test_StartPursue2()
    {
        // Arrange
		Barbarian.IsPursue = false;
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";
        Barbarian.Alive = true;
        Barbarian.Player = new player();
        Barbarian.Player.health = 100;
        Barbarian.Anim = Substitute.For<AnimatedSprite2D>();
        

		// Act
		Barbarian.StartPursue(area);

		// Assert
		Barbarian.State.ShouldBe(Barbarian.Statement.Run);

    }	

    [Test]
    public void Test_GetDamage()
    {
        // Arrange
        Barbarian.Anim = Substitute.For<AnimatedSprite2D>();
        Barbarian.Health = 100;
        Barbarian.Sound_Hurt = new AudioStreamPlayer2D();
        

		// Act
		Barbarian.GetDamage(10);

		// Assert
		Barbarian.Health.ShouldBe(90);

    }

    [Test]
    public void Test_GetDamage2()
    {
        // Arrange
        Barbarian.Anim = Substitute.For<AnimatedSprite2D>();
        Barbarian.Health = 9;
        Barbarian.Sound_Hurt = new AudioStreamPlayer2D();
        

		// Act
		Barbarian.GetDamage(10);

		// Assert
		Barbarian.Alive.ShouldBe(false);

    }

    [Test]
    public void Test_PrepareAttack()
    {
        // Arrange
		Barbarian.IsPursue = false;
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";
        Barbarian.Alive = true;
        Barbarian.Player = new player();
        Barbarian.Player.health = 100;
        Barbarian.Anim = Substitute.For<AnimatedSprite2D>();
        Barbarian.State = Barbarian.Statement.Run;
        

		// Act
		Barbarian.PrepareAttack(area);

		// Assert
		Barbarian.State.ShouldBe(Barbarian.Statement.PrepareAttack);

    }	

    [Test]
    public void Test_TurnAround()
    {
        // Arrange
        Barbarian.Anim = Substitute.For<AnimatedSprite2D>();
        Barbarian.Health = 100;
        Barbarian.Sound_Attack = new AudioStreamPlayer2D();
        Barbarian.Direction = 1;
        Barbarian.Anim.FlipH = true;
        

		// Act
		Barbarian.TurnAround();

		// Assert
		Barbarian.Anim.FlipH.ShouldBe(false);

    }

    [Test]
    public void Test_TurnAroundElements()
    {
        // Arrange
        var obj = Substitute.For<Node2D>();
        obj.Position = new Vector2(100,100);

		// Act
		Barbarian.TurnAroundElements(obj);

		// Assert
		obj.Position.ShouldBe(new Vector2(-100,100));

    }

}