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

public class AttackGolemTest : TestClass {

	public AttackGolemTest(Node testScene) : base(testScene) { }

    private AttackGolem attack;

    [Setup]
    public void Setup()
    {
        attack = new AttackGolem();
    }

    [Test]
    public void Test_Enter()
    {
        // Act
        attack.Enter();

        // Assert
        attack.IsAttackFinished.ShouldBe(false);
    }
}