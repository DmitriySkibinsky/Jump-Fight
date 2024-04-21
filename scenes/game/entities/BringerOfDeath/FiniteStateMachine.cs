using Godot;
using System;

public partial class FiniteStateMachine : Node2D
{
	public state CurrentState;
	public state PreviousState;
	public override void _Ready()
	{
		CurrentState = GetChild<state>(0);
		PreviousState = CurrentState;
		CurrentState.Enter();
	}

	public void ChangeState(string State){
		CurrentState = (state)FindChild(State);
		CurrentState.Enter();

		PreviousState.Exit();
		PreviousState = CurrentState;
	}
}
