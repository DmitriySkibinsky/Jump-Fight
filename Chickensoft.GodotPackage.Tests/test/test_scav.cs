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

public class ScavTests : TestClass
{
    public ScavTests(Node testScene) : base(testScene) { }
    private Scav _scav;

    [Setup]
    public void SetUp()
    {
        _scav = new Scav();
    }

    [Test]
    public void death_Test()
    {
        // Arrange
        Scav.Alive = true;
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;

        // Act
        _scav.Death();

        // Assert
        Scav.Alive.ShouldBeFalse();
    }

    [Test]
    public void GetDamage_Test()
    {
        //Arrange
        Scav.Health=100;
        int damage = 20;
        Scav.Anim = new AnimatedSprite2D();

        // Act
        _scav.GetDamage(damage);

        // Assert
        Scav.Health.ShouldBe(80); // Проверяем уменьшение здоровья
    }

    [Test]
    public void GetDamage_Test2()
    {
        //Arrange
        Scav.Alive = true;
        Scav.Health=10;
        int damage = 20;
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;

        // Act
        _scav.GetDamage(damage);

        // Assert
        Scav.Alive.ShouldBeFalse();
    }

    [Test]
    public void Attack_Test()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        var player = Substitute.For<player>();
        var body = Substitute.For<Node2D>();
        body.Name = "HurtBox";
        Scav.Player = new player();
        Scav.Player.health=50;
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Run;
        Scav.Alive = true;


        // Act
        _scav.Attack(body);

        // Assert
        Scav.IdleTime.ShouldBe(2);
    }

    [Test]
    public void _Process_Test()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = true;
        Scav.IdleTime = 0; 
        var delta = 0.5;
        Scav.DamagedTime=(float)0.5;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.DamagedTime.ShouldBe((float)0.0);
        Scav.State.ShouldBe(Scav.Statement.Run);
    }
    [Test]
    public void Process_Test2()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = true;
        Scav.IdleTime = 1; 
        var delta = 0.5;
        Scav.DamagedTime=(float)0.5;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.DamagedTime.ShouldBe((float)0.0);
        Scav.State.ShouldBe(Scav.Statement.Idle);
    }

    [Test]
    public void Process_Test3()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = true;
        Scav.IdleTime = 0; 
        var delta = 0.5;
        Scav.DamagedTime=(float)1;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.DamagedTime.ShouldBe((float)0.5);
        Scav.State.ShouldBe(Scav.Statement.Idle);
    }

    [Test]
    public void Process_Test4()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Attack;
        Scav.Alive = true;
        Scav.IdleTime = 1; 
        var delta = 0.5;
        Scav.DamagedTime=(float)1;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.DamagedTime.ShouldBe((float)0.5);
        Scav.State.ShouldBe(Scav.Statement.Attack);
    }

    [Test]
    public void Process_Test5()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = false;
        Scav.IdleTime = 0; 
        var delta = 0.5;
        Scav.DamagedTime=(float)0.5;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.DamagedTime.ShouldBe((float)0.5);
        Scav.State.ShouldBe(Scav.Statement.Idle);
    }

    [Test]
     public void Process_Test6()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = true;
        Scav.IdleTime = 0; 
        var delta = 0.5;
        Scav.DamagedTime=(float)-0.5;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.DamagedTime.ShouldBe((float)-0.5);
        Scav.State.ShouldBe(Scav.Statement.Idle);
    }

    [Test]
    public void Process_Test7()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = true;
        Scav.IdleTime = (float)0.5; 
        var delta = 0.5;
        Scav.DamagedTime=(float)0;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.IdleTime.ShouldBe((float)0.0);
        Scav.State.ShouldBe(Scav.Statement.Run);
    }

    [Test]
    public void Process_Test8()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = true;
        Scav.IdleTime = (float)0.5; 
        var delta = 0.5;
        Scav.DamagedTime=(float)1;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.IdleTime.ShouldBe((float)0.0);
        Scav.State.ShouldBe(Scav.Statement.Idle);
    }

    [Test]
    public void Process_Test9()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = true;
        Scav.IdleTime = 1; 
        var delta = 0.5;
        Scav.DamagedTime=(float)0;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.IdleTime.ShouldBe((float)0.5);
        Scav.State.ShouldBe(Scav.Statement.Idle);
    }

    [Test]
    public void Process_Test10()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Attack;
        Scav.Alive = true;
        Scav.IdleTime = (float)1; 
        var delta = 0.5;
        Scav.DamagedTime=(float)1;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.IdleTime.ShouldBe((float)0.5);
        Scav.State.ShouldBe(Scav.Statement.Attack);
    }

    [Test]
    public void Process_Tes11()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = false;
        Scav.IdleTime = (float)0.5; 
        var delta = 0.5;
        Scav.DamagedTime=(float)0;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.IdleTime.ShouldBe((float)0.5);
        Scav.State.ShouldBe(Scav.Statement.Idle);
    }

    [Test]
     public void Process_Test12()
    {
        // Arrange
        var anim = Substitute.For<AnimatedSprite2D>();
        Scav.Anim = anim;
        Scav.State = Scav.Statement.Idle;
        Scav.Alive = true;
        Scav.IdleTime = (float)-0.5; 
        var delta = 0.5;
        Scav.DamagedTime=(float)0;
        var hitBoxes = Substitute.For<Area2D>();
        Scav.HitBoxes = hitBoxes;


        // Act
        _scav._Process(delta);

        // Assert
        Scav.IdleTime.ShouldBe((float)-0.5);
        Scav.State.ShouldBe(Scav.Statement.Idle);
    }

}