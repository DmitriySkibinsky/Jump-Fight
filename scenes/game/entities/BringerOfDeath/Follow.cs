using Godot;
using System;

public partial class Follow : state
{
    public RayCast2D rayCast2D;
    public override void _Ready()
    {
        base._Ready();
        rayCast2D = GetNode<RayCast2D>("../../RayCast2D");
    }
    public override void Enter(){
        base.Enter();
        Owner.SetPhysicsProcess(true);
        AnimationPlayer.Play("walk");
    }
    public override void Exit(){
        base.Exit();
        Owner.SetPhysicsProcess(false);
    }

    public override void Transition()
    {
        float distance = (float)Owner.Get("Direction");

        if (!rayCast2D.IsColliding())
        {
            FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
            Parent.ChangeState ("Idle");
        }
    
        if (Math.Abs(distance) < 250){
            FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
            Parent.ChangeState ("Attack");
        }
    }

}
