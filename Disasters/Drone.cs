using Godot;
using System;
using System.Collections.Generic;

public class Drone : Node2D
{


    public Vector2 delta = new Vector2();
    public Player targetingPlayer;
    float speed = 1.5f;
    public static Random rand = new Random();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float dt)
    {
            Position += delta * dt;
            delta.x = Mathf.Lerp(delta.x, 0, dt / 2);
            delta.y = Mathf.Lerp(delta.y, 0, dt / 2);
            var mouse_pos = GetGlobalMousePosition();
            var mx = mouse_pos.x;
            var my = mouse_pos.y;

            double TargetAngle = Math.Atan2((double)targetingPlayer.Position.y - Position.y, (double)targetingPlayer.Position.x - Position.x);

            delta.x += (float)Math.Cos(TargetAngle) * dt * 100 * speed;
            delta.y += (float)Math.Sin(TargetAngle) * dt * 100 * speed;

            GetNode<Label>("Label").Text = ((TargetAngle + Math.PI * 2) % (Math.PI * 2)).ToString();

            UpdateThrusterAnimation("ThrusterBR", (TargetAngle + Math.PI * 2) % (Math.PI * 2));
            UpdateThrusterAnimation("ThrusterTR", (TargetAngle + Math.PI * 2) % (Math.PI * 2));
            UpdateThrusterAnimation("ThrusterBL", (TargetAngle + Math.PI * 2) % (Math.PI * 2));
            UpdateThrusterAnimation("ThrusterTL", (TargetAngle + Math.PI * 2) % (Math.PI * 2));

    }
    public void _on_ThrusterBR_animation_finished()
    {
        AnimatedThruster("ThrusterBR");
    }
    public void _on_ThrusterTR_animation_finished()
    {
        AnimatedThruster("ThrusterTR");
    }
    public void _on_ThrusterBL_animation_finished()
    {
        AnimatedThruster("ThrusterBL");
    }
    public void _on_ThrusterTL_animation_finished()
    {
        AnimatedThruster("ThrusterTL");
    }

    private void UpdateThrusterAnimation(string thrusterName, double CurrentAngle)
    {
        AnimatedSprite ThrusterAni = GetNode<Node2D>("Thruster").GetNode<AnimatedSprite>(thrusterName);
        const double _QPI = Math.PI / 8;


        switch (thrusterName)
        {
            case "ThrusterTL":
                if (CurrentAngle >= 0 & CurrentAngle < Math.PI / 2 + _QPI || (CurrentAngle > (Math.PI * 2 - _QPI)))
                {
                    if (!ThrusterAni.Visible)
                    {
                        ThrusterAni.Visible = true;
                        ThrusterAni.Animation = "Startup";
                    }
                }
                else
                {
                    ThrusterAni.Animation = "TurnOff";

                }
                break;
            case "ThrusterTR":
                if (CurrentAngle > Math.PI / 2 - _QPI & CurrentAngle < Math.PI + _QPI)
                {
                    if (!ThrusterAni.Visible)
                    {
                        ThrusterAni.Visible = true;
                        ThrusterAni.Animation = "Startup";
                    }
                }
                else
                {
                    ThrusterAni.Animation = "TurnOff";

                }
                break;
            case "ThrusterBR":
                if (CurrentAngle > Math.PI - _QPI & CurrentAngle < (Math.PI + Math.PI / 2) + _QPI)
                {
                    if (!ThrusterAni.Visible)
                    {
                        ThrusterAni.Visible = true;
                        ThrusterAni.Animation = "Startup";
                    }
                }
                else
                {
                    ThrusterAni.Animation = "TurnOff";

                }
                break;
            case "ThrusterBL":
                if (CurrentAngle > (Math.PI + Math.PI / 2) - _QPI & CurrentAngle < Math.PI * 2 || CurrentAngle < _QPI)
                {
                    if (!ThrusterAni.Visible)
                    {
                        ThrusterAni.Visible = true;
                        ThrusterAni.Animation = "Startup";
                    }
                }
                else
                {
                    ThrusterAni.Animation = "TurnOff";

                }
                break;
        }
    }

    public void AnimatedThruster(string thrusterName)
    {
        AnimatedSprite ThrusterAni = GetNode<Node2D>("Thruster").GetNode<AnimatedSprite>(thrusterName);
        switch (ThrusterAni.Animation)
        {
            case "Startup":
                ThrusterAni.Animation = "Normal";
                break;
            case "TurnOff":
                ThrusterAni.Visible = false;
                break;
        }
    }

    public static void RunDisaster(float intensity, Game game)
    {
        //Setup the drones
        List<Drone> childDrones = new List<Drone>();
        foreach (var child in game.GetChildren())
        {
            if (child is Drone drone)
            {
                childDrones.Add(drone);
            }
        }

        if (childDrones.Count == 0)
        {
            GD.Print("Settings up drones");
            foreach (Player player in game.ActivePlayers)
            {
                PackedScene drone = (PackedScene)ResourceLoader.Load("res://Disasters//Drone.tscn");
                Drone IDrone = (Drone)drone.Instance();

                IDrone.targetingPlayer = player;
                IDrone.speed = 0.75f + (intensity / 2);
                IDrone.GetNode<Sprite>("DroneInside").Modulate = Global.PlayerColors[IDrone.targetingPlayer.playerId];
                IDrone.GetNode<Node2D>("Thruster").GetNode<AnimatedSprite>("ThrusterBR").Modulate = Global.PlayerColors[IDrone.targetingPlayer.playerId];
                IDrone.GetNode<Node2D>("Thruster").GetNode<AnimatedSprite>("ThrusterTR").Modulate = Global.PlayerColors[IDrone.targetingPlayer.playerId];
                IDrone.GetNode<Node2D>("Thruster").GetNode<AnimatedSprite>("ThrusterBL").Modulate = Global.PlayerColors[IDrone.targetingPlayer.playerId];
                IDrone.GetNode<Node2D>("Thruster").GetNode<AnimatedSprite>("ThrusterTL").Modulate = Global.PlayerColors[IDrone.targetingPlayer.playerId];
                IDrone.Position = new Vector2(980, 500);
                game.AddChild(IDrone);
            }
        }
    }
    public static void CleanDisaster(Game game)
    {
        foreach (Node node in game.GetChildren())
        {
            if (node is Drone drone)
                drone.QueueFree();
        }
    }
}
