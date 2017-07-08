using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputReader : MonoBehaviour {

    public static InputReader Instance { get; set; }
    public int controlSchemeIndex = 0;
    InputDevice inputDevice;
    //0 = joypad, 1 = keyboard, 2 = other

    

    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        inputDevice = InputManager.ActiveDevice;
    }

    public static bool DeviceActive()
    {
        return Instance.inputDevice != null;
    }

    //TODO: Make array of control buttons for interchangable controls as well as account for whether the input decice is a controller or a keyboard
    public static bool MenuButton()
    {
        return Instance.inputDevice.Action4.WasPressed || Input.GetKeyDown(KeyCode.A);
    }
    
    public static bool SubmitButton()
    {
        return Instance.inputDevice.Action1.WasPressed || Input.GetKeyDown(KeyCode.Z);
    }

    public static bool CancelButton()
    {
        return Instance.inputDevice.Action2.WasPressed || Input.GetKeyDown(KeyCode.X);
    }

    public static bool MenuLeftShoulder()
    {
        return Instance.inputDevice.LeftBumper.WasPressed;
    }

    public static bool MenuLeftTrigger()
    {
        return Instance.inputDevice.LeftTrigger.WasPressed;
    }

    public static bool MenuRightShoulder()
    {
        return Instance.inputDevice.RightBumper.WasPressed;
    }

    public static bool MenuRightTrigger()
    {
        return Instance.inputDevice.RightTrigger.WasPressed;
    }

    public static bool LeftShoulder()
    {
        return Instance.inputDevice.LeftBumper.IsPressed;
    }
    public static bool RightShoulder()
    {
        return Instance.inputDevice.RightBumper.IsPressed;
    }

    public static bool LeftTrigger()
    {
        return Instance.inputDevice.LeftTrigger.IsPressed;
    }
    public static bool RightTrigger()
    {
        return Instance.inputDevice.RightTrigger.IsPressed;
    }

    public static bool MenuUp()
    {
        return Instance.inputDevice.DPadUp.WasPressed ||
               Instance.inputDevice.LeftStickUp.WasPressed ||
               Input.GetKeyDown(KeyCode.UpArrow);
    }

    public static bool MenuDown()
    {
        return Instance.inputDevice.DPadDown.WasPressed ||
               Instance.inputDevice.LeftStickDown.WasPressed ||
               Input.GetKeyDown(KeyCode.DownArrow);
    }

    public static bool MenuLeft()
    {
        return Instance.inputDevice.DPadLeft.WasPressed ||
               Instance.inputDevice.LeftStickLeft.WasPressed ||
               Input.GetKeyDown(KeyCode.LeftArrow);
    }

    public static bool MenuRight()
    {
        return Instance.inputDevice.DPadRight.WasPressed ||
               Instance.inputDevice.LeftStickRight.WasPressed ||
               Input.GetKeyDown(KeyCode.RightArrow);
    }

    public static float AxisHoriz()
    {
        if(Mathf.Abs(Instance.inputDevice.DPadX) > .1)
        {
            return Instance.inputDevice.DPadX;
        }
        else if(Mathf.Abs(Instance.inputDevice.LeftStickX) > .1)
        {
            return Instance.inputDevice.LeftStickX;
        }
        else if (Mathf.Abs(Input.GetAxis("Horizontal")) > .1)
        {
            return Input.GetAxis("Horizontal");
        }
        else
        {
            return 0;
        }
    }

    public static bool PauseButton()
    {
        return Instance.inputDevice.Command.WasPressed;
    }

    public static float AxisVert()
    {
        if (Mathf.Abs(Instance.inputDevice.DPadY) > .1)
        {
            return Instance.inputDevice.DPadY;
        }
        else if (Mathf.Abs(Instance.inputDevice.LeftStickY) > .1)
        {
            return Instance.inputDevice.LeftStickY;
        }
        else if (Mathf.Abs(Input.GetAxis("Vertical")) > .1)
        {
            return Input.GetAxis("Vertical");
        }
        else
        {
            return 0;
        }
    }

}
