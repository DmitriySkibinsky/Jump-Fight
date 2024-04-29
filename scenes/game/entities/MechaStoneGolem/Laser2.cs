using Godot;
using System;
using System.Threading.Tasks;

public partial class Laser2 : state
{
	public PackedScene laserBeam = GD.Load<PackedScene>("res://scenes/game/entities/MechaStoneGolem/LaserBeam.tscn");
	public bool IsAnimFinished;

    public override void _Ready()
    {
        base._Ready();
		IsAnimFinished = false;
		SetProcess(true);
    }
    public async override void Enter()
    {
        base.Enter();
		SetProcess(true);
		IsAnimFinished = false;
		AnimationPlayer.Play("laser_cast");

		LaserBeam laser1 = laserBeam.Instantiate<LaserBeam>();
		LaserBeam laser2 = laserBeam.Instantiate<LaserBeam>();

		laser1.GlobalPosition = new Vector2(110 ,GlobalPosition.Y+90);
		laser1.Scale = new Vector2(2,2);

		laser2.GlobalPosition = new Vector2(1790 ,GlobalPosition.Y+83);
		laser2.Scale = new Vector2(2,2);
		laser2.RotationDegrees = -180;

		laser1.Damage = 15;
		laser2.Damage = 15;

		Owner.GetParent().GetParent().AddChild(laser1);
		Owner.GetParent().GetParent().AddChild(laser2);

		laser1.Prepare();
		laser2.Prepare();

		await ToSignal(GetTree().CreateTimer(1.5), SceneTreeTimer.SignalName.Timeout);

		laser1.Attack();
		laser2.Attack();

		await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
		laser1.QueueFree();
		laser2.QueueFree();

		IsAnimFinished = true;
    }

	
	
	public override void Exit(){
		base.Exit();
	}
    
    public override void Transition()
    {
		if (IsAnimFinished){
			FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
            Parent.ChangeState ("Idle");
		}
    }
}
