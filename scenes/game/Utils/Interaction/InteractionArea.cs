using Godot;
using System;

public partial class InteractionArea : Area2D
{

	[Export]
	public string ActionName = "Interact";

	public Callable Interact;
	public InteractionManager interactionManager;

    public override void _Ready()
    {
        interactionManager = (InteractionManager)GetTree().GetFirstNodeInGroup("InteractionManager");
    }
    public void _on_body_entered(Node2D body){
		if (body.IsInGroup("Player")){
			interactionManager.RegisterArea(this);
		}
	}
	public void _on_body_exited(Node2D body){
		if (body.IsInGroup("Player")){
			interactionManager.UnregisterArea(this);
		}
	}
}
