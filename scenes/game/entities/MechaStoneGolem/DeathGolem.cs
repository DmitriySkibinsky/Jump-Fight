using Godot;
using System;

public partial class DeathGolem : state
{
    public async override void Enter()
    {
        base.Enter();
        AnimationPlayer.Play("death");
        await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
        Owner.QueueFree();
    }
}
