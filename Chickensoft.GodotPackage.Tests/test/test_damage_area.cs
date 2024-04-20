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

public class Damage_areaTest : TestClass {


	public Damage_areaTest(Node testScene) : base(testScene) { }
   [Test]
	public void OnDetectorBodyEntered_EnemyBody_Damages()
	{
		var enemy = new Node2D();
		enemy.Name = "Enemy";
		var d_area = new damage_area(); 
		bool damaged = false;
		d_area.damaged = damaged;

		// Act
		d_area.OnDetectorBodyEntered(enemy);

		// Assert
		d_area.damage.ShouldBe(100);

	}
}