using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Donut : KinematicBody2D
{
    public Vector2 delta = new Vector2(0, 384);

    private static Random rand = new Random();



    public Game game;
    private bool HasHitFloor = false;
    private bool canPlaceTile = false;

    private static float donutSpawner = 1;



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

        if (!HasHitFloor)
            delta.y += dt * 198;

        if (IsOnFloor() && !HasHitFloor)
        {
            hitGround(true);
        }


        if (Position.y > 1944)
        {
            QueueFree();
        }

        MoveAndSlide(delta, new Vector2(0, -1));
    }

    public void _on_DeletionTimer_timeout()
    {
        QueueFree();
    }

    public void hitGround(bool spawnChunks)
    {
        GetNode<Timer>("DeletionTimer").Start();

        //spawning chunks
        if (spawnChunks)
        {
            int randomChunk = rand.Next(0, 8) + 3;
            for (int i = 0; i < randomChunk; i++)
            {
                PackedScene donutChunk = (PackedScene)ResourceLoader.Load("res://Disasters//DonutChunk.tscn");
                DonutChunk IDonutChunk = (DonutChunk)donutChunk.Instance();

                IDonutChunk.Position = Position;

                double randomAngle = (((rand.NextDouble() / 1.33) + 0.125) * (Math.PI)) - Math.PI;

                IDonutChunk.delta.x = ((float)(rand.NextDouble() * Math.Cos(randomAngle) * 200));
                IDonutChunk.delta.y = ((float)((rand.NextDouble() + 0.25) * Math.Sin(randomAngle) * 200));
                game.AddChild(IDonutChunk);
            }
        }


        GetNode<CPUParticles2D>("DoughParticles").Emitting = false;
        GetNode<CPUParticles2D>("SprinkleParticles").Emitting = false;
        GetNode<CPUParticles2D>("ExplosionParticles").Emitting = true;
        GetNode<CPUParticles2D>("ExplosionParticles2").Emitting = true;
        GetNode<Sprite>("Sprite").Visible = false;
        HasHitFloor = true;

        CollisionShape2D collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        Area2D area2D = GetNode<Area2D>("Area2D");
        //Area2D = GetNode<Area2D>("Area2D")
        //RemoveChild(area2D);
        RemoveChild(area2D);
        RemoveChild(collisionShape2D);


        delta.x = 0;
        delta.y = 0;
    }

    public static void RunDisaster(float intensity, Game game)
    {
        donutSpawner -= game.GetPhysicsProcessDeltaTime() * (1 + intensity / 5);
        if (donutSpawner < 0)
        {
            donutSpawner += (float)(rand.NextDouble() * 1.5f + 1.5f);
            PackedScene donut = (PackedScene)ResourceLoader.Load("res://Disasters//Donut.tscn");
            Donut IDonut = (Donut)donut.Instance();

            IDonut.game = game;
            IDonut.Position = new Vector2(rand.Next(24, 1896), rand.Next(-400, -50));
            IDonut.delta.x = (float)(rand.NextDouble() * 800) - 400;

            game.AddChild(IDonut);
        }
    }
    public static void CleanDisaster(Game game)
    {
        foreach (Node node in game.GetChildren())
        {
            if (node is Donut donut)
                donut.QueueFree();
              if (node is DonutChunk donutChunk)
                donutChunk.QueueFree();  
        }
    }

}
