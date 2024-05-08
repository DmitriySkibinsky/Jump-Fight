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

public class MechaStoneGolemTests : TestClass
{

    public MechaStoneGolemTests(Node testScene) : base(testScene) { }

    private MechaStoneGolem MechaStoneGolem;

    [Setup]
    public void Setup()
    {
        MechaStoneGolem = new MechaStoneGolem();
    }

    [Test]
    public void Test_PhysicsProcess()
    {
        //Arrange
        MechaStoneGolem.velocity = new Vector2(0,0);
        
        //Act
        MechaStoneGolem._PhysicsProcess(0.1);

        //Assert
        MechaStoneGolem.velocity.Y.ShouldBe(98);
        MechaStoneGolem.velocity.X.ShouldBe(-20);
    }

    [Test]
    public void Test_PhysicsProcess1()
    {
        //Arrange
        MechaStoneGolem.velocity = new Vector2(0,0);
        MechaStoneGolem.Direction = 10;
        
        //Act
        MechaStoneGolem._PhysicsProcess(0.1);

        //Assert
        MechaStoneGolem.velocity.Y.ShouldBe(98);
        MechaStoneGolem.velocity.X.ShouldBe(20);
    }

}
