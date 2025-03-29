import os
import sys
import threading
import subprocess

import gi
gi.require_version("Gtk", "3.0")
gi.require_version("Gdk", "3.0")
gi.require_version('WebKit2', '4.1')
from gi.repository import GLib, Gdk, Gtk, WebKit2
from gi.repository.WebKit2 import WebView, Settings

from websites import * 

class Application(Gtk.Application):
    def __init__(self):
        super().__init__(application_id="com.ahopness.juno")
        GLib.set_application_name("juno")

        self.glade_file = os.path.dirname(os.path.abspath(__file__)) + '/ui.glade'
        self.window = None

    def do_activate(self):
        if self.window is None: self.create_window()
        self.load_website(current_website)
    
    def create_window(self):
        # loading layout
        builder = Gtk.Builder()
        builder.add_from_file(self.glade_file)

        # page stack
        self.page_stack = builder.get_object("page_stack")
        self.loading_text = builder.get_object("loading_text")

        # actions
        self.refesh_button = builder.get_object("refresh")
        self.refesh_button.connect("clicked", lambda refersh: self.load_website(current_website))
        builder.get_object("gamedevcity").connect("activate", lambda gdc: self.load_website(website_list[0]))
        builder.get_object("lobsters").connect("activate", lambda lob: self.load_website(website_list[1]))
        builder.get_object("hackernews").connect("activate", lambda hkr: self.load_website(website_list[2]))

        # feed
        self.feed = builder.get_object("feed")
        self.post = builder.get_object("post")

        # windowing
        self.window = builder.get_object("window_main")
        self.header_bar = builder.get_object("headerbar")
        self.add_window(self.window)
        self.window.present()
    
    def load_website(self, website):
        self.page_stack.set_visible_child_name("loading")
        self.header_bar.set_title("Juno")
        self.header_bar.set_subtitle(website.website_name)
        self.loading_text.set_label("Loading RSS")
        
        t = threading.Thread(target=self._fetch_website_data, args=[website])
        t.daemon = True
        t.start()

    def _fetch_website_data(self, website):
        feed_data = website.get_rss_feed()

        GLib.idle_add(self._update_ui_with_feed_data, website, feed_data)
    def _update_ui_with_feed_data(self, website, feed_data):
        self.loading_text.set_label("Updating feed")
        
        for child in self.feed.get_children(): self.feed.remove(child)
                    

        for post_data in feed_data:
            post_builder = Gtk.Builder()
            post_builder.add_from_file(self.glade_file)

            post_builder.get_object("post_title").set_label(post_data.title)
            post_builder.get_object("post_link").set_label(post_data.link)
            post_builder.get_object("post_author").set_label("by " + post_data.author)

            def button_clicked_callback(title, link):
                return lambda pst: self._on_post_clicked(title, link)
            post_builder.get_object("post_website").connect("clicked", button_clicked_callback(post_data.title, post_data.link))
            post_builder.get_object("post_comments").connect("clicked", button_clicked_callback(f"Comments of \'{post_data.title}\'", post_data.comments))

            self.feed.add(post_builder.get_object("post"))
        
        self.page_stack.set_visible_child_name("feed")
        
        # prevent this function from being called again
        return False
    
    def _on_post_clicked(self, title, link):
        webview_builder = Gtk.Builder()
        webview_builder.add_from_file(self.glade_file)
        
        webview_builder.get_object("webview_headerbar").set_title(title)
        webview_builder.get_object("webview_headerbar").set_subtitle(link)
        
        webview_builder.get_object("webview_copy").connect("clicked", lambda cp: Gtk.Clipboard.get(Gdk.SELECTION_CLIPBOARD).set_text(link, -1))
        webview_builder.get_object("webview_browser").connect("clicked", lambda br: self._open_link_in_browser(link))
        
        webview = webview_builder.get_object("webview")
        webview.load_uri(link)

        def _handle_loading(self, event):
            if event == WebKit2.LoadEvent.FINISHED:
                webview_builder.get_object("webview_spinner").stop()
        webview.connect("load-changed", _handle_loading)

        webview_window = webview_builder.get_object("window_webview")
        self.add_window(webview_window)
        webview_window.present()
    def _open_link_in_browser(self, link):
        if sys.platform == 'win32':
            os.startfile(link)  # Windows
        elif sys.platform == 'darwin':
            subprocess.run(['open', link])  # macOS
        else:
            subprocess.run(['xdg-open', link])  # Linux/Unix

if __name__ == "__main__":
    app = Application()
    exit_status = app.run(sys.argv)
    sys.exit(exit_status)
