using Godot;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Chickensoft.GoDotLog;
using System;
using System.Threading.Tasks;
using Chickensoft.GodotTestDriver.Drivers;
using Shouldly;
using NSubstitute;

public partial class ManagerTests : TestClass
{
    private Manager Manager;

    public ManagerTests(Node testScene) : base(testScene) { }

    [Setup]
    public void SetUp()
    {
        Manager = new Manager();
        Manager.mute = Substitute.For<AudioStreamPlayer>();
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
        Manager._on_exit_pressed();

        // Обновляем текущую сцену после совершения действия
        currentScene = tree.CurrentScene;

        // Assert
        currentScene.GetPath().ToString().ShouldBe("res://scenes/Menu/menu.tscn");
    }
    }

        [Test]
    public void NewGameButtonPressed_ShouldChangeSceneToLevel1()
    {
        // Arrange
        SceneTree tree = GetTree?.Invoke();

    if (tree != null)
    {
        // Получаем текущую сцену
        Node currentScene = tree.CurrentScene;
    
        // Act
        Manager._on_new_game_pressed();

        // Обновляем текущую сцену после совершения действия
        currentScene = tree.CurrentScene;

        // Assert
        currentScene.GetPath().ToString().ShouldBe("res://scenes/game/Level/Level1/level1.tscn");
    }
    }

        [Test]
    public void Test_ResumeButton()
    {
        // Arrange
        Manager.game_paused = true;
    
        // Act
        Manager._on_resum_pressed();

        // Assert
        Manager.game_paused.ShouldBe(false);
    }


    [Test]
    public void Test_AudioButtonPressed1()
    {
         // Arrange
        settings.Audio = true;
        Manager.button = Substitute.For<Button>();
        Manager.button_s = Substitute.For<Button>();
        Manager.button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
        
        // Act
        Manager._on_audio_pressed();

        // Assert
        settings.Audio.ShouldBe(false);
    }

    [Test]
    public void Test_AudioButtonPressed2()
    {
         // Arrange
        settings.Audio = false;
        Manager.button = Substitute.For<Button>();
        Manager.button_s = Substitute.For<Button>();
        Manager.button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
        
        // Act
        Manager._on_audio_pressed();

        // Assert
        settings.Audio.ShouldBe(true);
    }
    [Test]
    public void Test_SoundsButtonPressed1()
    {
         // Arrange
        settings.Sound = true;
        Manager.button = Substitute.For<Button>();
        Manager.button_s = Substitute.For<Button>();
        Manager.button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
        
        // Act
        Manager._on_sounds_pressed();

        // Assert
        settings.Sound.ShouldBe(false);
    }

        public void Test_SoundsButtonPressed2()
    {
         // Arrange
        settings.Sound = false;
        Manager.button = Substitute.For<Button>();
        Manager.button_s = Substitute.For<Button>();
        Manager.button.Icon = (Texture2D)ResourceLoader.Load("res://assets/sprites/Audio Square Button Off.png");
        
        // Act
        Manager._on_sounds_pressed();

        // Assert
        settings.Sound.ShouldBe(true);
    }

}
