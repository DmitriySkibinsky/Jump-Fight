using Godot;
using System;
using System.Buffers;

public partial class state : Node2D
{
	public Label Debug;
	public player Player;
	public AnimationPlayer AnimationPlayer;
	public override void _Ready()
	{
		Debug = (Label)GetTree().GetFirstNodeInGroup("debug");
		Player = (player)GetTree().GetFirstNodeInGroup("Player");
		AnimationPlayer = (AnimationPlayer)GetTree().GetFirstNodeInGroup("AnimationPlayer");
		this.SetPhysicsProcess(false);

	}

	public virtual void Enter(){
		this.SetPhysicsProcess(true);
	}

	public virtual void Exit(){
		this.SetPhysicsProcess(false);
	}

	public virtual void Transition(){

	}
    public override void _PhysicsProcess(double delta)
    {
		Transition();
    }

}
