using Gtk;
using WebKit;
using System.ServiceModel.Syndication;

using System;
using System.Xml;
using System.Threading.Tasks;

namespace Juno
{
    public class Website{
        public string name;
        public string RSSAdress;
        public Website(string _name, string customRSSAdress = "")
        {
            name = _name;
            
            if (customRSSAdress == "")
                RSSAdress = "https://" + name + "/rss";
            else
                RSSAdress = customRSSAdress;
        }

        public async Task<SyndicationFeed> GetRSSFeed()
        {
            var client = new System.Net.Http.HttpClient();
            string xml = await client.GetStringAsync(RSSAdress);
            var reader = XmlReader.Create(new System.IO.StringReader(xml));
            return await Task.Run(() => SyndicationFeed.Load(reader));
        }
    }

    public class Program
    {
        public static Website[] websiteList = {new Website("gamedev.city"),new Website("lobste.rs"), new Website("hnrss.org", @"https://hnrss.org/frontpage")};
        public static Website currentWebsite = websiteList[0];

        public static Gtk.Application app = new Gtk.Application("org.Ahopness.Juno", GLib.ApplicationFlags.None);
        public static Gtk.Builder builderMain = new Gtk.Builder();

        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            app.Register(GLib.Cancellable.Current);

            builderMain.AddFromFile("ui.glade");

            ((Gtk.Button)builderMain.GetObject("refresh")).Clicked += (object sender, EventArgs a) => { LoadWebsite(currentWebsite); };

            ((Gtk.MenuItem)builderMain.GetObject("gamedevcity")).Activated += (object sender, EventArgs a) => { LoadWebsite(websiteList[0]); };
            ((Gtk.MenuItem)builderMain.GetObject("lobsters")).Activated += (object sender, EventArgs a) => { LoadWebsite(websiteList[1]); };
            ((Gtk.MenuItem)builderMain.GetObject("hackernews")).Activated += (object sender, EventArgs a) => { LoadWebsite(websiteList[2]); };

            ((Gtk.ApplicationWindow)builderMain.GetObject("window_main")).DeleteEvent += (object obj, DeleteEventArgs args) => { Gtk.Application.Quit(); args.RetVal = true; };
            app.AddWindow((Gtk.ApplicationWindow)builderMain.GetObject("window_main"));
            ((Gtk.ApplicationWindow)builderMain.GetObject("window_main")).Present();

            LoadWebsite(currentWebsite);

            Gtk.Application.Run();
        }

        public static async void LoadWebsite(Website website)
        {

            Application.Invoke( (object sender, EventArgs e) => 
            {
                ((Gtk.Stack)builderMain.GetObject("page_stack")).VisibleChildName = "loading";
                ((Gtk.Label)builderMain.GetObject("loading_text")).Text = "Loading RSS";

                ((Gtk.HeaderBar)builderMain.GetObject("headerbar")).Title = "Juno";
                ((Gtk.HeaderBar)builderMain.GetObject("headerbar")).Subtitle = website.name;
            });

            try
            {
                SyndicationFeed feed = await website.GetRSSFeed();
                _UpdateFeed(website, feed);
            }
            catch (System.Exception e)
            {
                ((Gtk.Label)builderMain.GetObject("loading_text")).Text = "Failed to load feed\n(" + e.Message +")";
            }
            
        }

        static void _UpdateFeed(Website website, SyndicationFeed feed)
        {
            Application.Invoke( (object sender, EventArgs e) => 
            {
                ((Gtk.Label)builderMain.GetObject("loading_text")).Text = "Updating feed";

                foreach ( Gtk.FlowBoxChild post in ((Gtk.FlowBox)builderMain.GetObject("feed")).Children )
                    ((Gtk.FlowBox)builderMain.GetObject("feed")).Remove(post);
            } );

            foreach (SyndicationItem postData in feed.Items)
            {
                var builderPost = new Gtk.Builder();
                builderPost.AddFromFile("ui.glade");

                ((Gtk.Label)builderPost.GetObject("post_title")).Text = postData.Title.Text;
                ((Gtk.Label)builderPost.GetObject("post_link")).Text = postData.Links[0].Uri.ToString();
                ((Gtk.Label)builderPost.GetObject("post_author")).Text = "by " + postData.Authors[0].Name;

                ((Gtk.Button)builderPost.GetObject("post_website")).Clicked += (object sender, EventArgs a) => { _OpenURL( postData.Title.Text, postData.Links[0].Uri.ToString() ); };
                // TOOD: Encontrar aonde q ta o link desas joça
                //((Gtk.Button)builderPost.GetObject("post_comments")).Clicked += (object sender, EventArgs a) => { _OpenURL( ( "Comments of " + postData.Title.Text ), postData.Links[0].Uri.ToString() ); };
            
                Application.Invoke( (object sender, EventArgs e) => { ((Gtk.FlowBox)builderMain.GetObject("feed")).Add((Gtk.Box)builderPost.GetObject("post")); } );
            }

            Application.Invoke( (object sender, EventArgs e) => { ((Gtk.Stack)builderMain.GetObject("page_stack")).VisibleChildName = "feed"; } );
        }

        static void _OpenURL(string windowTitle, string URL)
        {
            var builderWebview = new Gtk.Builder();
            builderWebview.AddFromFile("ui.glade");

            ((Gtk.HeaderBar)builderWebview.GetObject("webview_headerbar")).Title = windowTitle;
            ((Gtk.HeaderBar)builderWebview.GetObject("webview_headerbar")).Subtitle = URL;

            ((Gtk.Button)builderWebview.GetObject("webview_copy")).Clicked += (object sender, EventArgs a) => { Gtk.Clipboard.Get(Gdk.Selection.Clipboard).Text = URL; };
            ((Gtk.Button)builderWebview.GetObject("webview_browser")).Clicked += (object sender, EventArgs a) => { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(URL) { UseShellExecute = true }); };

            var webview = new WebKit.WebView();
            webview.LoadUri(URL);
            webview.ShowAll();
            webview.LoadChanged += (object sender, LoadChangedArgs args) => 
            {
                if ( args.LoadEvent == LoadEvent.Finished )
                    ((Gtk.Spinner)builderWebview.GetObject("webview_spinner")).Stop();
            };

            ((Gtk.ApplicationWindow)builderWebview.GetObject("window_webview")).Add(webview);
            app.AddWindow((Gtk.ApplicationWindow)builderWebview.GetObject("window_webview"));
            ((Gtk.ApplicationWindow)builderWebview.GetObject("window_webview")).Present();
        }
    }
}
