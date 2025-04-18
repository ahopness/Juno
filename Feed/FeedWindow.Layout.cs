using Avalonia;
using Avalonia.Media;
using Avalonia.Layout;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Juno;

public class PostGrid : Grid
{
    public string title;
    public string author;
    public string pageURI;
    public string commentsURI;
    public bool isBookmark;

    public PostGrid(FeedWindow window, string postTitle, string postAuthor, string postPageURI, string postCommentsURI, bool postIsBookmark )
    {
        this.title = postTitle;
        this.author = postAuthor;
        this.pageURI = postPageURI;
        this.commentsURI = postCommentsURI;
        this.isBookmark = postIsBookmark;

        this.Margin = new Thickness( 4 );
        
        this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength( 100 ) });
        this.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        
        this.RowDefinitions.Add( new RowDefinition { Height = GridLength.Auto } );
        this.RowDefinitions.Add( new RowDefinition { Height = GridLength.Auto } );
        this.RowDefinitions.Add( new RowDefinition { Height = GridLength.Auto } );

        var paddingButton = new Thickness ( 0, 8 );

        var buttonPost = new Button { Content = "Open", HorizontalAlignment = HorizontalAlignment.Stretch, Padding = paddingButton };
        buttonPost.Click += ( object? sender, RoutedEventArgs a ) => { window.OpenURI( title, pageURI ); };
        Grid.SetColumn( buttonPost, 0 );
        Grid.SetRow( buttonPost, 0 );
        this.Children.Add( buttonPost );

        var buttonComments = new Button { Content = "Comments", HorizontalAlignment = HorizontalAlignment.Stretch, Padding = paddingButton };
        buttonComments.Click += ( object? sender, RoutedEventArgs a ) => { window.OpenURI( ( "Comments of " + title ), commentsURI ); };
        Grid.SetColumn( buttonComments, 0 );
        Grid.SetRow( buttonComments, 1 );
        this.Children.Add( buttonComments );

        var textButtonBookmark = new TextBlock { TextAlignment = TextAlignment.Center };
        var buttonBookmark = new Button { Content = textButtonBookmark, HorizontalAlignment = HorizontalAlignment.Stretch, Padding = paddingButton };;
        if( isBookmark )
        {
            textButtonBookmark.Text = "Remove\nBookmark";
            buttonBookmark.Click += ( object? sender, RoutedEventArgs args ) => window.OnPostGridRemoveBookmark( this );
        }
        else
        {
            textButtonBookmark.Text = "Bookmark";
            buttonBookmark.Click += ( object? sender, RoutedEventArgs args ) => window.OnPostGridAddToBookmarks( this );
        }
        Grid.SetColumn( buttonBookmark, 0 );
        Grid.SetRow( buttonBookmark, 2 );
        this.Children.Add( buttonBookmark );

        var marginText = new Thickness ( 6, 0, 0, 0 );
        
        var textTitle = new TextBlock { Text = title, Opacity = 1, VerticalAlignment = VerticalAlignment.Top, TextWrapping = TextWrapping.Wrap, Margin = marginText };
        Grid.SetColumn( textTitle, 1 );
        Grid.SetRow( textTitle, 0 );
        this.Children.Add( textTitle );
        
        var textAuthor = new TextBlock { Text = "by " + author, VerticalAlignment = VerticalAlignment.Center, Opacity = .5, TextWrapping = TextWrapping.Wrap, Margin = marginText };
        Grid.SetColumn( textAuthor, 1 );
        Grid.SetRow( textAuthor, 1 );
        this.Children.Add( textAuthor );
        
        var textURI = new TextBlock { Text = pageURI, Opacity = .25, VerticalAlignment = VerticalAlignment.Bottom, TextWrapping = TextWrapping.Wrap, Margin = marginText };
        Grid.SetColumn( textURI, 1 );
        Grid.SetRow( textURI, 2 );
        this.Children.Add( textURI );
    }
}

