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
public class CanvasLayerTests: TestClass
{
    public CanvasLayerTests(Node testScene) : base(testScene) { }

    public canvas_layer canvas_layer;
    public Func<SceneTree> GetTree { get; set; }

    [Setup]
    public void SetUp()
    {
        canvas_layer = new canvas_layer();
    }

    [Test]
    public void Test_on_player_super_reload()
    {

        canvas_layer.Super_bar = Substitute.For<TextureProgressBar>();
        canvas_layer.Super_bar.Value = 90;

        canvas_layer._on_player_super_reload(100);

        canvas_layer.Super_bar.MaxValue.ShouldBe(100);
    }

    [Test]
    public void Test_on_reload_timeout()
    {

        canvas_layer.Super_bar = Substitute.For<TextureProgressBar>();
        canvas_layer.Super_bar.Value = 99;

        canvas_layer._on_reload_timeout();

        canvas_layer.Super_bar.MaxValue.ShouldBe(100);
    }
}