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

public class AttackTest : TestClass {

	public AttackTest(Node testScene) : base(testScene) { }

    private Attack attack;

    [Setup]
    public void Setup()
    {
        attack = new Attack();
    }

    [Test]
    public void Test_Enter()
    {
        // Arrange
        var animationPlayer = new AnimationPlayer();
        var timer1 = new Timer();
        var timer2 = new Timer();
        var tree = new SceneTree();
        var owner = new Node();
        var parent = new FiniteStateMachine();
        attack.AnimationPlayer = animationPlayer;
        attack.HitBox = new Area2D();
        attack.Owner = owner;


        // Act
        attack.Enter();

        // Assert
        attack.IsAttackFinished.ShouldBe(false);
    }
}