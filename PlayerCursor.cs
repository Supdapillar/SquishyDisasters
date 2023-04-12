using Godot;
using System;
using System.Collections.Generic;
public class PlayerCursor : AnimatedSprite
{
    [Export]
    public int PlayerNumber = 0;

    public override void _Ready()
    {

    }

    public override void _Process(float dt)
    {
        if (Global.PlayerInputs.Count >= PlayerNumber + 1)
        {
            Vector2 Velocity = new Vector2(0, 0);

            //Sticks
            Velocity.x += Input.GetAxis("move_left_"+PlayerNumber.ToString(),"move_right_"+PlayerNumber.ToString());
            Velocity.y += Input.GetAxis("move_up_"+PlayerNumber.ToString(),"move_down_"+PlayerNumber.ToString());
            Modulate = new Color(Global.PlayerColors[PlayerNumber]);
            GetNode<Label>("PlayerNumber").Text = (PlayerNumber+1).ToString();
            //GD.Print(Input.GetActionStrength("move_left_x_"+PlayerNumber.ToString()));
            //GD.Print(Input.GetAxis("move_left_x_"+PlayerNumber.ToString()).ToString());


            Position += Velocity * 500 * dt;
            Position = new Vector2(
                x: Mathf.Clamp(Position.x, 0, 1920),
                y: Mathf.Clamp(Position.y, 0, 1080)
            );
        }
    }
}