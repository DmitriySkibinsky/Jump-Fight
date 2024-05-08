using Godot;
using System;
using static Godot.TextServer;
using static System.Net.Mime.MediaTypeNames;

public partial class Spike : Area2D
{
    public int Damage = 15;
    public bool Collided = false;
    public bool Enable = false;
    public float SpeedMultiplier = 1.5f;

    public AnimationPlayer Anim;

    public player Player;

    public override void _Ready()
    {
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        Player = (player)GetTree().GetFirstNodeInGroup("Player");

        Anim.Play("Spike", SpeedMultiplier);
    }

    public override void _Process(double delta)
    {
        if (Enable && !Collided)
        {
            Godot.Collections.Array<Area2D> OverlappingAreas = GetOverlappingAreas();
            for (int i = 0; i < OverlappingAreas.Count; i++)
            {
                if (OverlappingAreas[i].Name == "HurtBox" && Player.health > 0)
                {
                    Collided = true;
                    Player.CallDeferred("GetDamaged", Damage);
                    break;
                }
            }
        }
    }

    public void SetState(bool State)
    {
        Enable = State;
    }

    public void Destroy(string Animation)
    {
        QueueFree();
    }
}
