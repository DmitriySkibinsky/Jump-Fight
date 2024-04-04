using Godot;
using Godot.NativeInterop;
using System;

public partial class EndRoom : Node2D
{
	public string NextScenePath = "res://scenes/game/Level/Level1/level1.tscn";
	public InteractionArea interactionArea;
	public override void _Ready()
	{
		interactionArea = GetNode<InteractionArea>("Doors/InteractionArea");
		interactionArea.Interact = new Callable(this, "Interact");
	}

	public void Interact(){
		GetTree().ChangeSceneToFile(NextScenePath);
	}
	
}
