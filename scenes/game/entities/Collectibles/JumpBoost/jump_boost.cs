using Godot;
using System;

public partial class jump_boost : Area2D
{
    public void _on_body_entered(Node2D body)
    {
        if (body.Name == "Player")
        {
            CharacterBody2D Player = (player)GetTree().GetFirstNodeInGroup("Player");
            Player.CallDeferred("jump_boost");
            var tween = GetTree().CreateTween();
            var tween_fade = GetTree().CreateTween();
            Vector2 collect = new Vector2(0, 25);
            tween.TweenProperty(GetNode("jump_boost"), "position", Position - collect, 0.3f);
            tween_fade.TweenProperty(GetNode("jump_boost"), "modulate:a", 0, 0.3f);
            tween.TweenCallback(Callable.From(GetNode("jump_boost").QueueFree));
        }
    }
}
