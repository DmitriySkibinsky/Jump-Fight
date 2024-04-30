using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

public partial class InteractionManager : Node2D
{
	public player Player;
	public Label label;
	public AudioStreamPlayer door;

	public string BaseText = "[E] to";

	public List<InteractionArea> ActiveAreas = new List<InteractionArea>();
	public bool CanInteract = true;
	public override void _Ready()
	{
		Player = (player)GetTree().GetFirstNodeInGroup("Player");
		label = GetNode<Label>("Label");
		door = GetNode<AudioStreamPlayer>("Door");
	}

	public void RegisterArea(InteractionArea area){
		ActiveAreas.Add(area);
	}

	public void UnregisterArea(InteractionArea area){
		int index = ActiveAreas.LastIndexOf(area);
		if (index != -1){
			ActiveAreas.Remove(area);
		}
	}

    public override void _Process(double delta)
{
    if (ActiveAreas.Count > 0 && CanInteract)
    {
        ActiveAreas.Sort(SortByPlayerDistance);
		label.Text = BaseText + ActiveAreas[0].ActionName;
		label.GlobalPosition = ActiveAreas[0].GlobalPosition;
		label.Show();
    }else{
		label.Hide();
	}
}

	public int SortByPlayerDistance(InteractionArea area1, InteractionArea area2)
	{
		double distance1 = Player.GlobalPosition.DistanceTo(area1.GlobalPosition);
		double distance2 = Player.GlobalPosition.DistanceTo(area2.GlobalPosition);
		if (distance1 < distance2)
		{
			return 1; 
		}
		else if (distance1 > distance2)
		{
			return -1;
		}
		else
		{
			return 0;
		}
	}

    public override async void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("interact") && CanInteract && ActiveAreas.Count > 0){
			if (settings.Sound)
			{
                door.Play();
                await ToSignal(door, AudioStreamPlayer.SignalName.Finished);
            }
			CanInteract = false;
			label.Hide();

			ActiveAreas[0].Interact.Call();

			CanInteract = true;
		}
    }
}

