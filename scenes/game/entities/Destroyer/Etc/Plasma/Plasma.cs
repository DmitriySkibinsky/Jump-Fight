using Godot;

public partial class Plasma : Area2D
{

    public float Speed = 500;
    public int Damage = 25;
    public double LiveTime = 10;

    public int Direction = 1;

    AnimatedSprite2D AnimatedSprite;

    public player Player;

    public override void _Ready()
    {
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        Player = (player)GetTree().GetFirstNodeInGroup("Player");
    }

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
        if ((Direction == 1) == AnimatedSprite.FlipH)
        {
            AnimatedSprite.FlipH = !AnimatedSprite.FlipH;
        }
    }

    public void Collide(Area2D Area)
    {
        if (Area.GetCollisionLayerValue(6))
        {
            SetCollisionMaskValue(2, false);
            SetCollisionMaskValue(3, true);
            Direction *= -1;
            TurnAround();
        }
        else if (Area.Name == "HurtBox" || Area.Name == "HurtBoxes")
        {
            if (Area.GetCollisionLayerValue(2) && Player.health > 0)
            {
                Player.CallDeferred("GetDamaged", Damage);
                QueueFree();
            }
            else if (Area.GetCollisionLayerValue(3) && Area.GetParent() is Destroyer Enemy && (int)Enemy.Get("Armor") > 0)
            {
                Enemy.CallDeferred("SetArmorBreak");
                QueueFree();
            }
        }
    }
}
