<div align="center">
  <img align="center" src="https://github.com/user-attachments/assets/ecd79ed6-311e-45e7-abbb-a16822ec4f68"></img>
  <hr>
  <div align="left">
    <a>Link aggregation social media app (a.k.a. basic RSS client), made for research.</a>
    <br>
    <b>- THE C# AVALONIA UI BRANCH -</b>
    <br>
    <h2>Technicals</h2>
    <ul>
      <li>This branch differs from the previous ones by a large amount, it made me go outside my confort zone and write the GUI in a <b>imperative style</b> instead of relying on UI Builders apps to to that for me. Its still pretty different than writing the UI in a declarative style, <a href="https://docs.flutter.dev/get-started/flutter-for/declarative">like Flutter does</a>, but it's still close enough, let's not rush things;</li>
      <li>This time around i choose AvaloniaUI instead of GTK, it's a UI framework best fit as a modern replacement for WPF, with XAML files in mind for building the UI, but i decided to build it in code anyway, shout out to <a href="https://github.com/stevensrmiller/AvaloniaWithoutXAML">Stevens R. Miller and his samples repository</a>, there are barely any tutorials on Avalonia's documentation for building the UI in code, his samples guided me through the new framework;</li>
      <li>For the webviews i didn't had WebKit this time around, so i had to choose another webview framework for the task, and i decided to go for the next obvious answer, that is <a href="https://github.com/chromiumembedded/cef">CEF (a.k.a. Chromium Embedded Framework)</a>, more specifically it's dotnet wrapper <a href="https://github.com/OutSystems/WebView">WebView-Avalonia</a>, which itself is build on top of <a href="https://github.com/OutSystems/CefGlue">CefGlue</a>. I'm not really a fan of the Chromium project due to it's sheer bloat, but it got the job done anyway so i won't complain much.</li>
    </ul>
    <h2>Known Issues</h2>
    <ul>
      <li>I used the <a href="https://github.com/Splitwirez/avalonia-aero-theme">Avalonia Aero Theme</a> to style my UI, and it is not available inside NuGet, so if you want to build the project, you'll need to download the repo and paste the folder <b>besides</b> the Juno repo folder.</li>
    </ul>
  </div>
</div>
