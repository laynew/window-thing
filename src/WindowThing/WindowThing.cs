using WindowThing.InputHandling;

namespace WindowThing;

internal class WindowThing : ApplicationContext
{
    private readonly NotifyIcon _notifyIcon = new();
    private readonly KeyHook _keyHook;
    private readonly WindowSnapCommand _command;

    public WindowThing()
    {
        Application.ApplicationExit += ApplicationOnApplicationExit;

        _notifyIcon.Text = @"Window Thing";
        _notifyIcon.Icon = AppIcon.Icon;
        _notifyIcon.ContextMenuStrip = CreateMenu();
        _notifyIcon.Visible = true;

        _keyHook = new KeyHook();
        _command = new WindowSnapCommand(_keyHook);
        _command.BindShortcut();
    }

    private void ApplicationOnApplicationExit(object? sender, EventArgs e)
    {
        _keyHook.Dispose();
        _command.Dispose();
    }

    private static ContextMenuStrip CreateMenu()
    {
        var contextMenuStrip = new ContextMenuStrip();
        contextMenuStrip.Items.Add("E&xit", null, (_, _) => Application.Exit());
        return contextMenuStrip;
    }
}
