using Godot;
using Godot.Collections;
using System;

public partial class SpellCast2 : state
{
	public bool IsSpellCastFinished = false;

	public Array<Node2D> Portals = new Array<Node2D>();
	public PackedScene Portal = GD.Load<PackedScene>("res://scenes/game/entities/BringerOfDeath/RangedAttack.tscn");
    public async override void Enter()
    {
        base.Enter();
		IsSpellCastFinished = false;

		AnimationPlayer.Play("spell_cast");
		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
        IsSpellCastFinished = true;
    }
	public Node2D portal; 
	public async void Attack(){
		float initial_x = -45;
		Vector2 BossPosition = (Vector2)Owner.Get("position");
		while (initial_x < 1920){
			initial_x += 275;
			portal = Portal.Instantiate<Node2D>();
        	portal.GlobalPosition = new Vector2(initial_x, BossPosition.Y + 115);
			portal.Set("IsSpellCast2", true);
       		Portals.Add(portal);
        	Owner.GetParent().AddChild(portal);
        	await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
		}

		for (int i = 0; i < Portals.Count; i++){
			Portals[i].CallDeferred("TurnAttack");
		}

		await ToSignal(GetTree().CreateTimer(3), SceneTreeTimer.SignalName.Timeout);

		for (int i = 0; i < Portals.Count; i++){
			Portals[i].QueueFree();
		}
		Portals.Clear();
	}
	public override void Transition()
    {
        if (IsSpellCastFinished){
			Attack();
            FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
            Parent.ChangeState ("Idle");
        }
        
    }
}
