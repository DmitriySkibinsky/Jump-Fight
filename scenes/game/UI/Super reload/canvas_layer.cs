using Godot;
using System;

public partial class canvas_layer : CanvasLayer
{
    TextureProgressBar Super_bar;
    CharacterBody2D Player;

    public override void _Ready()
    {
        Super_bar = GetNode<TextureProgressBar>("SuperReload");
        Player = (player)GetTree().GetFirstNodeInGroup("Player");
        Super_bar.MaxValue = 100;
        Super_bar.Value = Super_bar.MaxValue;
    }

    public override void _Process(double delta)
    {
    }

    public void _on_player_super_reload(int new_super)
    {
       Super_bar.Value = new_super;
    }

    public void _on_reload_timeout()
    {
        Super_bar.Value += 1;
    }
}
