using Godot;
using System;
using System.Collections.Generic;
public class PlayerInput : Node
{
    public bool IsKeyboard;
    public int DeviceId;

    //Movement
    public bool MoveLeft;
    public bool MoveRight;
    public bool Jump;
    public bool MoveDown;
    //Cursor
    public Vector2 CursorPos = new Vector2();
    public bool Click;

    public PlayerInput(int devId, bool isKey)
    {
        DeviceId = devId;
        IsKeyboard = isKey;
        string id = (Global.PlayerInputs.Count).ToString();
        if (isKey)
        {

            CreateNewKeyAction("move_right_" + id, (int)KeyList.D);
            CreateNewKeyAction("move_left_" + id, (int)KeyList.A);
            CreateNewKeyAction("jump_" + id, (int)KeyList.Space);
            CreateNewKeyAction("move_up_" + id, (int)KeyList.W);
            CreateNewKeyAction("move_down_" + id, (int)KeyList.S);
            GD.Print("Added Controls for: " + id);
        }
        else
        {
            CreateNewJoystickAndButtonAction("move_right_" + id, (int)JoystickList.DpadRight,(int)JoystickList.AnalogLx,1);
            CreateNewJoystickAndButtonAction("move_left_" + id, (int)JoystickList.DpadLeft,(int)JoystickList.AnalogLx,-1);
            CreateNewJoyAction("jump_" + id, (int)JoystickList.Button0);
            CreateNewJoystickAndButtonAction("move_up_" + id, (int)JoystickList.DpadUp,(int)JoystickList.AnalogLy,-1);
            CreateNewJoystickAndButtonAction("move_down_" + id, (int)JoystickList.DpadDown,(int)JoystickList.AnalogLy,1);
            GD.Print("Added Controls for: " + id);
        }
    }

    private void CreateNewKeyAction(string inputName, uint inputCheck)
    {
        var ev = new InputEventKey();
        ev.Scancode = inputCheck;

        InputMap.AddAction(inputName);
        InputMap.ActionAddEvent(inputName, ev);
    }

    private void CreateNewJoyAction(string inputName, int butt)
    {
        var ev = new InputEventJoypadButton();
        ev.ButtonIndex = butt;
        ev.Device = DeviceId;


        InputMap.AddAction(inputName);
        InputMap.ActionAddEvent(inputName, ev);
    }

    private void CreateNewJoystickAction(string inputName, int axis, float axisValue)
    {
        var ev = new InputEventJoypadMotion();
        ev.Axis = axis;
        ev.Device = DeviceId;
        ev.AxisValue = axisValue;
        

        InputMap.AddAction(inputName);
        InputMap.ActionAddEvent(inputName, ev);

    }

    private void CreateNewJoystickAndButtonAction(string inputName, int butt, int axis,float axisValue)
    {
        var ev = new InputEventJoypadButton();
        ev.ButtonIndex = butt;
        ev.Device = DeviceId;


        InputMap.AddAction(inputName);
        InputMap.ActionAddEvent(inputName, ev);

        var stickEvent = new InputEventJoypadMotion();
        stickEvent.Axis = axis;
        stickEvent.Device = DeviceId;
        stickEvent.AxisValue = axisValue;

        InputMap.ActionAddEvent(inputName, stickEvent);
    }
}
