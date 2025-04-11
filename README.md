<div align="center">
  <img align="center" src="https://github.com/user-attachments/assets/ecd79ed6-311e-45e7-abbb-a16822ec4f68"></img>
  <hr>
  <div align="left">
    <a>Link aggregation social media app (a.k.a. basic RSS client), made for research.</a>
    <br>
    <b>- THE PYTHON GTK3 BRANCH -</b>
    <br>
    <h2>Technicals</h2>
    <ul>
      <li>Install depedencies: <code>pip install pygobject feedparser</code>;</li>
      <li>RSS feeds loaded with <a href="https://pypi.org/project/feedparser/">feedparser</a>, really easy to work with, i can say that the python community really has mastered simplicity with their tools as described in <a href="https://peps.python.org/pep-0008/">PEP8</a>;</li>
      <li>UI built with <a href="https://glade.gnome.org/">Glade</a>, i could've coded the UI but i opted for a tool instead because i wanted to have fast iteration times. Glade can be a bit instable and have unexpected behaviour but it gets the job done anyways;</li>
      <li>The websites are loaded with <a href="https://webkitgtk.org/">WebkitGTK</a>, i could've used <a href="https://github.com/chromiumembedded/cef">CEF</a> instead but the easy integration with Glade and <a href="https://pygobject.gnome.org/">PyGObject</a> made me use Webkit instead. It takes a bit long to load pages but i belive this won't be a major issue with users.</li>
    </ul>
  </div>
</div>
