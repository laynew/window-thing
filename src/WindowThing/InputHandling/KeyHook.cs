using System.Diagnostics;
using WindowThing.Win32Interop;

namespace WindowThing.InputHandling;

public sealed class KeyHook : IDisposable
{
    private readonly IntPtr _hookId;
    private readonly KeyState _keyState = new();

    public event EventHandler<KeyHookKeyEventArgs>? KeyPressed;
    public event EventHandler<KeyHookKeyEventArgs>? KeyDown;
    public event EventHandler<KeyHookKeyEventArgs>? KeyUp;

    public KeyHook()
    {
        var currentProcess = Process.GetCurrentProcess();
        var currentModule = currentProcess.MainModule;
        var moduleHandle = Kernal32.GetModuleHandle(currentModule!.ModuleName);

        _hookId = User32.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, HookHandler, moduleHandle, 0);
        Debug.WriteLine("Hook acquired: 0x{0:X}", _hookId.ToInt64());
    }

    private IntPtr HookHandler(int nCode, KeyboardMessageType wParam, ref KbdLlHookStruct lParam)
    {
        if (nCode >= 0)
        {
            bool handled;

            if (wParam is KeyboardMessageType.WM_KEYDOWN or KeyboardMessageType.WM_SYSKEYDOWN)
            {
                var keyEventArgs = new KeyHookKeyEventArgs(_keyState.GetKeys((Keys)lParam.vkCode), _keyState.IsWindowsKeyPressed);
                _keyState.KeyDown((Keys)lParam.vkCode);
                OnKeyDown(keyEventArgs);
                handled = keyEventArgs.Handled;
            }
            else
            {
                var keyEventArgs = new KeyHookKeyEventArgs(_keyState.GetKeys((Keys)lParam.vkCode), _keyState.IsWindowsKeyPressed);
                _keyState.KeyUp((Keys)lParam.vkCode);
                OnKeyUp(keyEventArgs);
                handled = keyEventArgs.Handled;
            }

            if (wParam is KeyboardMessageType.WM_KEYUP or KeyboardMessageType.WM_SYSKEYUP)
            {
                var keyEventArgs = new KeyHookKeyEventArgs(_keyState.GetKeys((Keys)lParam.vkCode), _keyState.IsWindowsKeyPressed);
                OnKeyPressed(keyEventArgs);
                handled = keyEventArgs.Handled;
            }

            if (handled)
            {
                Debug.WriteLine("Handling event");
                return 1;
            }
        }

        return User32.CallNextHookEx(_hookId, nCode, wParam, ref lParam);
    }

    public void Dispose()
    {
        User32.UnhookWindowsHookEx(_hookId);
    }

    private void OnKeyPressed(KeyHookKeyEventArgs e)
    {
        KeyPressed?.Invoke(this, e);
    }

    private void OnKeyDown(KeyHookKeyEventArgs e)
    {
        KeyDown?.Invoke(this, e);
    }

    private void OnKeyUp(KeyHookKeyEventArgs e)
    {
        KeyUp?.Invoke(this, e);
    }
}
