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

public class InteractionManagerTest : TestClass {

	public InteractionManagerTest(Node testScene) : base(testScene) { }

    public InteractionManager IM;
    public player Player;

    [Setup]
    public void SetUp()
    {
        IM = new InteractionManager();
        Player = new player();
        IM.Player = Player;
    }

[Test]
    public void Test_RegisterPlatform()
    {
        // Arrange
        var area = new InteractionArea();

        // Act
        IM.RegisterArea(area);

        // Assert
        IM.ActiveAreas.ShouldNotBeEmpty();
    }

    [Test]
    public void Test_UnregisterPlatform()
    {
        // Arrange
        var area = new InteractionArea();

        // Act
        IM.RegisterArea(area);
        IM.ActiveAreas.ShouldNotBeEmpty();
        IM.UnregisterArea(area);

        // Assert
        IM.ActiveAreas.ShouldBeEmpty();
    }

    [Test]
    public void Test_SortByPlayerDistance()
    {
        // Arrange
        IM.Player = new player();
        var obj1 = new InteractionArea();
        var obj2 = new InteractionArea();
        obj1.GlobalPosition = new Vector2(0,100);
        obj2.GlobalPosition = new Vector2(0,100);

        // Act
        var res = IM.SortByPlayerDistance(obj1,obj2);
 
        // Assert
        res.ShouldBe(0);
    }

    [Test]
    public void Test_SortByPlayerDistance_2()
    {
        // Arrange
        var area1 = new InteractionArea();
        var area2 = new InteractionArea();
        area1.GlobalPosition = new Vector2(0,100);
        area2.GlobalPosition = new Vector2(0,110);

        // Act
        var res = IM.SortByPlayerDistance(area1,area2);
 
        // Assert
        res.ShouldBe(1);
    }

    [Test]
    public void Test_SortByPlayerDistance_3()
    {
        // Arrange
        IM.Player = new player();
        var obj1 = new InteractionArea();
        var obj2 = new InteractionArea();
        obj1.GlobalPosition = new Vector2(0,110);
        obj2.GlobalPosition = new Vector2(0,100);

        // Act
        var res = IM.SortByPlayerDistance(obj1,obj2);
 
        // Assert
        res.ShouldBe(-1);
    }

    [Test]
    public void Test_Process()
    {
        // Arrange
        var fakeLabel = new Label();
        IM.label = fakeLabel; 
        IM.BaseText = "Interact with: ";

        var area1 = new InteractionArea();
        area1.ActionName = "Door";
        area1.GlobalPosition = new Vector2(10, 10);
        IM.RegisterArea(area1);

        // Act
        IM._Process(0.1);

        // Assert
        fakeLabel.Text.ShouldBe("Interact with: Door");
        fakeLabel.GlobalPosition.ShouldBe(new Vector2(10, 10));
        fakeLabel.Visible.ShouldBeTrue();
    }

    [Test]
    public void Test_Process_2()
    {
        // Arrange
        var fakeLabel = new Label(); 
        IM.label = fakeLabel; 
        IM.CanInteract = false;

        // Act
        IM._Process(0.1);

        // Assert
        fakeLabel.Visible.ShouldBeFalse();
    }

}
