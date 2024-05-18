using Godot;
using System;
using static Godot.TextServer;
using static System.Net.Mime.MediaTypeNames;

public partial class Spike : Area2D
{

    public enum SoundSettings
    {
        ON,
        OFF
    }

    public SoundSettings Switcher = SoundSettings.ON;

    public int Damage = 15;
    public bool Collided = false;
    public bool Enable = false;
    public float SpeedMultiplier = 1.5f;

    public AnimationPlayer Anim;

    public Node2D Sounds;
    public AudioStreamPlayer2D Sound_Appear;

    public float DeffaultVolume_Sound_Appear;

    public player Player;

    public override void _Ready()
    {
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        Player = (player)GetTree().GetFirstNodeInGroup("Player");

        //Звуки
        Sounds = GetNode<Node2D>("Sounds");
        Sound_Appear = Sounds.GetNode<AudioStreamPlayer2D>("Appear");

        DeffaultVolume_Sound_Appear = Sound_Appear.VolumeDb;
        //

        Anim.Play("Spike", SpeedMultiplier);
    }

    public override void _Process(double delta)
    {

        if (settings.Sound)
        {
            Switcher = SoundSettings.ON;
        }
        else
        {
            Switcher = SoundSettings.OFF;
        }

        switch (Switcher)
        {
            case SoundSettings.ON:
                turn_on();
                break;
            case SoundSettings.OFF:
                turn_off();
                break;
        }


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


    public void turn_on()
    {
        Sound_Appear.VolumeDb = DeffaultVolume_Sound_Appear;
    }

    public void turn_off()
    {
        Sound_Appear.VolumeDb = -80;
    }
}
