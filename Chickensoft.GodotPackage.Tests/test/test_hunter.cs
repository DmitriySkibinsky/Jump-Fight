using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;
using NSubstitute;

public partial class HunterTests : TestClass
{
    private Hunter Hunter;

    public HunterTests(Node testScene) : base(testScene) { }

    [Setup]
    public void SetUp()
    {
        Hunter = new Hunter();
        Hunter.Sound_Attack = new AudioStreamPlayer2D();
        Hunter.Sound_Shoot = new AudioStreamPlayer2D();
        Hunter.Anim = Substitute.For<AnimationPlayer>();
        Hunter.AnimSprite = Substitute.For<AnimatedSprite2D>();
    }

    [Test]
    public void Test_turn_off()
    {
		// Act
		Hunter.turn_off();

		// Assert
		Hunter.Sound_Attack.VolumeDb.ShouldBe(-80);
        Hunter.Sound_Shoot.VolumeDb.ShouldBe(-80);
    }

    [Test]
    public void Test_turn_on()
    {

		// Act
		Hunter.turn_on();

		// Assert
		Hunter.Sound_Attack.VolumeDb.ShouldBe(0);
        Hunter.Sound_Shoot.VolumeDb.ShouldBe(0);
    }

    [Test]
    public void Test_Death()
    {
        // Arrange
		Hunter.Alive = true;

		// Act
		Hunter.Death();

		// Assert
		Hunter.Alive.ShouldBe(false);

    }

        [Test]
    public void Test_GetDamage()
    {
        // Arrange
        Hunter.Health = 100;
        //Hunter.Sound_Hurt = new AudioStreamPlayer2D();

		// Act
		Hunter.GetDamage(10);

		// Assert
		Hunter.Health.ShouldBe(90);

    }

    [Test]
    public void Test_GetDamage2()
    {
        // Arrange
        Hunter.Health = 9;
        //Hunter.Sound_Hurt = new AudioStreamPlayer2D();

		// Act
		Hunter.GetDamage(10);

		// Assert
		Hunter.Alive.ShouldBe(false);

    }

    [Test]
    public void Test_Target_Out_Area()
    {
        // Arrange
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";    
        Hunter.State = Hunter.Statement.Battle;    

		// Act
		Hunter.Target_Out_Area(area);

		// Assert
		Hunter.State.ShouldBe(Hunter.Statement.Idle);

    }

    [Test]
    public void Test_Target_In_Area()
    {
        // Arrange
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";    
        Hunter.State = Hunter.Statement.Idle;  
        Hunter.Player = new player();
        Hunter.Player.health = 100;  

		// Act
		Hunter.Target_In_Area(area);

		// Assert
		Hunter.State.ShouldBe(Hunter.Statement.Battle);

    }	

    [Test]
    public void Test_Attack()
    {
        // Arrange  
        Hunter.DeltaReload = 0;    

		// Act
		Hunter.Attack(0.1);

		// Assert
		Hunter.DeltaReload.ShouldBe(Hunter.Reload);

    }

    [Test]
    public void Test_Roll()
    {
        // Arrange  
        Hunter.DeltaRollCooldown = 0;    

		// Act
		Hunter.Roll(0.1);

		// Assert
		Hunter.DeltaRollCooldown.ShouldBe(Hunter.RollCooldown);

    }	

    [Test]
    public void Test_Move()
    {
        // Arrange  
        Hunter.MoveTime = 10;  
        Hunter.State = Hunter.Statement.Move;  

		// Act
		Hunter.Move(0.1, true);

		// Assert
		Hunter.MoveTime.ShouldBe(9.9);

    }

    [Test]
    public void Test_Move2()
    {
        // Arrange  
        Hunter.MoveTime = 0;  
        Hunter.State = Hunter.Statement.Move;  

		// Act
		Hunter.Move(0.1, true);

		// Assert
		Hunter.MoveTime.ShouldBe(0);

    }	
    
    [Test]
    public void Test_Idle()
    {
        // Arrange  
        Hunter.IdleTime = 10;  
        Hunter.State = Hunter.Statement.Idle;  

		// Act
		Hunter.Idle(0.1);

		// Assert
		Hunter.IdleTime.ShouldBe(9.9);

    }

    [Test]
    public void Test_Idle2()
    {
        // Arrange  
        Hunter.IdleTime = 0;  
        Hunter.State = Hunter.Statement.Move;  

		// Act
		Hunter.Idle(0.1);

		// Assert
		Hunter.IdleTime.ShouldBe(0);

    }

    [Test]
    public void Test_TurnAround()
    {
        // Arrange
        Hunter.Direction = 1;
        Hunter.AnimSprite.FlipH = true;
        

		// Act
		Hunter.TurnAround();

		// Assert
		Hunter.AnimSprite.FlipH.ShouldBe(false);

    }

    [Test]
    public void Test_TurnAroundElements()
    {
        // Arrange
        var obj = Substitute.For<Node2D>();
        obj.Position = new Vector2(100,100);

		// Act
		Hunter.TurnAroundElements(obj);

		// Assert
		obj.Position.ShouldBe(new Vector2(-100,100));

    }

    [Test]
    public void Test_Process()
    {
        // Arrange
        Hunter.DeltaReload = 0.1;
        Hunter.State = Hunter.Statement.Move; 

		// Act
		Hunter._Process(0.1);

		// Assert
		Hunter.DeltaReload.ShouldBe(0);

    }    

    [Test]
    public void Test_Process1()
    {
        // Arrange
        Hunter.DamageEffectTime = 0.1;
        Hunter.AnimSprite.Modulate = new Color(10, 10, 10);

		// Act
		Hunter._Process(0.1);

		// Assert
		Hunter.DamageEffectTime.ShouldBe(0);
        Hunter.AnimSprite.Modulate.ShouldBe(new Color(1, 1, 1));


    }    

    [Test]
    public void Test_Process3()
    {
        // Arrange
        Hunter.DeltaRollCooldown = 0.1;

		// Act
		Hunter._Process(0.1);

		// Assert
		Hunter.DeltaRollCooldown.ShouldBe(0);

    } 

    
    	

}