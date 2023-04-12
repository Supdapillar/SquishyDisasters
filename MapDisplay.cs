using Godot;
using System;
using System.Collections.Generic;

public class MapDisplay : Control
{
    [Signal]
    public delegate void PlayerPressed(int playerNumber, MapDisplay mapDisplay);
    public List<Player> votedPlayers = new List<Player>();
    public override void _Ready()
    {

    }

    public override void _Process(float dt)
    {
        //Aligns players
        for (int i = 0; i < votedPlayers.Count; i++)
        {
            votedPlayers[i].Position = new Vector2((float)((this.RectSize.x / 2) + 40) - (40 * votedPlayers.Count) + (i * 80), 700);
            votedPlayers[i].Scale = new Vector2(10, 10);
            votedPlayers[i].GetNode<CollisionShape2D>("CollisionShape").Disabled = true;
            //votedPlayers[i].SetCollisionLayerBit(2,false);
        }




        //animating background
        var ChildrenMaps = GetChildren();
        foreach (var node in ChildrenMaps)
        {
            if (!(node is AnimatedSprite || node is Player))
            {
                if (node is Node2D map)
                {
                    map.Position = new Vector2((map.Position.x) - 0.5f*dt*240, 0);
                }
            }
            else if (node is Player player)
            {
                if (!votedPlayers.Contains(player))
                {
                    this.RemoveChild(player);
                }
            }

        }
        CursorButton mapButton = GetNode<CursorButton>("MapButton");

        Label mapName = GetNode<Label>("Map Name");
        Label mapNameShadow = mapName.GetNode<Label>("Name Shadow");

        switch (mapButton.Animation)
        {
            case "Click":
                mapName.SetPosition(new Vector2(0, 412));
                mapName.Modulate = new Color(0.5f, 0.5f, 1f);
                mapNameShadow.Modulate = new Color(1f, 1f, 1f, 0f);
                break;
            case "Hover":
                mapName.SetPosition(new Vector2(0, 400));
                mapName.Modulate = new Color(0.5f, 0.5f, 1f);
                mapNameShadow.Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
                break;
            case "Normal":
                mapName.SetPosition(new Vector2(0, 400));
                mapName.Modulate = new Color(1f, 1f, 1f);
                mapNameShadow.Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
                break;
        }
    }

    public void _on_MapButton_ButtonReleased(int playerNumber)
    {
        EmitSignal(nameof(PlayerPressed), playerNumber, this);
    }

    public void ResetMapPosition()
    {
        int mapNumber = 0;
        var ChildrenMaps = GetChildren();
        foreach (var node in ChildrenMaps)
        {
            if (!(node is AnimatedSprite))
            {
                if (node is Node2D map)
                {
                    map.Position = new Vector2(0 + mapNumber * 1972, 0);
                    if (mapNumber == 0)
                    {
                        Label mapName = GetNode<Label>("Map Name");
                        Label mapNameShadow = mapName.GetNode<Label>("Name Shadow");

                        mapName.Text = map.Name;
                        mapNameShadow.Text = map.Name;
                    }
                    mapNumber++;
                }
            }

        }
    }
}
