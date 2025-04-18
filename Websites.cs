using System.Xml;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;

namespace Juno;

public class Website
{
    public string name;
    public string RSSAddress;
    
    public Website(string _name, string customRSSAddress = "")
    {
        name = _name;
        
        if (customRSSAddress == "")
            RSSAddress = "https://" + name + "/rss";
        else
            RSSAddress = customRSSAddress;
    }
}

public class WebsiteRegistry
{
    public static Website[] list = 
    { 
        new Website( "gamedev.city" ), 
        new Website( "lobste.rs" ), 
        new Website( "hacker-news", @"https://hnrss.org/frontpage" ) 
    };

    public Website currentWebsite = list[0];
}