using Godot;
using System;

public partial class Attack : state
{
 
    public Area2D HitBox;
    public bool IsAttackFinished;
    public override void _Ready()
    {
        base._Ready();
        HitBox = GetNode<Area2D>("../../HitBox");
    }
    public async override void Enter()
    {
        base.Enter();
        IsAttackFinished = false;
        AnimationPlayer.Play("attack");
        await ToSignal(GetTree().CreateTimer(0.4), SceneTreeTimer.SignalName.Timeout);
        HitBox.GetChild<CollisionShape2D>(0).SetDeferred("disabled", false);
        await ToSignal(GetTree().CreateTimer(0.7), SceneTreeTimer.SignalName.Timeout);
        HitBox.GetChild<CollisionShape2D>(0).SetDeferred("disabled", true);
        IsAttackFinished = true;
    }
    public override void Transition()
    {
        if (IsAttackFinished){
            FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
            Parent.ChangeState ("Idle");
        }
        
    }

}
