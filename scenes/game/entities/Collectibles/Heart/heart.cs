using Godot;
using System;

public partial class heart : Area2D
{
    public void _on_body_entered(Node2D body)
	{
		if (body.Name == "Player")
		{
            player Player = (player)GetTree().GetFirstNodeInGroup("Player");
			Player.heal(30, this);
			var tween = GetTree().CreateTween();
            var tween_fade = GetTree().CreateTween();
            Vector2  collect = new Vector2(0, 25);
			tween.TweenProperty(this, "position", Position - collect, 0.3f);
			tween_fade.TweenProperty(this, "modulate:a", 0, 0.3f);
            
        }
	}
	public void Exit(){
        QueueFree();
    }
}
