using Godot;
using Godot.Collections;
using System;

public partial class SpellCast1 : state
{
    public bool IsSpellCastFinished = false;
    public bool IsAwaitEnd = false;
	public PackedScene PortalScene = GD.Load<PackedScene>("res://scenes/game/entities/BringerOfDeath/RangedAttack.tscn");
    public RangedAttack Portal;

    public override void _Ready()
    {
        base._Ready();
        SetProcess(false);
    }
    public async override void Enter()
    {
        base.Enter();
        SetProcess(false);
		IsSpellCastFinished = false;
        IsAwaitEnd = false;
        Portal = new RangedAttack();
		AnimationPlayer.Play("spell_cast");
		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
        IsSpellCastFinished = true;
        
    }
    public async void Await(float sec){
        await ToSignal(GetTree().CreateTimer(sec), SceneTreeTimer.SignalName.Timeout);
        IsAwaitEnd = true;
    }

	public void SpawnPortal(){
		Portal = PortalScene.Instantiate<RangedAttack>();
        Portal.Set("IsSpellCast2", false);
        Owner.GetParent().AddChild(Portal);
        SetProcess(true);

        Await(3);
	}

    public async override void _Process(double delta)
    {
        Portal.GlobalPosition = new Vector2(Player.GlobalPosition.X-75, Player.GlobalPosition.Y+65);
        if (IsAwaitEnd){
            SetProcess(false);
            Portal.CallDeferred("TurnAttack");
            await ToSignal(GetTree().CreateTimer(2), SceneTreeTimer.SignalName.Timeout);
            Portal.QueueFree();
            Portal = new RangedAttack();
        }
    }
    public override void Transition()
    {
        if (IsSpellCastFinished){
            SpawnPortal();
            FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
            Parent.ChangeState ("Idle");
        }
        
    }
}
