using Godot;
using System;
using System.Collections.Generic;



public class CharacterSelect : CanvasLayer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Creating a character maker for each player
        for (int i = 0; i < Global.PlayerInputs.Count; i++)
        {
            PackedScene CharacterMaker = (PackedScene)ResourceLoader.Load("res://CharacterMaker.tscn");
            CharacterMaker NewMaker = (CharacterMaker)CharacterMaker.Instance();
            NewMaker.Position = new Vector2((float)(960 - (144 * (Global.PlayerInputs.Count - 1)) + (288 * i)), 660);
            NewMaker.PlayerNumber = i;
            AddChild(NewMaker);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float dt)
    {
        //Going to next scene
        var CharacterMakers = GetChildren();
        var numOfMakers = 0;
        var numOfReadyMakers = 0;
        foreach (var node in CharacterMakers)
        {
            if (node is CharacterMaker characterMaker)
            {
                if (characterMaker.isReady) numOfReadyMakers++;
                numOfMakers++;
            }
        }
        if (numOfMakers == numOfReadyMakers)
        {
            GetTree().ChangeScene("res://MapVote.tscn");
        }


        //Animating Background
        Node2D map = GetNode<Node2D>("MenuMap");

        if (map.Position.x <= -4704)
        {
            map.Position = new Vector2(0, 0);
        }
        map.Position = new Vector2((map.Position.x) - 1*dt*240, 0);

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
    }
}

