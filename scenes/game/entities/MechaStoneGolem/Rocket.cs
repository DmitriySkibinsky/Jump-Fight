using Chickensoft.GodotTestDriver.Input;
using Godot;
using System;
using System.Threading.Tasks;

public partial class Rocket : state
{
	public PackedScene RocketScene = GD.Load<PackedScene>("res://scenes/game/entities/MechaStoneGolem/HomingMissile.tscn");
	public Marker2D RocketSpawn;
	public bool IsAnimFinished;

    public override void _Ready()
    {
        base._Ready();
		RocketSpawn = GetNode<Marker2D>("RocketSpawn");
		IsAnimFinished = false;
    }
	public async override void Enter()
    {
        base.Enter();
		IsAnimFinished = false;
		if ((int)Owner.Get("HealthPoints") > 400){
			await LaunchRocket();
			await LaunchRocket();
		}else{
			await LaunchRocket();
			await LaunchRocket();
			await LaunchRocket();
		}
		
	
		IsAnimFinished = true;
    }

	public async Task LaunchRocket(){
		AnimationPlayer.Play("ranged_attack");
		await ToSignal(GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);

		HomingMissile rocket= RocketScene.Instantiate<HomingMissile>();
		Transform2D transform2D= new Transform2D(RocketSpawn.Rotation, new Vector2(2.5f,2.5f), rocket.Skew, RocketSpawn.GlobalPosition);
		rocket.Start(transform2D, Player);
		Owner.GetParent().GetParent().AddChild(rocket);
		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}
    public override void _Process(double delta)
    {
        base._Process(delta);
		RocketSpawn.LookAt(Player.GlobalPosition);
    }
    public override void Transition()
    {
		if (IsAnimFinished){
			FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
            Parent.ChangeState ("Idle");
		}
    }

}
