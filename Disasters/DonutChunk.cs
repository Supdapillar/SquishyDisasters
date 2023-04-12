using Godot;
using System;

public class DonutChunk : KinematicBody2D
{
    public Vector2 delta = new Vector2();

    public Donut donut;
    Random rand = new Random();

    private bool hasHitPlayer = false;

    private double randomRotation;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<AnimatedSprite>("AnimatedSprite").Animation = (rand.Next(0, 3) + 1).ToString();
        randomRotation = (rand.NextDouble()*360-10);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float dt)
    {
        
    }

    public void hitPlayer()
    {
        if (!hasHitPlayer)
        {
            RemoveChild(GetNode<Area2D>("Area2D"));
        }
        RotationDegrees = 0;
        randomRotation = 0;
        GetNode<CPUParticles2D>("TrailParticles").Emitting = false;
        GetNode<CPUParticles2D>("ExplosionParticles").Emitting = true;
        GetNode<AnimatedSprite>("AnimatedSprite").Visible = false;
        hasHitPlayer = true;
    }

    public override void _PhysicsProcess(float dt)
    {
        RotationDegrees+=(float)randomRotation*dt;
        if (!hasHitPlayer)
        {
            Position += delta * dt;
            delta.y += dt * 198;
        }

        if (Position.y > 1104)
        {
            QueueFree();
        }
        if (IsOnFloor())
        {
            QueueFree();
        }


        MoveAndSlide(delta, new Vector2(0, -1));

    }
}
