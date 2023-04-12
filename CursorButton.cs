using Godot;
using System;
using System.Collections.Generic;

public class CursorButton : AnimatedSprite
{
    [Export]
    public int PlayerNumber = 0;
    [Signal]
    public delegate void ButtonReleased(int playerNumber);

    public List<PlayerCursor> hoveringCursors = new List<PlayerCursor>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //If cursor is hovering
        bool isPressing = false;
        if (hoveringCursors.Count > 0)
        {
            for (int i = 0; i < Global.PlayerInputs.Count; i++)
            {
                if (PlayerNumber != -1)
                {
                    i = PlayerNumber;
                }

                if (Input.IsActionPressed("jump_" + i.ToString())) //PlayerNumber.ToString()
                {
                    if (hoveringCursors.Exists(x => (x.PlayerNumber == i)))
                    {
                        isPressing = true;

                    }
                }

                //Activating Action
                if (Input.IsActionJustReleased("jump_" + i.ToString()))
                {
                    if (hoveringCursors.Exists(x => (x.PlayerNumber == i)))
                    {
                        EmitSignal(nameof(ButtonReleased), i);
                    }
                }

                if (PlayerNumber != -1)
                {
                    i = 9999;
                }
            }
            if (isPressing)
            {
                Animation = "Click";

            }
            else
            {
                Animation = "Hover";

            }
        }
    }

    public void _on_Area2D_area_entered(Area2D area)
    {
        if (area.Name == "CursorCollider")
        {
            PlayerCursor pCursor = ((PlayerCursor)area.GetParent());

            if (PlayerNumber == -1)
            {
                hoveringCursors.Add(pCursor);
                pCursor.Animation = "Hand";
            }
            else
            {
                if (pCursor.PlayerNumber == PlayerNumber)
                {
                    hoveringCursors.Add(pCursor);
                    pCursor.Animation = "Hand";
                }
            }
        }
    }

    public void _on_Area2D_area_exited(Area2D area)
    {
        if (area.Name == "CursorCollider")
        {
            PlayerCursor pCursor = ((PlayerCursor)area.GetParent());

            if (PlayerNumber == -1)
            {
                hoveringCursors.Remove(pCursor);
                pCursor.Animation = "Cursor";
                Animation = "Normal";
            }
            else
            {
                if (pCursor.PlayerNumber == PlayerNumber)
                {
                    hoveringCursors.Remove(pCursor);
                    pCursor.Animation = "Cursor";
                    Animation = "Normal";
                }
            }
        }
    }
}