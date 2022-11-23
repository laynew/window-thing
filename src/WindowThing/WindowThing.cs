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
        _notifyIcon.ContextMenuStrip = CreateMenu();
        _notifyIcon.Visible = true;
        AppIcon.WhenIconChanges(SetIcon);

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
        contextMenuStrip.Items.Add("&New Icon", null, (_, _) => AppIcon.TimeForNewIcon());
        contextMenuStrip.Items.Add("E&xit", null, (_, _) => Application.Exit());
        return contextMenuStrip;
    }

    private void SetIcon(Icon icon)
    {
        _notifyIcon.Icon = icon;
    }
}
