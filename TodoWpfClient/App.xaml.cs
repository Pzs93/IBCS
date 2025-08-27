using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace TodoWpfClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private Process? _apiProcess;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Current.DispatcherUnhandledException += (s, args) =>
        {
            MessageBox.Show(args.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
            Current.Shutdown();
        };

        AppDomain.CurrentDomain.UnhandledException += (s, args) =>
        {
            if (args.ExceptionObject is Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        };

        StartApi();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        StopApi();
        base.OnExit(e);
    }

    private void StartApi()
    {
        string apiPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "TodoAPI", "TodoAPI.exe");

        _apiProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = apiPath,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        foreach (Process process in Process.GetProcessesByName("TodoAPI"))
        {
            process.Kill();
            process.WaitForExit();
            process.Dispose();
        }

        _apiProcess.Start();
    }

    private void StopApi()
    {
        if (_apiProcess is not null && !_apiProcess.HasExited)
        {
            _apiProcess.Kill();
            _apiProcess.Dispose();
        }
    }
}

