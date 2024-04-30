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

public class Laser2Tests : TestClass
{

    public Laser2Tests(Node testScene) : base(testScene) { }

    private Laser2 Laser;

    [Setup]
    public void Setup()
    {
        Laser = new Laser2();
    }

    [Test]
    public void Test_Enter()
    {
        //Act
        Laser.Enter();

        //Assert
        Laser.IsAnimFinished.ShouldBe(false);
    }

}
