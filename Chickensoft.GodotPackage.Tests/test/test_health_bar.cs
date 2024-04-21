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
public class HealthBarTests: TestClass
{
    public HealthBarTests(Node testScene) : base(testScene) { }

    public health_bar health_bar;
    public Func<SceneTree> GetTree { get; set; }

    [Setup]
    public void SetUp()
    {
        health_bar = new health_bar();
    }

    [Test]
    public void Test__on_player_health_changed()
    {
        health_bar.Health_bar = Substitute.For<TextureProgressBar>();

        health_bar._on_player_health_changed(10);

        health_bar.Health_bar.Value.ShouldBe(10f);        
    }
}