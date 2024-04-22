using Godot;
using System;

public partial class health_bar : CanvasLayer
{
    public TextureProgressBar Health_bar;
    CharacterBody2D Player;

    public override void _Ready()
    {
        Health_bar = GetNode<TextureProgressBar>("HealthBar");
        Player = (player)GetTree().GetFirstNodeInGroup("Player");
        Health_bar.MaxValue = (double)Player.Get("health");
        Health_bar.Value = Health_bar.MaxValue;
    }

    public override void _Process(double delta)
    {
    }

    public void _on_player_health_changed(int new_health)
    {
        Health_bar.Value = new_health; 
    }
}
