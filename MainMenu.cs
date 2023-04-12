using Godot;
using System;

public class MainMenu : CanvasLayer
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        Input.UseAccumulatedInput = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float dt)
    {
        //Animating Background
        Node2D map = GetNode<Node2D>("MenuMap");

        if (map.Position.x <= -4704)
        {
            map.Position = new Vector2(0, 0);
        }
        map.Position = new Vector2((map.Position.x) - 1*dt*240, 0);

        //Making the Add Player Label pulse
        Label addPlayerLabel = GetNode<Label>("AddPlayerLabel");
        addPlayerLabel.Modulate = new Color(1f, 1f, 1f, ((float)Math.Sin(((double)Time.GetTicksMsec()) / 500) / 2.4f) + 0.60f);
        //Making the player counter change
        AnimatedSprite pSprite = GetNode<AnimatedSprite>("PlayerCounter");
        pSprite.Animation = (Global.PlayerInputs.Count.ToString());

        //Adding Cursors foreach player
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
    public void _on_CursorButton_ButtonReleased(int playerNumber)
    {
        GetTree().ChangeScene("res://CharacterSelect.tscn");
    }

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
}