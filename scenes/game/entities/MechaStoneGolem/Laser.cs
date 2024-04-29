using Godot;
using System;
using System.Threading.Tasks;

public partial class Laser : state
{
	public Node2D LaserContainer;
	
	public bool IsAnimFinished;

    public override void _Ready()
    {
        base._Ready();
		IsAnimFinished = false;
		LaserContainer = GetNode<Node2D>("LaserContainer");
		SetProcess(true);
    }
    public async override void Enter()
    {
        base.Enter();
		SetProcess(true);
		IsAnimFinished = false;
		AnimationPlayer.Play("laser_cast");
		await PrepareAttack();
		await ToSignal(GetTree().CreateTimer(2), SceneTreeTimer.SignalName.Timeout);
		SetProcess(false);
		await ToSignal(GetTree().CreateTimer(0.7), SceneTreeTimer.SignalName.Timeout);
		await Attack();

		if ((int)Owner.Get("HealthPoints") < 400){
				AnimationPlayer.Play("reboot");
				await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		}
		
		IsAnimFinished = true;
    }

	public async Task PrepareAttack()
{
    if ((int)Owner.Get("HealthPoints") < 400){
        foreach (LaserBeam laser in LaserContainer.GetChildren())
        {
            laser.Prepare();
        }
    }
    else{
        LaserContainer.GetChildren()[0].CallDeferred("Prepare");
    }
}

	public async Task Attack(){
		if  ((int)Owner.Get("HealthPoints") < 400){
			foreach (LaserBeam laser in LaserContainer.GetChildren()){
				laser.Attack();
			}
		}else{
			LaserContainer.GetChildren()[0].CallDeferred("Attack");
		}
	}

	public override void Exit(){
		base.Exit();
	}
    public override void _Process(double delta)
    {
		foreach (LaserBeam laser in LaserContainer.GetChildren()){
				laser.LookAt(Player.Position);
		}
    }

    public override void Transition()
    {
		if (IsAnimFinished){
			FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
            Parent.ChangeState ("Idle");
		}
    }
}
