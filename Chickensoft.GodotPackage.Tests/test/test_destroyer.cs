using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;
using NSubstitute;

public partial class DestroyerTests : TestClass
{
    private Destroyer Destroyer;

    public DestroyerTests(Node testScene) : base(testScene) { }

    [Setup]
    public void SetUp()
    {
        Destroyer = new Destroyer();
        Destroyer.Sound_ArmorHurt = new AudioStreamPlayer2D();
        Destroyer.Sound_BlockHurt = new AudioStreamPlayer2D();
        Destroyer.Sound_Hurt = new AudioStreamPlayer2D();
        Destroyer.Sound_Hit = new AudioStreamPlayer2D();
        Destroyer.Sound_RushAttack = new AudioStreamPlayer2D();
        Destroyer.Sound_StartRushAttack = new AudioStreamPlayer2D();
        Destroyer.Sound_ArmorBlock = new AudioStreamPlayer2D();
        Destroyer.Sound_Death = new AudioStreamPlayer2D();
        Destroyer.Sound_ArmorBreak = new AudioStreamPlayer2D();
        Destroyer.Sound_Explosion = new AudioStreamPlayer2D();
        Destroyer.Sound_Shoot = new AudioStreamPlayer2D();
        Destroyer.Anim = Substitute.For<AnimationPlayer>();
        Destroyer.AnimSprite = Substitute.For<AnimatedSprite2D>();
        Destroyer.Player = new player();
    }

    [Test]
    public void Test_turn_off()
    {
		// Act
		Destroyer.turn_off();

		// Assert
		Destroyer.Sound_ArmorHurt.VolumeDb.ShouldBe(-80);
        Destroyer.Sound_BlockHurt.VolumeDb.ShouldBe(-80);
        Destroyer.Sound_Hurt.VolumeDb.ShouldBe(-80);
        Destroyer.Sound_Hit.VolumeDb.ShouldBe(-80);

    }

    [Test]
    public void Test_turn_on()
    {

		// Act
		Destroyer.turn_on();

		// Assert
		Destroyer.Sound_ArmorHurt.VolumeDb.ShouldBe(0);
        Destroyer.Sound_BlockHurt.VolumeDb.ShouldBe(0);
        Destroyer.Sound_Hurt.VolumeDb.ShouldBe(0);
        Destroyer.Sound_Hit.VolumeDb.ShouldBe(0);
    }

    [Test]
    public void Test_Death()
    {
        // Arrange
		Destroyer.Alive = true;

		// Act
		Destroyer.Death();

		// Assert
		Destroyer.Alive.ShouldBe(false);

    }

        [Test]
    public void Test_GetDamage()
    {
        // Arrange
        Destroyer.Health = 100;
        Destroyer.Armor = 0;
        Destroyer.State = Destroyer.Statement.Stun;

		// Act
		Destroyer.GetDamage(10);

		// Assert
		Destroyer.Health.ShouldBe(90);

    }

    [Test]
    public void Test_GetDamage2()
    {
        // Arrange
        Destroyer.Health = 9;
        Destroyer.Armor = 0;
        Destroyer.State = Destroyer.Statement.Stun;

		// Act
		Destroyer.GetDamage(10);

		// Assert
		Destroyer.Alive.ShouldBe(false);

    }

    [Test]
    public void Test_ArmorBreak()
    {
        // Arrange
        Destroyer.CanArmorBreak = true;
        Destroyer.Armor = 10;
        Destroyer.State = Destroyer.Statement.Stun;

		// Act
		Destroyer.ArmorBreak();

		// Assert
		Destroyer.CanArmorBreak.ShouldBe(false);
        Destroyer.IsReadyToRush.ShouldBe(false);
    }

    [Test]
    public void Test_ArmorBreak2()
    {
        // Arrange
        Destroyer.CanArmorBreak = true;
        Destroyer.Armor = 1;
        Destroyer.State = Destroyer.Statement.Stun;
        Destroyer.ChanceToZoom = 1;

		// Act
		Destroyer.ArmorBreak();

		// Assert
		Destroyer.ChanceToZoom.ShouldBe(0);
    }

    [Test]
    public void Test_SetArmorBreak()
    {
        // Arrange
        Destroyer.State = Destroyer.Statement.Stun;

		// Act
		Destroyer.SetArmorBreak();

		// Assert
        Destroyer.State.ShouldBe(Destroyer.Statement.ArmorBreak);

    }

    [Test]
    public void Test_OutHitArea()
    {
        // Arrange
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";    
        Destroyer.State = Destroyer.Statement.Idle;  
        Destroyer.Player = new player();
        Destroyer.Player.health = 100;  
        Destroyer.IsInHitArea = true;
        Destroyer.IsInBattleArea = true;

		// Act
		Destroyer.OutHitArea(area);

		// Assert
        Destroyer.IsInHitArea.ShouldBe(false);
		Destroyer.State.ShouldBe(Destroyer.Statement.Move);

    }

