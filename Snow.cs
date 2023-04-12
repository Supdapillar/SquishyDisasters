using Godot;
using System;

public class Snow : KinematicBody2D
{
    public Vector2 Delta = new Vector2();
    public bool RightSide = false;

    public float Gravity = 1f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Delta.y = -(GD.Randi()%30);
        if (GD.Randi()%2==0)
        {
            RightSide = true;
        }

        if (RightSide)
        {
            this.Position = new Vector2(2020,0+GD.Randi()%200);
            Delta.x = -((float)GD.Randi()%750)-50;
        }
        else 
        {
            this.Position = new Vector2(-100,0+GD.Randi()%200);
            Delta.x = ((float)GD.Randi()%750)+50;
        }
    }

  public override void _Process(float dt)
  {
      Position+=Delta*dt;
      Delta.y+=Gravity;
  }
}
