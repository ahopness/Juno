import os
import sys
import threading

import gi
gi.require_version("Gtk", "3.0")
from gi.repository import GLib, Gtk

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

        # loading indicators
        self.loading_text = builder.get_object("loading_text")
        self.status_bar = builder.get_object("status_bar")

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
        self.window = builder.get_object("window")
        self.add_window(self.window)
        self.window.present()

    def load_website(self, website):
        t = threading.Thread(target=self._load_website, args=[website])
        t.start()
    def _load_website(self, website):
        self.page_stack.set_visible_child_name("loading")
        self.status_bar.set_label(f"Loading {website.website_name} ...")
        self.window.set_title(f"Juno @ {website.website_name}")

        # make feed
        for child in self.feed.get_children(): self.feed.remove(child)

        self.loading_text.set_label("Loading RSS")
        self.feed_data = website.get_rss_feed()

        self.loading_text.set_label("Fetching data")
        for post_data in self.feed_data:
            post_builder = Gtk.Builder()
            post_builder.add_from_file(self.glade_file)

            post_object = post_builder.get_object("post")
            post_title = post_builder.get_object("post_title")
            post_link = post_builder.get_object("post_link")
            post_author = post_builder.get_object("post_author")
            
            post_title.set_label(post_data.title)
            post_link.set_label(post_data.link)
            post_author.set_label("by " + post_data.author)

            post_object.connect("clicked", self._on_post_clicked)

            self.feed.add(post_object)
        
        self.page_stack.set_visible_child_name("feed")
        self.status_bar.set_label("Ready!")
    
    def _on_post_clicked(self, button):
        print(f"clicked")

if __name__ == "__main__":
    app = Application()
    exit_status = app.run(sys.argv)
    sys.exit(exit_status)
