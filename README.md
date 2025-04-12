<div align="center">
  <img align="center" src="https://github.com/user-attachments/assets/ecd79ed6-311e-45e7-abbb-a16822ec4f68"></img>
  <hr>
  <div align="left">
    <a>Link aggregation social media app (a.k.a. basic RSS client), made for research.</a>
    <br>
    <b>- THE C# GTK3 BRANCH -</b>
    <br>
    <h2>Technicals</h2>
    <ul>
      <li>I tried replicating the python-gtk3 version with dotnet tools thinking it would make a more stable and fast app and i ended up making the exact opossite, i think there's still place for improvement in the c# project but i think this project proves that python can be an excellent language for a fast & safe tool development;</li>
      <li><a href="https://github.com/GtkSharp/GtkSharp">GtkSharp</a> (not the mono one) works fine for the most part but i don't see myself writing more GUI apps with it in the future due to the bad portability and final executable size. It had some issues with importing the WebkitGtk webview tag from the Glade file so i decided to create the webview object at runtime instead, which i'm aware is bad practice.</li>
    </ul>
    <h2>Known Issues:</h2>
    <ul>
      <li>Gtk.Builder not loading the Glade file properly.</li>
    </ul>
  </div>
</div>
