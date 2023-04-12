using Godot;
using System;
using System.Collections.Generic;

public class Game : Node2D
{

    [Export]
    public PackedScene PlayerScene;


    public List<Player> ActivePlayers = new List<Player>();
    private float timeBeforeNewDisaster = 5;

    private Random rand = new Random();


    private int currentRound = 0;
    private float intensity = 1;

    private bool currentlyDisplayingDisaster = false;
    private string[] disasterNames = { "ANVIL", "RAIN", "DRONE", "COINS", "DONUTS", "" };
    //debug get rid
    public int RandomDisaster = 5;

    public override void _Ready()
    {
        for (int i = 0; i < Global.PlayerInputs.Count; i++)
        {
            var player = (Player)PlayerScene.Instance();

            player.Position = new Vector2((float)(GD.Randi() % 1920), -48);
            player.playerId = i;
            player.IsParachuting = true;

            ActivePlayers.Add(player);
            AddChild(player);
        }

        AddChild(Global.selectedMap.Instance());
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        for (int i = 0; i < ActivePlayers.Count; i++)
        {
            if (ActivePlayers[i].health <= 0)
            {
                foreach (Node node in GetChildren())
                {
                    if (node is Drone drone)
                    {

                        if (drone.targetingPlayer == ActivePlayers[i])
                            drone.QueueFree();
                    }
                }
                ActivePlayers[i].QueueFree();
                ActivePlayers.Remove(ActivePlayers[i]);

            }
            else
            {

            }
        }

        if (ActivePlayers.Count == 0)
        {
            GetTree().ChangeScene("res://MainMenu.tscn");
        }

        //Disaster label
        Label disasterLabel = GetNode<Label>("DisasterLabel");
        disasterLabel.Text = "ROUND " + currentRound.ToString() + " " + disasterNames[RandomDisaster];
        disasterLabel.Modulate = new Color(1f, 1f, 1f, 1f);
    }
    public override void _PhysicsProcess(float delta)
    {
        switch (RandomDisaster)
        {
            case 0:
                Anvil.RunDisaster(intensity, this);
                break;
            case 1:
                Rain.RunDisaster(intensity, this);
                break;
            case 2:
                Drone.RunDisaster(intensity, this);
                break;
            case 3:
                Coin.RunDisaster(intensity, this);
                break;
            case 4:
                Donut.RunDisaster(intensity, this);
                break;
        }
    }

    public void _on_DisasterChange_timeout()
    {
        switch (RandomDisaster)
        {
            case 0:
                Anvil.CleanDisaster(this);
                break;
            case 1:
                Rain.CleanDisaster(this);
                break;
            case 2:
                Drone.CleanDisaster(this);
                break;
            case 3:
                Coin.CleanDisaster(this);
                break;
            case 4:
                Donut.CleanDisaster(this);
                break;
        }

        currentRound++;
        intensity = (float)(currentRound - 0.5f) + ((float)rand.NextDouble());
        RandomDisaster = rand.Next(0, 5);
        Timer disasterChange = GetNode<Timer>("DisasterChange");
        disasterChange.Start(rand.Next(0, 10) + 15);


        Powerup.spawnPowerup(this);
    }
}