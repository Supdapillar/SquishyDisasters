using Godot;
using System;

public class Powerup : KinematicBody2D
{
    public bool IsParachuting = false;

    private static Random rand = new Random();

    public string PowerupType;

    public Vector2 Delta = new Vector2(0, 0);
    public double lifeSpan = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimatedSprite Aspr = GetNode<AnimatedSprite>("AnimatedSprite");

        //The type of powerup
        switch (rand.Next(3, 4))
        {
            case 0:
                PowerupType = "Speed";
                Aspr.Animation = "Speed";
                break;
            case 1:
                PowerupType = "Jump";
                Aspr.Animation = "Jump";

                break;
            case 2:
                PowerupType = "Health";
                Aspr.Animation = "Health";

                break;
            case 3:
                PowerupType = "Shield";
                Aspr.Animation = "Shield";

                break;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float dt)
    {
        Position += Delta * dt;
        lifeSpan -= dt;
        CalculateParachuting(dt);

    }

    public static void spawnPowerup(Game game)
    {
        PackedScene powerUp = (PackedScene)ResourceLoader.Load("res://Powerup.tscn");
        Powerup IPowerUp = (Powerup)powerUp.Instance();

        AnimatedSprite Aspr = IPowerUp.GetNode<AnimatedSprite>("AnimatedSprite");
        //The way it appears into the map
        switch (rand.Next(0, 1))
        {
            case 0:
                IPowerUp.Position = new Vector2(rand.Next(46, 1884), -200);
                IPowerUp.IsParachuting = true;
                IPowerUp.Delta.y = rand.Next(24, 80);
                IPowerUp.lifeSpan = (rand.NextDouble() * 15) + 15;
                break;
            case 1:
                IPowerUp.PowerupType = "Jump";
                break;
            case 2:
                IPowerUp.PowerupType = "Health";
                break;
        }
        game.AddChild(IPowerUp);
    }

    private void CalculateParachuting(float dt)
    {
        //
        if (IsParachuting)
        {
            GetNode<AnimatedSprite>("Parachute").Visible = true;
            Rotation = -(float)Math.Sin(lifeSpan * 2) / 10;
            Delta.x = (float)(Math.Sin((double)lifeSpan * 2)) * 8 * Delta.y / 24;
            if (IsOnFloor())
            {
                IsParachuting = false;
                Delta.y = 0;
                if (GetNode<AnimatedSprite>("Parachute").Frame == 0)
                    GetNode<AnimatedSprite>("Parachute").Frame = 1;
            }
        }
        else
        {
            Rotation = 0;
            Delta.x = 0;
            GetNode<AnimatedSprite>("Parachute").Playing = true;

            if (GetNode<AnimatedSprite>("Parachute").Frame == 3)
                GetNode<AnimatedSprite>("Parachute").Visible = false;
        }
        MoveAndSlide(Delta, new Vector2(0, -1));
    }
}
