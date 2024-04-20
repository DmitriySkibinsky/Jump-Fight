using Godot;
using System;

public partial class TriggerOnEnterBattle : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void _on_trigger_area_body_exited(Node2D body)
    {
        //if (body.Name == "Player"){
        GetParent().GetParent().GetNode<PlatformingBorder>("PlayerCamera/PlatformingBorder").turn_off();
        GetParent().GetParent().GetNode<in_level_ui>("InLevelUI").animate_out();
        GetParent().GetParent().Set("isBattleSection", true);
        //}
    }
}
