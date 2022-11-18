namespace WindowThing.InputHandling;

public class KeyHookKeyEventArgs : KeyEventArgs
{
    public bool IsWindowsKeyPressed { get; }

    public KeyHookKeyEventArgs(Keys keyData, bool isWindowsKeyPressed) : base(keyData)
    {
        IsWindowsKeyPressed = isWindowsKeyPressed;
    }
}