    [Test]
    public void Test_InHitArea()
    {
        // Arrange
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";    
        Destroyer.State = Destroyer.Statement.Move;  
        Destroyer.Player = new player();
        Destroyer.Player.health = 100;  
        Destroyer.IsInHitArea = false;

		// Act
		Destroyer.InHitArea(area);

		// Assert
        Destroyer.IsInHitArea.ShouldBe(true);
		Destroyer.State.ShouldBe(Destroyer.Statement.Idle);

    }

    [Test]
    public void Test_OutBattleArea()
    {
        // Arrange
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";    
        Destroyer.State = Destroyer.Statement.Move;  
        Destroyer.Player = new player();
        Destroyer.Player.health = 100;  
        Destroyer.IsInBattleArea = true;

		// Act
		Destroyer.OutBattleArea(area);

		// Assert
        Destroyer.IsInBattleArea.ShouldBe(false);
		Destroyer.State.ShouldBe(Destroyer.Statement.Idle);

    }

    [Test]
    public void Test_InBattleArea()
    {
        // Arrange
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";    
        Destroyer.State = Destroyer.Statement.Idle;  
        Destroyer.Player = new player();
        Destroyer.Player.health = 100;  
        Destroyer.IsInBattleArea = false;

		// Act
		Destroyer.InBattleArea(area);

		// Assert
        Destroyer.IsInBattleArea.ShouldBe(true);
		Destroyer.State.ShouldBe(Destroyer.Statement.Move);

    }

    [Test]
    public void Test_Stun()
    {
        // Arrange
        Destroyer.DeltaStunTime = 0;

		// Act
		Destroyer.Stun(0.1);

		// Assert
        Destroyer.AnimSprite.SelfModulate.ShouldBe(new Color(1, 1, 1));

    }

    [Test]
    public void Test_BezierCurve()
    {
        // Arrange
        Destroyer.DeltaStunTime = 0;

		// Act
		Vector2 r = Destroyer.BezierCurve(1f);

		// Assert
        r.ShouldBe(new Vector2(0,0));

    }

    [Test]
    public void Test_FinishRush()
    {
        // Arrange
        Destroyer.Armor = 0;
        Destroyer.IsReadyToRush = true;
        Destroyer.IsCollided = true;
        Destroyer.State = Destroyer.Statement.Idle; 

		// Act
		Destroyer.FinishRush(true);

		// Assert
        Destroyer.IsCollided.ShouldBe(false);
        Destroyer.IsReadyToRush.ShouldBe(false);
        Destroyer.AnimSprite.SelfModulate.ShouldBe(new Color(1, 1, 1));
        Destroyer.State.ShouldBe(Destroyer.Statement.Stun);

    }

    [Test]
    public void Test_FinishRush2()
    {
        // Arrange
        Destroyer.Armor = 0;
        Destroyer.IsInHitArea = true;
        Destroyer.IsInBattleArea = false;
        Destroyer.State = Destroyer.Statement.Move; 

		// Act
		Destroyer.FinishRush(false);

		// Assert
        Destroyer.State.ShouldBe(Destroyer.Statement.Idle);

    }

    [Test]
    public void Test_FinishRush3()
    {
        // Arrange
        Destroyer.Armor = 1;
        Destroyer.IsInHitArea = false;
        Destroyer.IsInBattleArea = true;
        Destroyer.State = Destroyer.Statement.Idle; 

		// Act
		Destroyer.FinishRush(false);

		// Assert
        Destroyer.State.ShouldBe(Destroyer.Statement.Move);

    }

    [Test]
    public void Test_Collided()
    {
        // Arrange
        var area = Substitute.For<Node2D>();
        area.Name = "HurtBox";    
        Destroyer.State = Destroyer.Statement.Rush;  
        Destroyer.Player = new player();
        Destroyer.Player.health = 100;  
        Destroyer.IsCollided = false;

		// Act
		Destroyer.Collided(area);

		// Assert
        Destroyer.IsCollided.ShouldBe(true);

    }

    [Test]
    public void Test_Shoot()
    {
        // Arrange
        Destroyer.Armor = 1;
        Destroyer.IsInHitArea = false;
        Destroyer.IsInBattleArea = true;
        Destroyer.DeltaShootCooldown = 0;
        Destroyer.ShootCooldown = 10;
        Destroyer.State = Destroyer.Statement.Idle; 

		// Act
		Destroyer.Shoot();

		// Assert
        Destroyer.DeltaShootCooldown.ShouldBe(10);

    }

    [Test]
    public void Test_SBasicAttack()
    {
        // Arrange
        Destroyer.Armor = 1;
        Destroyer.IsInHitArea = false;
        Destroyer.IsInBattleArea = true;
        Destroyer.DeltaBasicAttackCooldown = 0;
        Destroyer.BasicAttackCooldown = 10;
        Destroyer.State = Destroyer.Statement.Idle; 

		// Act
		Destroyer.BasicAttack();

		// Assert
        Destroyer.DeltaBasicAttackCooldown.ShouldBe(10);

    }
    [Test]
    public void Test_TurnAroundElements()
    {
        // Arrange
        var obj = Substitute.For<Node2D>();
        obj.Position = new Vector2(10,10);

		// Act
		Destroyer.TurnAroundElements(obj);

		// Assert
        obj.Position.ShouldBe(new Vector2(-10,10));

    }

}