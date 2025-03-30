<div align="center">
  <img align="center" src="https://github.com/user-attachments/assets/ecd79ed6-311e-45e7-abbb-a16822ec4f68"></img>
  <hr>
  <div align="left">
    <a>Link aggregation social media app (a.k.a. basic RSS client), made for research.</a>
    <br>
    <b>- THE GTK4 BRANCH -</b>
    <br>
    <h2>Technicals</h2>
    <ul>
      <li>The UI this time was built with <a href="https://gitlab.gnome.org/jpu/cambalache">Cambalache</a>, which felt like a toned down version of Glade, the UX isn't that great and i had to manually port all the Widgets one by one, if this was a large project that would defnetly be a hastle;</li>
      <li>I tried to use libadw to this project but some widgets were desmostrating wierd behaviour so i just gave up + aparently i had to update the gtk packages in my OS to use gtk-4.16 but i couldn't figure out how to do it;</li>
      <li>I also had some issues but Webkit versioning, <a href="https://webkitgtk.org/reference/webkit2gtk/2.39.7/migrating-to-webkitgtk-6.0.html">version 6.0 isn't installed by default on most systems</a>, which makes the portability of the app less efficient;</li>
      <li>My final veredict of this failed atempt is that i'll think twice before programming any app with GTK4 and ADW, since GTK3 support is still wide-spread and dealing with the dated API calls aren't that stressfull at all.</li>
    </ul>
  </div>
</div>
