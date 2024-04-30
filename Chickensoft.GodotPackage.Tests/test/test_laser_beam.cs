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

public class LaserBeamTests : TestClass
{

    public LaserBeamTests(Node testScene) : base(testScene) { }

    private LaserBeam Laser;

    [Setup]
    public void Setup()
    {
        Laser = new LaserBeam();
    }

    [Test]
    public void Test_on_hit_box_body_entered()
    {
        //Arrange
        var body = new Node2D();
        body.Name = "Player";
        Laser.Player = new player();
        Laser.Player.health = 100;
        body = Laser.Player;
        Laser.Player.smack = new AudioStreamPlayer();
        Laser.Player.smack.Stream = ResourceLoader.Load<AudioStream>("Sounds/Smack");
        Laser.Damage = 10;
        
        //Act
        Laser._on_hit_box_body_entered(body);

        //Assert
        Laser.Player.health.ShouldBe(90);
    }

}
