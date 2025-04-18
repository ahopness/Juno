using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using WebViewControl;

namespace Juno;

public partial class WebviewWindow : Window
{
    TextBlock textStatus;
    Panel WebviewStatusBar()
    {
        var statusBar = new Panel { Margin = new Thickness( 8, 4 ), Height = 24 };

        var stackOptions = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6, HorizontalAlignment = HorizontalAlignment.Left };
	    statusBar.Children.Add( stackOptions );        

        var buttonBrowser = new Button { Content = "Open in Browser", VerticalAlignment = VerticalAlignment.Center };
        buttonBrowser.Click += OnButtonBroswerClick;
        stackOptions.Children.Add( buttonBrowser );
        
        var buttonCopy = new Button { Content = "Copy Link", VerticalAlignment = VerticalAlignment.Center };
        buttonCopy.Click += OnButtonCopyClick;
        stackOptions.Children.Add( buttonCopy );

        textStatus = new TextBlock { Text = "...", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Right };
        statusBar.Children.Add( textStatus );

        return statusBar;
    }

    string description;
    string uri;
    public WebviewWindow( FeedWindow window, string webviewDescription, string webviewUri )
    {
        this.description = webviewDescription;
        this.uri = webviewUri;
        
        this.Title = $"{description} ({uri})";
        this.Icon = new WindowIcon("icon.png");

        this.Width = 1024;
        this.Height = 512;
		
        this.CanResize = true;
        this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        var panelWebview = new DockPanel { LastChildFill = true };
        this.Content = panelWebview;

        var webviewStatusBar = WebviewStatusBar();
        DockPanel.SetDock( webviewStatusBar, Dock.Bottom );
        panelWebview.Children.Add( webviewStatusBar );

        try
        {
            var webview = new WebView { Address = uri, Focusable = true };
            webview.Initialized += OnWebviewInitialized;
            webview.Loaded += OnWebviewLoaded; 
            webview.LoadFailed += OnWebviewLoadFailed;
            panelWebview.Children.Add( webview );
        }
        catch ( Exception e )
        {
            window.RaiseStatus( e.Message );
        }
    }
}
