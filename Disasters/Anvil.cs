using Godot;
using System;

public class Anvil : KinematicBody2D
{
    public Vector2 delta = new Vector2(0, 0);
    private static Random rand = new Random();
    public bool hasHitPlayer = false;
    public bool hasHitFloor = false;

    private static float anvilSpawner = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float dt)
    {


    }

    public override void _PhysicsProcess(float dt)
    {
        if (hasHitPlayer)
        {
            delta.y = 0f;
            GetNode<Sprite>("Sprite").Hide();
            GetNode<CPUParticles2D>("DeathParticles").Emitting = true;
        }
        else
        {
            if (IsOnFloor())
            {
                GetNode<CPUParticles2D>("CPUParticles2D").Emitting = true;
                GetNode<KinematicBody2D>("PlatformShape").SetCollisionLayerBit(4, true);
                delta.y = 0f;
                hasHitFloor = true;
            }
            else
            {
                delta.y += dt * 594;
            }
        }

        if (hasHitFloor)
        {
            delta.y = 0f;
            GetNode<Area2D>("Area2D").GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;

        }
        else
        {
            MoveAndSlide(delta, new Vector2(0, -1));
        }
    }

    //This runs the disaster from the games perspective
    public static void RunDisaster(float intensity, Game game)
    {
        anvilSpawner -= game.GetPhysicsProcessDeltaTime() * (2 + intensity / 5);
        if (anvilSpawner < 0)
        {
            anvilSpawner += (float)rand.NextDouble() + 0.5f;
            foreach (Player player in game.ActivePlayers)
            {
                PackedScene anvil = (PackedScene)ResourceLoader.Load("res://Disasters//Anvil.tscn");
                Anvil IAnvil = (Anvil)anvil.Instance();

                IAnvil.Position = new Vector2(player.Position.x + rand.Next(-10, 10), -24 - rand.Next(0, 120));
                game.AddChild(IAnvil);
            }
        }



    }

    public static void CleanDisaster(Game game)
    {
        foreach (Node node in game.GetChildren())
        {
            if (node is Anvil anvil)
                anvil.QueueFree();
        }
    }

    public void _on_DeathTimer_timeout()
    {
        QueueFree();
    }
}
