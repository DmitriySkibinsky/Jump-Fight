using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;
using NSubstitute;

public partial class settingsTests : TestClass
{
    private settings _settings;

    public settingsTests(Node testScene) : base(testScene) { }

    [Setup]
    public void SetUp()
    {
        _settings = new settings();
        _settings.mute = Substitute.For<AudioStreamPlayer>();
    }

     public Func<SceneTree> GetTree { get; set; }

    [Test]
    public void ExitButtonPressed_ShouldChangeSceneToMenu()
    {
        // Arrange
        SceneTree tree = GetTree?.Invoke();
        settings.Sound = false;

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

    [Test]
    public void Settings_AudioButtonPressed1()
    {
         // Arrange
        settings.Audio = true;
        settings.button = Substitute.For<Button>();
        settings.button_s = Substitute.For<Button>();
        settings.button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
        
        // Act
        _settings._on_audio_pressed();

        // Assert
        settings.Audio.ShouldBe(false);
    }

    [Test]
    public void Settings_AudioButtonPressed2()
    {
         // Arrange
        settings.Audio = false;
        settings.button = Substitute.For<Button>();
        settings.button_s = Substitute.For<Button>();
        settings.button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
        
        // Act
        _settings._on_audio_pressed();

        // Assert
        settings.Audio.ShouldBe(true);
    }
    [Test]
    public void Settings_SoundsButtonPressed1()
    {
         // Arrange
        settings.Sound = true;
        settings.button = Substitute.For<Button>();
        settings.button_s = Substitute.For<Button>();
        settings.button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
        
        // Act
        _settings._on_sounds_pressed();

        // Assert
        settings.Sound.ShouldBe(false);
    }

        public void Settings_SoundsButtonPressed2()
    {
         // Arrange
        settings.Sound = false;
        settings.button = Substitute.For<Button>();
        settings.button_s = Substitute.For<Button>();
        settings.button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
        
        // Act
        _settings._on_sounds_pressed();

        // Assert
        settings.Sound.ShouldBe(true);
    }

}
