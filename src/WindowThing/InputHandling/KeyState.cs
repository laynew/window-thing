namespace WindowThing.InputHandling;

public class KeyState
{
    public bool IsControlPressed { get; private set; }
    public bool IsAltPressed { get; private set; }
    public bool IsShiftPressed { get; private set; }
    public bool IsWindowsKeyPressed { get; private set; }

    public void KeyDown(Keys key)
    {
        switch (key)
        {
            case Keys.LControlKey:
            case Keys.RControlKey:
                IsControlPressed = true;
                break;
            case Keys.LMenu:
            case Keys.RMenu:
                IsAltPressed = true;
                break;
            case Keys.LShiftKey:
            case Keys.RShiftKey:
                IsShiftPressed = true;
                break;
            case Keys.LWin:
            case Keys.RWin:
                IsWindowsKeyPressed = true;
                break;
        }
    }

    public void KeyUp(Keys key)
    {
        switch (key)
        {
            case Keys.LControlKey:
            case Keys.RControlKey:
                IsControlPressed = false;
                break;
            case Keys.LMenu:
            case Keys.RMenu:
                IsAltPressed = false;
                break;
            case Keys.LShiftKey:
            case Keys.RShiftKey:
                IsShiftPressed = false;
                break;
            case Keys.LWin:
            case Keys.RWin:
                IsWindowsKeyPressed = false;
                break;
        }
    }

    public Keys GetKeys(Keys key)
    {
        var keys = key;

        if (IsControlPressed)
        {
            keys |= Keys.Control;
        }

        if (IsShiftPressed)
        {
            keys |= Keys.Shift;
        }

        if (IsAltPressed)
        {
            keys |= Keys.Alt;
        }

        return keys;
    }
}
