using System;

using Avalonia;
using Avalonia.Styling;
using Avalonia.Controls.ApplicationLifetimes;

using WebViewControl;

using AvaloniaAero;

namespace Juno;

public class Program
{
    public class JunoApp : Application
    {
        public override void OnFrameworkInitializationCompleted()
        {
            WebView.Settings.LogFile = "cef.log";

            Styles.Add( new AeroTheme() );

            if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
                desktop.MainWindow = new FeedWindow();

            base.OnFrameworkInitializationCompleted();
        }
    }

    [STAThread]
    public static void Main( string[] args ) 
    {
        var app = AppBuilder.Configure<JunoApp>();
        app.UsePlatformDetect();
        app.StartWithClassicDesktopLifetime( args );
    }
}
