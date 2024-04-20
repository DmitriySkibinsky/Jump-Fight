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
        Scav.Anim = new AnimatedSprite2D();

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
        Scav.Anim = new AnimatedSprite2D();

        // Act
        _scav.GetDamage(damage);

        // Assert
        Scav.Alive.ShouldBeFalse();
    }
        

}
