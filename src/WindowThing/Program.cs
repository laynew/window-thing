using WindowThing.InputHandling;

namespace WindowThing;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        using var keyHook = new KeyHook();
        var command = new WindowSnapCommand(keyHook);
        command.BindShortcut();
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
