using Godot;
using System;
using System.Collections.Generic;
public class Global : Node
{
    public static List<PlayerInput> PlayerInputs { get; set; }
    public static List<Color> PlayerColors { get; set; } // property
    public static List<int> Accessories { get; set; }
    public static PackedScene selectedMap;
    //Maps
    public static string[] MapList = {"Silly Swamp"
    ,"Sno"
    ,"Clif"
    ,"Thats a wonderous question good chap"
    ,"Island"
    ,"Spaec"
    ,"Vancano"
    }; 

    public static string[] DisasterList = {"Rain"};
    public override void _Ready()
    {
        PlayerInputs = new List<PlayerInput>();
        PlayerColors = new List<Color>() { new Color(0, 1, 0, 1), new Color(0, 0, 1, 1), new Color(1, 0, 1, 1), new Color(1, 0.5f, 0, 1) };
        Accessories = new List<int>() { 0, 0, 0, 0 };
    }


    public static void AddNewPlayerInput(int id, bool isKey)
    {
        bool CanAdd = true;
        foreach (PlayerInput pInput in PlayerInputs)
        {
            if (pInput.DeviceId == id && pInput.IsKeyboard == isKey)
            {
                CanAdd = false;
                break;
            }
        }
        if (CanAdd) PlayerInputs.Add(new PlayerInput(id, isKey));
    }
}