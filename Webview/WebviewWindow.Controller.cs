using System;
using System.Diagnostics;

using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Juno;

public partial class WebviewWindow : Window
{
    void OnWebviewInitialized( object? sender, EventArgs a )
    {
        textStatus.Text = "Loading...";
    }

    void OnWebviewLoaded( object? sender, RoutedEventArgs a )
    {
        textStatus.Text = "Ready!";
    }

    void OnWebviewLoadFailed( string url, int errorCode, string frameName )
    {
        textStatus.Text = $"Load Failed: {errorCode}.";
    }

    void OnButtonBroswerClick( object? sender, RoutedEventArgs a ) 
    {
        Process.Start( new ProcessStartInfo( uri ) { UseShellExecute = true } );
    }

    void OnButtonCopyClick( object? sender, RoutedEventArgs a ) 
    { 
        Clipboard?.SetTextAsync( uri );
    }
}