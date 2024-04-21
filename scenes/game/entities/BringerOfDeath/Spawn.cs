using Godot;
using System;

public partial class Spawn : state
{
	public bool IsAnimFinished = false;
    public async override void Enter()
    {
        base.Enter();
		AnimationPlayer.Play("appear");
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
