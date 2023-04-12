using Godot;
using System;

public class Rain : Node2D
{

    public Vector2 delta = new Vector2(0, 96);
    private KinematicBody2D Kbody;
    private bool HasHitFloor = false;

    private static Random rand = new Random();
    private static float rainSpawner = 1;

    public override void _Ready()
    {
        Kbody = GetNode<KinematicBody2D>("KinematicBody2D");
        if (rand.Next(0, 2) == 0)
        {
            Kbody.GetNode<Sprite>("Sprite").FlipH = true;
        }

    }

    public override void _PhysicsProcess(float dt)
    {
        Position += delta * dt;
        if (!HasHitFloor)
            delta.y += dt * 198;

        if (Kbody.IsOnFloor() && !HasHitFloor)
        {
            delta.y = 0;
            HasHitFloor = true;
            Area2D area2D = Kbody.GetNode<Area2D>("Area2D");
            CollisionShape2D collisionShape2D = Kbody.GetNode<CollisionShape2D>("CollisionShape2D");

            Kbody.GetNode<Sprite>("Sprite").Visible = false;
            Kbody.RemoveChild(area2D);
            Kbody.RemoveChild(collisionShape2D);
            GetNode<Timer>("DeletionTimer").Start();

            Kbody.GetNode<CPUParticles2D>("CPUParticles2D").Emitting = true;
        }

        if (Position.y > 1944)
        {
            QueueFree();
        }

        Kbody.MoveAndSlide(delta, new Vector2(0, -1));
    }

    public void _on_DeletionTimer_timeout()
    {
        QueueFree();
    }


    //This runs the disaster from the games perspective
    public static void RunDisaster(float intensity, Game game)
    {
        rainSpawner -= game.GetPhysicsProcessDeltaTime() * (2 + intensity / 5);

        while (rainSpawner < 0)
        {
            rainSpawner += (float)rand.NextDouble() / 3;

            PackedScene rain = (PackedScene)ResourceLoader.Load("res://Disasters//Rain.tscn");
            Rain IRain = (Rain)rain.Instance();
            IRain.Position = new Vector2(rand.Next(0, 1920), -60);
            game.AddChild(IRain);
        }
    }

    public static void CleanDisaster(Game game)
    {
        foreach (Node node in game.GetChildren())
        {
            if (node is Rain rain)
            rain.QueueFree();
        }
    }
}
