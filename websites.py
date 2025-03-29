import feedparser

class Website():
    def __init__(self, website_name, custom_rss_adress = ""):
        self.website_name = website_name
        if custom_rss_adress == "":
            self.rss_adress = "https://" + website_name + "/rss"
        else:
            self.rss_adress = custom_rss_adress
    
    def get_rss_feed(self):
        self.feed = feedparser.parse(self.rss_adress)
        return self.feed.entries

website_list = [Website("gamedev.city"), Website("lobste.rs"), Website("hnrss.org", "https://hnrss.org/frontpage")]

current_website = website_list[0]