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
public class InLevelUITests: TestClass
{
    public InLevelUITests(Node testScene) : base(testScene) { }
    private in_level_ui _inLevelUI;
    public Func<SceneTree> GetTree { get; set; }

    public PathFollow2D PathLeft;

    [Setup]
    public void SetUp()
    {
        _inLevelUI = new in_level_ui();
        _inLevelUI._Ready();
    }

   [Test]
    public void Test_AnimateOut()
    {
        // Act
        _inLevelUI.animate_out();

        // Assert
        _inLevelUI.is_animate_out.ShouldBeTrue();
    }

    [Test]
    public void Test_AnimateIn()
    {
        // Act
        _inLevelUI.animate_in();

        // Assert
        _inLevelUI.is_animate_in.ShouldBeTrue();
    }

    [Test]
    public void Test_Process()
    {
        // Arrange
        _inLevelUI.is_animate_in = false;
        _inLevelUI.PathLeft = Substitute.For<PathFollow2D>();
        _inLevelUI.PathRight = Substitute.For<PathFollow2D>();
        var initialProgressRatio = _inLevelUI.PathLeft.ProgressRatio;
        _inLevelUI.PathLeft.ProgressRatio = 0.5f;
        

        // Act
        _inLevelUI._Process(0.1);

        // Assert
        _inLevelUI.is_animate_out.ShouldBeFalse();
    }

    [Test]
    public void Test_Process_2()
    {
        // Arrange
        _inLevelUI.is_animate_in = true;
        _inLevelUI.PathLeft = Substitute.For<PathFollow2D>();
        _inLevelUI.PathRight = Substitute.For<PathFollow2D>();
        var initialProgressRatio = _inLevelUI.PathLeft.ProgressRatio;
        _inLevelUI.PathLeft.ProgressRatio = 0.49f;

        // Act
        _inLevelUI._Process(0.1);

        // Assert
        _inLevelUI.is_animate_in.ShouldBeFalse();
    }

    [Test]
    public void OnSettingsPressed_WhenCalled_ShouldChangeSceneToSettingsPause()
    {
        // Arrange
        SceneTree tree = GetTree?.Invoke();

        if (tree != null)
        {
            // Получаем текущую сцену
            Node currentScene = tree.CurrentScene;
        
            // Act
            _inLevelUI._on_settings_pressed();

            // Обновляем текущую сцену после совершения действия
            currentScene = tree.CurrentScene;

            // Assert
            currentScene.GetPath().ToString().ShouldBe("res://scenes/Menu/settingspause.tscn");
        }
    }


}
