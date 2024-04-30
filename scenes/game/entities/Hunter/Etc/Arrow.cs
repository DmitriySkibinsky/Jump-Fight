using Godot;
using System;

public partial class Arrow : Area2D
{

	public float Speed = 400;
	public int Damage = 15;
    public double LiveTime = 10;

    public int Direction = 1;

    public Sprite2D AnimSprite;

    public player Player;

    public override void _Ready()
	{
        AnimSprite = GetNode<Sprite2D>("Sprite2D");
        Player = (player)GetTree().GetFirstNodeInGroup("Player");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Position += new Vector2(Speed * (float)delta * Direction, 0);
        LiveTime -= delta;
        if (LiveTime <= 0)
        {
            QueueFree();
        }
    }

    public void TurnAround()
    {
        if ((Direction == 1) == AnimSprite.FlipH)
        {
            AnimSprite.FlipH = !AnimSprite.FlipH;
        }
    }

    public void Collide(Area2D Area)
    {
        if (Area.Name == "HurtBox" && Player.health > 0)
        {
            Player.CallDeferred("GetDamaged", Damage);
            QueueFree();
        }
    }
}
