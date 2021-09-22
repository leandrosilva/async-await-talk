using System;
using System.Net.Http;
using System.Threading.Tasks;

Console.WriteLine("#1 Before call");
var content = await KnowWhatToDownloadAsync();
Console.WriteLine("#1 After call");

Console.WriteLine($"#1 Content size: {content.Length}");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static Task<string> KnowWhatToDownloadAsync()
{
    var url = "https://leandrosilva.com.br"; // whatever evil logic here

    return DownloadAsync(url); // no need to await here, let the client code to do so
}

static async Task<string> DownloadAsync(string url)
{
    if (url == null) throw new ArgumentException($"Argument \"{nameof(url)}\" is required.");

    var httpClient = new HttpClient();
    var content = await httpClient.GetStringAsync(url);

    Console.WriteLine($"[INFO] Downloaded: {content.Substring(0, 15)}...");

    return content;
}