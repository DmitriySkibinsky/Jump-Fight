using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;
using NSubstitute;

public partial class DeathTests : TestClass
{
    private Death death;

    public DeathTests(Node testScene) : base(testScene) { }

    [Setup]
    public void SetUp()
    {
        death = new Death();
    }

    [Test]
    public void Test_Enter()
    {
		// Act
		death.Enter();

		// Assert
		death.Owner.ShouldBeNull();
        
    }

}