public partial class FeedWindow : Window
{
    StackPanel stackFeed;
    ScrollViewer FeedScrollViewer()
    {
        stackFeed = new StackPanel { Margin = new Thickness( 4, 4, 10, 0 ), Spacing = -2, Orientation = Orientation.Vertical };

        var scrollViewer = new ScrollViewer { Content = stackFeed };
        return scrollViewer;
    }

    StackPanel stackBookmarks;
    ScrollViewer BookmarksScrollViewer()
    {
        var gridBookmarks = new Grid { Margin = new Thickness( 4, 4, 10, 0 ) };

        gridBookmarks.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        gridBookmarks.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        gridBookmarks.RowDefinitions.Add( new RowDefinition { Height = GridLength.Auto } );
        gridBookmarks.RowDefinitions.Add( new RowDefinition { Height = GridLength.Star } );

        var marginButton = new Thickness ( 8, 8 );

        var buttonClear = new Button { Content = "Clear List", HorizontalAlignment = HorizontalAlignment.Stretch, Margin = marginButton };
        buttonClear.Click += OnBookmarkButtonClearClick;
        Grid.SetColumn( buttonClear, 0 );
        Grid.SetRow( buttonClear, 0 );
        gridBookmarks.Children.Add( buttonClear );

        var buttonExport = new Button { Content = "Export List", HorizontalAlignment = HorizontalAlignment.Stretch, Margin = marginButton };
        buttonExport.Click += OnBookmarkButtonExportClick;
        Grid.SetColumn( buttonExport, 1 );
        Grid.SetRow( buttonExport, 0 );
        gridBookmarks.Children.Add( buttonExport );

        stackBookmarks = new StackPanel { Spacing = -2, Orientation = Orientation.Vertical };
        Grid.SetColumn( stackBookmarks, 0 );
        Grid.SetColumnSpan( stackBookmarks, 2 );
        Grid.SetRow( stackBookmarks, 1 );
        gridBookmarks.Children.Add( stackBookmarks );

        var scrollViewer = new ScrollViewer { Content = gridBookmarks };
        return scrollViewer;
    }

    TextBlock textStatus;
    ComboBox comboOptions;
    Panel FeedStatusBar()
    {
        var statusBar = new Panel { Margin = new Thickness( 8, 4 ), Height = 24 };

        var stackOptions = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6, HorizontalAlignment = HorizontalAlignment.Left };
        statusBar.Children.Add( stackOptions );

        var textTitle = new TextBlock { Text = "Juno @", VerticalAlignment = VerticalAlignment.Center };
        stackOptions.Children.Add( textTitle );

        comboOptions = new ComboBox { VerticalAlignment = VerticalAlignment.Center };
        foreach (var website in WebsiteRegistry.list)
        {
            var comboItemWebsite = new ComboBoxItem { Content = website.name };
            comboOptions.Items.Add( comboItemWebsite );
        }
        comboOptions.SelectionChanged += OnComboOptionsSelectionChanged;
        stackOptions.Children.Add( comboOptions );

        textStatus = new TextBlock { Text = "...", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Right };
        statusBar.Children.Add( textStatus );

        return statusBar;
    }

    TabItem tabItemFeed;
    TabItem tabItemBookmarks;
    public FeedWindow()
    {
        this.Title = "Juno";
        this.Icon = new WindowIcon("icon.png");

        this.Width = 450;
        this.Height = 600;

        this.CanResize = false;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        var panelApp = new DockPanel { LastChildFill = true };
        this.Content = panelApp;

        var feedStatusBar = FeedStatusBar();
        DockPanel.SetDock( feedStatusBar, Dock.Bottom );
        panelApp.Children.Add( feedStatusBar );

        var tabControl = new TabControl { Margin = new Thickness( 8, 8, 8, 0 ) };
        panelApp.Children.Add( tabControl );

        tabItemFeed = new TabItem { Header = "Feed", Content = FeedScrollViewer() };
        tabControl.Items.Add( tabItemFeed );
        
        tabItemBookmarks = new TabItem { Header = "Bookmarks", Content = BookmarksScrollViewer() };
        tabControl.Items.Add( tabItemBookmarks );
    }

}
