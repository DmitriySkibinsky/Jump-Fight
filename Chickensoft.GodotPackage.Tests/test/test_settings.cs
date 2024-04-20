using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;

public partial class settingsTests : TestClass
{
    private settings _settings;

    public settingsTests(Node testScene) : base(testScene) { }

    public void SetUp()
    {
        _settings = new settings();
    }

     public Func<SceneTree> GetTree { get; set; }

    [Test]
    public void PlayButtonPressed_ShouldChangeSceneToMenu()
    {
        // Arrange
        SceneTree tree = GetTree?.Invoke();

    if (tree != null)
    {
        // Получаем текущую сцену
        Node currentScene = tree.CurrentScene;
    
        // Act
        _settings._on_exit_pressed();

        // Обновляем текущую сцену после совершения действия
        currentScene = tree.CurrentScene;

        // Assert
        currentScene.GetPath().ToString().ShouldBe("res://scenes/Menu/menu.tscn");
    }
    }

    [Test]
    public void Settings_QuitButtonPressed_ShouldQuit()
    {
         // Arrange
        SceneTree tree = GetTree?.Invoke();

    if (tree != null)
    {
        // Получаем текущую сцену
        Node currentScene = tree.CurrentScene;
    
        // Act
        _settings._on_quit_pressed();

        // Обновляем текущую сцену после совершения действия
        currentScene = tree.CurrentScene;

        // Assert
        currentScene.GetPath().ToString().ShouldBe("");
    }
    }

}