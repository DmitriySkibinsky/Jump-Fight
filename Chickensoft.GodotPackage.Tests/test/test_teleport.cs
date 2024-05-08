using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;
using NSubstitute;

public partial class TeleportTests : TestClass
{
    private Teleport Teleport;

    public TeleportTests(Node testScene) : base(testScene) { }

    [Setup]
    public void SetUp()
    {
        Teleport = new Teleport();
    }

    [Test]
    public void Test_Enter()
    {
        //Arrange
        Teleport.IsTeleportFinished = true;
        Teleport.IsTeleportToPlayer = true;

        //Act
        Teleport.Enter();

        //Assert
        Teleport.IsTeleportFinished.ShouldBe(false);
        Teleport.IsTeleportToPlayer.ShouldBe(false);
    }


}