using Godot;
using System;

public class Coin : KinematicBody2D
{
    public Vector2 delta = new Vector2(0, 96);

    private static Random rand = new Random();

    private double lifeSpan;
    private static float coinSpawner = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lifeSpan = rand.NextDouble() * 5 + 10;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float dt)
    {
        lifeSpan -= dt;
        if (Position.y > 1960 || lifeSpan <= 0)
        {
            QueueFree();
        }

        //Pulsing at low life
        if (GetNode<AnimatedSprite>("AnimatedSprite").Visible == true)
        {
            if (lifeSpan < 5)
            {
                double lifeSpanInvert = 6 - lifeSpan;
                double LifeSin = (Math.Sin(4 * lifeSpanInvert * lifeSpanInvert));
                if (LifeSin < 0)
                {
                    Modulate = new Color(1f, 1f, 1f, 0.5f);
                }
                else
                {
                    Modulate = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
    }

    public override void _PhysicsProcess(float dt)
    {
        MoveAndSlide(delta, new Vector2(0, -1));
        //Position += delta * dt;

        delta.y += dt * 300;

        if (IsOnFloor())
        {
            delta.x = Mathf.Lerp(delta.x, 0, dt);
            delta.y = 0;
        }

    }

    public void PlayerCollect()
    {
        Area2D area2D = GetNode<Area2D>("Area2D");
        CPUParticles2D particles = GetNode<CPUParticles2D>("CPUParticles2D");
        CPUParticles2D plusOne = GetNode<CPUParticles2D>("+1");
        AnimatedSprite Asprite = GetNode<AnimatedSprite>("AnimatedSprite");


        RemoveChild(area2D);
        Asprite.Visible = false;
        particles.Emitting = true;
        plusOne.Emitting = true;
        lifeSpan = 1;
    }

    //This runs the disaster from the games perspective
    public static void RunDisaster(float intensity, Game game)
    {

        coinSpawner -= game.GetPhysicsProcessDeltaTime() * (1 + intensity / 8);
        //spawning the disaster
        if (coinSpawner < 0)
        {
            coinSpawner += (float)rand.NextDouble() / 3 + 0.33f;
            if (rand.Next(0, 80) == 0) // Coin Explosion
            {
                int randomAmount = rand.Next(0, 6) + 5 + (int)Math.Floor(intensity);
                Vector2 randomPos = new Vector2(rand.Next(0, 1920), rand.Next(-80, 400));
                for (int i = 0; i < randomAmount; i++)
                {
                    PackedScene coin = (PackedScene)ResourceLoader.Load("res://Disasters//Coin.tscn");
                    Coin ACoin = (Coin)coin.Instance();
                    double angle = rand.NextDouble() * (Math.PI * 2) - Math.PI;

                    ACoin.delta.x = (float)Math.Cos(angle) * 200;
                    ACoin.delta.y = (float)Math.Sin(angle) * 200;
                    ACoin.Position = randomPos;
                    game.AddChild(ACoin);
                }
            }
            else // Normal Coin Spawn
            {
                PackedScene coin = (PackedScene)ResourceLoader.Load("res://Disasters//Coin.tscn");
                Coin ACoin = (Coin)coin.Instance();
                game.AddChild(ACoin);

                ACoin.Position = new Vector2(rand.Next(0, 1920), -24 - rand.Next(0, 50));
                ACoin.delta.x = (float)rand.NextDouble() * 80 - 40;

            }
        }
    }

    public static void CleanDisaster(Game game)
    {
        foreach (Node node in game.GetChildren())
        {
            if (node is Coin coin)
                coin.QueueFree();
        }
        Player highestCoinPlayer = game.ActivePlayers[0];
        foreach (Player player in game.ActivePlayers)
        {
            if (player.Coins > highestCoinPlayer.Coins)
            {
                highestCoinPlayer = player;
            }
        }
        foreach (Player player in game.ActivePlayers)
        {
            player.Coins = 0;
        }

        PackedScene powerUp = (PackedScene)ResourceLoader.Load("res://Powerup.tscn");
        Powerup IPowerUp = (Powerup)powerUp.Instance();

        IPowerUp.Position = new Vector2(highestCoinPlayer.Position.x, highestCoinPlayer.Position.y - 72);
        IPowerUp.IsParachuting = true;
        IPowerUp.Delta.y = rand.Next(24, 80);
        IPowerUp.lifeSpan = (rand.NextDouble() * 15) + 15;
        game.AddChild(IPowerUp);


    }
}
