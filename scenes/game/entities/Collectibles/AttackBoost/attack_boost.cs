using Godot;
using System;

public partial class attack_boost : Area2D
{
    public void _on_body_entered(Node2D body)
    {
        if (body.Name == "Player")
        {
            CharacterBody2D Player = (player)GetTree().GetFirstNodeInGroup("Player");
            Player.CallDeferred("attack_boost");
            var tween = GetTree().CreateTween();
            var tween_fade = GetTree().CreateTween();
            Vector2 collect = new Vector2(0, 25);
            tween.TweenProperty(GetNode("attack_boost"), "position", Position - collect, 0.3f);
            tween_fade.TweenProperty(GetNode("attack_boost"), "modulate:a", 0, 0.3f);
            tween.TweenCallback(Callable.From(GetNode("attack_boost").QueueFree));
        }
    }
}
