using Godot;
using System;

public partial class SpawnGolem : state
{
	public bool IsAnimFinished = false;
    public async override void Enter()
    {
        base.Enter();
		AnimationPlayer.Play("Spawn");
		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		IsAnimFinished = true;
    }

    public override void Transition()
    {
        if (IsAnimFinished){
			FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
            Parent.ChangeState ("Idle");
		}
    }
}
