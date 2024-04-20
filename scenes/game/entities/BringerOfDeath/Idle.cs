using Godot;
using System;

public partial class Idle : state
{
    public Node collision;
    public bool IsAwaitEnd = false; 

    public override void Enter()
    {
        base.Enter();
        IsAwaitEnd = false;
        AnimationPlayer.Play("idle");
        
        Await(2.5f);
    }
    public async void Await(float sec){
        await ToSignal(GetTree().CreateTimer(sec), SceneTreeTimer.SignalName.Timeout);
        IsAwaitEnd = true;
    }
    public override void Exit(){

        base.Exit();
    }
    public override void Transition(){
        FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
        Random rnd = new Random();
        if (IsAwaitEnd){
            switch (rnd.Next(1,3))
            {
                case 1: 
                    Parent.ChangeState("Follow");
                    break;
                case 2:
                    Parent.ChangeState("Teleport");
                    break;
            }
        }
        
    }
}
