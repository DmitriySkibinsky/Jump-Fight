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
public class JumpPlatformTests: TestClass
{
    public JumpPlatformTests(Node testScene) : base(testScene) { }

    [Test]
    public void Test_on_area_2d_body_entered()
    {
        // Arrange
		player body = new player();
		body.velocity.Y = 100;
		JumpPlatform pl = new JumpPlatform();
		// Act
		pl._on_area_2d_body_entered(body);

		// Assert
		body.Velocity.Y.ShouldBe(0f);
    }


}