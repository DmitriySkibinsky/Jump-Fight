using Godot;
using Godot.NativeInterop;
using System;

public partial class Teleport : state
{
	public bool IsTeleportFinished;
	public bool IsTeleportToPlayer;
	public async override void Enter(){
		base.Enter();
		IsTeleportFinished = false;
		IsTeleportToPlayer = false;
		AnimationPlayer.Play("disappear");

		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		
		Random rnd = new Random();
		int NewPosition;

		float DistanceToPlayer = (float)Owner.Get("Direction");
		
		if(Math.Abs(DistanceToPlayer) > 800){
			GD.Print(DistanceToPlayer, "v1");
			int SideOfTeleport = Player.GlobalPosition.X <= 960 ? 1 : -1;
			NewPosition = SideOfTeleport * rnd.Next(100, 200);
			Owner.CallDeferred("Teleport", Player.GlobalPosition.X + NewPosition);
			IsTeleportToPlayer = true;
		}else if(Math.Abs(DistanceToPlayer) < 300){
			GD.Print(DistanceToPlayer, "v2");
			int SideOfTeleport = rnd.Next(2) == 1 ? 1 : -1;
			NewPosition = SideOfTeleport * rnd.Next(600, 700);
			Owner.CallDeferred("Teleport", 960 + NewPosition);
		}else{
			GD.Print(DistanceToPlayer, "v3");
			NewPosition = rnd.Next(320, 1730);
			Owner.CallDeferred("Teleport", NewPosition);
		}
		
		AnimationPlayer.Play("appear");

		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		IsTeleportFinished = true;
	}

	public override void Transition(){
		if (IsTeleportFinished){
			FiniteStateMachine Parent = (FiniteStateMachine)GetParent();
			Random rnd = new Random();
			if (IsTeleportToPlayer){
				Parent.ChangeState("Attack");
			}else{
				switch (rnd.Next(1,3))
            	{
                case 1: 
                    Parent.ChangeState("SpellCast1");
                    break;
				case 2: 
                    Parent.ChangeState("SpellCast2");
                    break;
				}	
			}
		}
	}
}
