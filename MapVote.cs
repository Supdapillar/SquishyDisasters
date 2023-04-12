using Godot;
using System;
using System.Collections.Generic;

public class MapVote : Control
{
    private List<PackedScene> MapSelection = new List<PackedScene>();
    private int countDown = 10;
    private Random rand = new Random();

    public override void _Ready()
    {
        for (int i = 0; i < 3; i++)
        {
            PackedScene RandomMap = (PackedScene)ResourceLoader.Load("res://Maps//" + Global.MapList[rand.Next(0, Global.MapList.Length)] + ".tscn");

            while (MapSelection.Contains(RandomMap))
            {
                RandomMap = (PackedScene)ResourceLoader.Load("res://Maps//" + Global.MapList[rand.Next(0, Global.MapList.Length)] + ".tscn");
            }
            MapSelection.Add(RandomMap);
        }
        // rand.Next(0, Global.MapList.Length)



        for (int i = 0; i < 3; i++)
        {
            MapDisplay MapDisplay = GetNode<MapDisplay>("MapDisplay" + i.ToString());
            MapDisplay.AddChild(MapSelection[i].Instance());
            MapDisplay.AddChild(MapSelection[i].Instance());
            MapDisplay.ResetMapPosition();
            //AddChild(MapDisplay);
        }

    }

    public override void _Process(float delta)
    {

        //Adding cursors
        for (int i = 0; i < Global.PlayerInputs.Count; i++)
        {
            bool containsCursor = false;
            var player_cursors = GetChildren();
            foreach (var node in player_cursors)
            {
                if (node is PlayerCursor playerCursor)
                {
                    if (playerCursor.PlayerNumber == i)
                    {
                        containsCursor = true;
                    }
                }
            }
            if (!containsCursor)
            {
                PackedScene cursor = (PackedScene)ResourceLoader.Load("res://PlayerCursor.tscn");
                PlayerCursor new_cursor = (PlayerCursor)cursor.Instance();

                new_cursor.PlayerNumber = i;
                AddChild(new_cursor);
            }
        }
        //Checking if all players have voted
        MapDisplay mapDisplay0 = GetNode<MapDisplay>("MapDisplay0");
        MapDisplay mapDisplay1 = GetNode<MapDisplay>("MapDisplay1");
        MapDisplay mapDisplay2 = GetNode<MapDisplay>("MapDisplay2");
        if (mapDisplay0.votedPlayers.Count + mapDisplay1.votedPlayers.Count + mapDisplay2.votedPlayers.Count == Global.PlayerInputs.Count)
        {
            GetNode<Timer>("MapCountdown").WaitTime = 1;
        }
    }

    private Player PreparePlayer(int playerNumber)
    {
        PackedScene packedPlayer = (PackedScene)ResourceLoader.Load("res://Player.tscn");
        Player player = (Player)packedPlayer.Instance();

        player.Gravity = 0;
        player.Speed = 0;
        player.Position = new Vector2(-500, -500);
        player.GetNode<Area2D>("PlayerCollider").GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
        player.playerId = playerNumber;
        player.GetNode<CanvasLayer>("PlayerStats").Visible = false;

        for (int i = 0; i < 3; i++)
        {
            MapDisplay md = GetNode<MapDisplay>("MapDisplay" + i.ToString());
            md.votedPlayers.RemoveAll(x => (x.playerId == player.playerId));
        }
        return player;

    }
    public void _on_MapDisplay0_PlayerPressed(int playerNumber, MapDisplay mapDisplay)
    {
        Player player = PreparePlayer(playerNumber);
        mapDisplay.votedPlayers.Add(player);
        mapDisplay.AddChild(player);
    }

    public void _on_MapDisplay1_PlayerPressed(int playerNumber, MapDisplay mapDisplay)
    {
        Player player = PreparePlayer(playerNumber);
        mapDisplay.votedPlayers.Add(player);
        mapDisplay.AddChild(player);
    }

    public void _on_MapDisplay2_PlayerPressed(int playerNumber, MapDisplay mapDisplay)
    {
        Player player = PreparePlayer(playerNumber);
        mapDisplay.votedPlayers.Add(player);
        mapDisplay.AddChild(player);
    }


    public void _on_MapCountdown_timeout()
    {
        countDown--;
        Label Counter = GetNode<Sprite>("Title").GetNode<Label>("Counter");
        Counter.Text = " " + countDown.ToString();

        GetNode<Timer>("MapCountdown").Start();

        //Switching to the game scene and readying the maps
        if (countDown < 1)
        {
            MapDisplay mapDisplay0 = GetNode<MapDisplay>("MapDisplay0");
            MapDisplay mapDisplay1 = GetNode<MapDisplay>("MapDisplay1");
            MapDisplay mapDisplay2 = GetNode<MapDisplay>("MapDisplay2");

            List<PackedScene> baggedMaps = new List<PackedScene>();

            for (int i = 0; i < mapDisplay0.votedPlayers.Count; i++)
            {
                baggedMaps.Add(MapSelection[0]);
            }
            for (int i = 0; i < mapDisplay1.votedPlayers.Count; i++)
            {
                baggedMaps.Add(MapSelection[1]);
            }
            for (int i = 0; i < mapDisplay2.votedPlayers.Count; i++)
            {
                baggedMaps.Add(MapSelection[2]);
            }
            if (baggedMaps.Count == 0)
            {
                Global.selectedMap = (PackedScene)ResourceLoader.Load("res://Maps//" + Global.MapList[rand.Next(0, Global.MapList.Length)] + ".tscn");
            }
            else
            {
                Global.selectedMap = baggedMaps[rand.Next(0, baggedMaps.Count - 1)];

            }


            GetTree().ChangeScene("res://Game.tscn");

        }
    }



    //debug this is for adding players and i only want it for the menu /////
    public override void _Input(InputEvent @event)
    {
        //Keyboard
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Scancode == ((uint)KeyList.Space))
                Global.AddNewPlayerInput(eventKey.Device, true);
        }
        //Mouse
        if (@event is InputEventMouse eventMouse)
        {
            if (eventMouse.ButtonMask == ((uint)ButtonList.Left))
                Global.AddNewPlayerInput(eventMouse.Device, true);
        }
        //Gamepad
        if (@event is InputEventJoypadButton eventJoypadButton)
        {
            if (eventJoypadButton.ButtonIndex == ((uint)JoystickList.Button0))
                Global.AddNewPlayerInput(eventJoypadButton.Device, false);
        }
    }
    //debug this is for adding players and i only want it for the menu /////


}
