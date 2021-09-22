using System;
using System.Net.Http;
using System.Threading.Tasks;

try
{
    Console.WriteLine("#1 Before call");
    _ = DownloadAsync(null); // it's discarting the resulting Task, as a way of fire&forget;
                             // use of .Wait() or .Result here would be bad too
    Console.WriteLine("#1 After call");
}
catch (Exception e)
{
    Console.WriteLine($"#1: {e}"); // that would never be caught here
                                   // it can be silent ou kill the process
}

try
{
    Console.WriteLine("#2 Before call");
    await DownloadAsync(null); // always await it
                               // and always await inside try/catch blocks
    Console.WriteLine("#2 After call");
}
catch (Exception e)
{
    Console.WriteLine($"#2 Exception: {e}"); // now we catch it here
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static async Task<string> DownloadAsync(string url)
{
    if (url == null) throw new ArgumentException($"Argument \"{nameof(url)}\" is required.");

    var httpClient = new HttpClient();
    var content = await httpClient.GetStringAsync(url);

    Console.WriteLine($"[INFO] Downloaded: {content.Substring(0, 15)}...");

    return content;
}