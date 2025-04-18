using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

using CodeHollow.FeedReader;

namespace Juno;

public partial class FeedWindow : Window
{
    async void LoadWebsite( Website website )
    {
        Title = "Juno";
        RaiseStatus( "Fetching data..." );

        stackFeed.Children.Clear();

        Feed feed;
        try
        {
            feed = await FeedReader.ReadAsync( website.RSSAddress );
        }
        catch ( Exception e )
        {
            RaiseStatus( e.Message );
            return;
        }
        
        RaiseStatus("Updating feed..");

        foreach ( FeedItem postData in feed.Items )
        {
            try
            {
                var post = new PostGrid
                (
                    this,
                    postData.Title, 
                    String.IsNullOrEmpty( postData.Author ) ? postData.SpecificItem.Element.Elements().FirstOrDefault( node => node.Name.ToString().EndsWith("creator") ).Value : postData.Author,
                    postData.Link,
                    postData.SpecificItem.Element.Elements().FirstOrDefault( node => node.Name == "comments" ).Value,
                    false
                );

                stackFeed.Children.Add( post );
            }
            catch ( Exception e )

            {
                RaiseStatus( e.Message );
                return;
            }
        }

        Title = "Juno @ " + website.name;
        RaiseStatus( "Ready!" );
    } 

    void OnComboOptionsSelectionChanged( object? sender, SelectionChangedEventArgs args )
    {
        LoadWebsite
        ( 
            WebsiteRegistry.list
                .FirstOrDefault( site => site.name == ((ComboBoxItem?)comboOptions.SelectedItem).Content ) 
        );
    }

    List<PostGrid> bookmarks = new List<PostGrid>();
    void UpdateBookmarksStack()
    {
        stackBookmarks.Children.Clear();
        foreach ( PostGrid post in bookmarks )
        {
            stackBookmarks.Children.Add( post );
        }
        
        if (bookmarks.Count == 0)
        {
            stackBookmarks.Children.Add( new TextBlock { Text = "Empty...", TextAlignment = TextAlignment.Center } );
            tabItemBookmarks.Header = "Bookmarks";
        }
        else
        {
            tabItemBookmarks.Header = $"Bookmarks ({bookmarks.Count})";
        }
    }

    void OnBookmarkButtonClearClick( object? sender, RoutedEventArgs a )
    {
        bookmarks.Clear();

        UpdateBookmarksStack();
    }
    
    async void OnBookmarkButtonExportClick( object? sender, RoutedEventArgs a )
    {
        string content = $"Juno Bookmarks - Exported @ {DateTime.Now.ToString("dd/MM/yyyy ss:mm:hh")}\n\n";
        foreach ( PostGrid post in bookmarks )
        {
            content += $"- {post.title}:\n{post.pageURI}\n\n";
        }
        
        var file = await StorageProvider.SaveFilePickerAsync( new FilePickerSaveOptions {  Title= "Save Bookmark Text File" , DefaultExtension = "txt", SuggestedFileName = "juno-bookmarks" } );
        if ( file is not null )
        {
            await using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);
            await streamWriter.WriteLineAsync(content);
            RaiseStatus( "List Exported." );
        }
        else
        {
            RaiseStatus( "Export Failed!" );
        }
    }

    public void OnPostGridAddToBookmarks( PostGrid originalPost )
    {
        bookmarks.Add( new PostGrid
        ( 
            this,
            originalPost.title,
            originalPost.author,
            originalPost.pageURI,
            originalPost.commentsURI,
            true
        ) );

        UpdateBookmarksStack();
    }

    public void OnPostGridRemoveBookmark( PostGrid bookmarkedPost )
    {
        bookmarks.Remove( bookmarkedPost );

        UpdateBookmarksStack();
    }

    public void OpenURI( string description, string uri )
    {
        var webviewWindow = new WebviewWindow( this, description, uri );
        webviewWindow.Show();
    }

    public void RaiseStatus( string text )
    {
        textStatus.Text = text;
    }

    public override void BeginInit()
    {
        comboOptions.SelectedIndex = 0;
        UpdateBookmarksStack();

        base.BeginInit();
    }
}