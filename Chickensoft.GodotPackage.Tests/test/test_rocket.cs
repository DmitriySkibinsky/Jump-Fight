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

public class RocketTests : TestClass
{

    public RocketTests(Node testScene) : base(testScene) { }

    private Rocket Rocket;

    [Setup]
    public void Setup()
    {
        Rocket = new Rocket();
    }

    [Test]
    public void Test_Enter()
    {
        //Act
        Rocket.Enter();

        //Assert
       Rocket.IsAnimFinished.ShouldBe(false);
    }

}
