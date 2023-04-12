using Godot;
using System;

public class CharacterMaker : Node2D
{
    [Export]
    public int PlayerNumber = 0;
    public bool isReady = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Label pNumLabel = GetNode<Label>("PlayerNumber");
        pNumLabel.Text = " " + (PlayerNumber + 1).ToString();

        Player Player = GetNode<Player>("Player");
        Player.playerId = PlayerNumber;

        Player.Speed = 0;
        Player.JumpBoost = 0;
        Player.RemoveChild(Player.GetNode<CollisionShape2D>("CollisionShape"));
        Player.SetCollisionMaskBit(1, false);
        Player.SetCollisionMaskBit(2, false);
        Player.SetCollisionMaskBit(5, false);


        Player.GetNode<CanvasLayer>("PlayerStats").Visible = false;
        //Player.RemoveChild(Player.GetNode<CollisionObject2D>("Collsi"))

        CursorButton HatButton = GetNode<CursorButton>("HatButton");
        HatButton.PlayerNumber = PlayerNumber;
        CursorButton HLArrow = GetNode<CursorButton>("HLArrow");
        HLArrow.PlayerNumber = PlayerNumber;
        CursorButton CLArrow = GetNode<CursorButton>("CLArrow");
        CLArrow.PlayerNumber = PlayerNumber;
        CursorButton HueButton = GetNode<CursorButton>("HueButton");
        HueButton.PlayerNumber = PlayerNumber;
        CursorButton HRArrow = GetNode<CursorButton>("HRArrow");
        HRArrow.PlayerNumber = PlayerNumber;
        CursorButton CRArrow = GetNode<CursorButton>("CRArrow");
        CRArrow.PlayerNumber = PlayerNumber;
        CursorButton Ready = GetNode<CursorButton>("Ready");
        Ready.PlayerNumber = PlayerNumber;

    }

    public override void _Process(float delta)
    {
        GetNode<Sprite>("BorderColor").Modulate = Global.PlayerColors[PlayerNumber];
        GetNode<Label>("PlayerNumberShadow").Modulate = Global.PlayerColors[PlayerNumber];
    }
    //////// UNGODLY AMOUNTS OF SIGNAL FUNCTIONS ////////

    //Hat buttons
    public void _on_HatButton_ButtonReleased(int playerNumber)
    {
        Random rand = new Random();
        Global.Accessories[PlayerNumber] = rand.Next(0, 6);
    }

    public void _on_HLArrow_ButtonReleased(int playerNumber)
    {
        if (Global.Accessories[PlayerNumber] > 0)
        {
            Global.Accessories[PlayerNumber] -= 1;
        }
        else
        {
            Global.Accessories[PlayerNumber] = 6;
        }
    }

    public void _on_CRArrow_ButtonReleased(int playerNumber)
    {
        if (Global.Accessories[PlayerNumber] < 6)
        {
            Global.Accessories[PlayerNumber] += 1;
        }
        else
        {
            Global.Accessories[PlayerNumber] = 0;
        }
    }


    //Hue buttons
    public void _on_HueButton_ButtonReleased(int playerNumber)
    {
        Random rand = new Random();
        Global.PlayerColors[PlayerNumber] = Color.FromHsv((float)rand.NextDouble(), 1, (float)rand.NextDouble(), 1);
    }

    public void _on_CLArrow_ButtonReleased(int playerNumber)
    {
        float hue;
        float sat;
        float val;

        Global.PlayerColors[PlayerNumber].ToHsv(out hue, out sat, out val);
        Global.PlayerColors[PlayerNumber] = Color.FromHsv(hue += 0.1f, 1, 1, 1);
    }


    public void _on_HRArrow_ButtonReleased(int playerNumber)
    {
        float hue;
        float sat;
        float val;

        Global.PlayerColors[PlayerNumber].ToHsv(out hue, out sat, out val);
        Global.PlayerColors[PlayerNumber] = Color.FromHsv(hue -= 0.1f, 1, 1, 1);
    }

    //Ready buttons
    public void _on_Ready_ButtonReleased(int playerNumber)
    {
        Player Player = GetNode<Player>("Player");
        Player.Visible = false;

        Label PlayerNumber = GetNode<Label>("PlayerNumber");
        PlayerNumber.Visible = false;

        Label PlayerNumberShadow = GetNode<Label>("PlayerNumberShadow");
        PlayerNumberShadow.Visible = false;

        Sprite Border = GetNode<Sprite>("Border");
        Border.Visible = false;

        Sprite BorderColor = GetNode<Sprite>("BorderColor");
        BorderColor.Visible = false;

        CursorButton HatButton = GetNode<CursorButton>("HatButton");
        RemoveChild(HatButton);
        CursorButton HLArrow = GetNode<CursorButton>("HLArrow");
        RemoveChild(HLArrow);
        CursorButton CLArrow = GetNode<CursorButton>("CLArrow");
        RemoveChild(CLArrow);
        CursorButton HueButton = GetNode<CursorButton>("HueButton");
        RemoveChild(HueButton);
        CursorButton HRArrow = GetNode<CursorButton>("HRArrow");
        RemoveChild(HRArrow);
        CursorButton CRArrow = GetNode<CursorButton>("CRArrow");
        RemoveChild(CRArrow);
        CursorButton Ready = GetNode<CursorButton>("Ready");
        RemoveChild(Ready);
        

        Sprite Readied = GetNode<Sprite>("Readied");
        Readied.Visible = true;

        isReady = true;
    }





    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
