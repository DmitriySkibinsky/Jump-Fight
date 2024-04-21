using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;

public partial class menuTests : TestClass
{
    private menu _menu;

    public menuTests(Node testScene) : base(testScene) { }

    public void SetUp()
    {
        _menu = new menu();
    }

     public Func<SceneTree> GetTree { get; set; }

    [Test]
    public void PlayButtonPressed_ShouldChangeSceneToLevel1()
    {
        // Arrange
        SceneTree tree = GetTree?.Invoke();

    if (tree != null)
    {
        // Получаем текущую сцену
        Node currentScene = tree.CurrentScene;
    
        // Act
        _menu._on_play_pressed();

        // Обновляем текущую сцену после совершения действия
        currentScene = tree.CurrentScene;

        // Assert
        currentScene.GetPath().ToString().ShouldBe("res://scenes/game/Level/Level1/level1.tscn");
    }
    }

    [Test]
    public void QuitButtonPressed_ShouldQuit()
    {
         // Arrange
        SceneTree tree = GetTree?.Invoke();

    if (tree != null)
    {
        // Получаем текущую сцену
        Node currentScene = tree.CurrentScene;
    
        // Act
        _menu._on_quit_pressed();

        // Обновляем текущую сцену после совершения действия
        currentScene = tree.CurrentScene;

        // Assert
        currentScene.GetPath().ToString().ShouldBe("");
    }
    }

    [Test]
    public void SettingsButtonPressed_ShouldChangeSceneToSettings()
    {
         // Arrange
        SceneTree tree = GetTree?.Invoke();

    if (tree != null)
    {
        // Получаем текущую сцену
        Node currentScene = tree.CurrentScene;
    
        // Act
        _menu._on_settings_pressed();

        // Обновляем текущую сцену после совершения действия
        currentScene = tree.CurrentScene;

        // Assert
        currentScene.GetPath().ToString().ShouldBe("res://scenes/Menu/settings.tscn");
    }
    }
}
