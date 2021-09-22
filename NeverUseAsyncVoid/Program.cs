using System;
using System.Threading.Tasks;

try
{
    try
    {
        Console.WriteLine("#1 Before call");
        TheVoidAsync(null); // there's no way to await an async void method
        Console.WriteLine("#1 After call");
    }
    catch (Exception e)
    {
        Console.WriteLine($"#1: {e}"); // that would never be caught here
    }

    try
    {
        Console.WriteLine("#2 Before call");
        await TheTaskAsync(null); // now you can await it
        Console.WriteLine("#2 After call");
    }
    catch (Exception e)
    {
        Console.WriteLine($"#2 Exception: {e}"); // but here we catch it
    }

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
catch (Exception e)
{
    Console.WriteLine($"Global try/catch: {e}"); // the exception happens in another context
}

static async void TheVoidAsync(string url)
{
    if (url == null) throw new ArgumentException($"Argument \"{nameof(url)}\" is required.");
    await Task.Delay(100);
}

static async Task TheTaskAsync(string url)
{
    if (url == null) throw new ArgumentException($"Argument \"{nameof(url)}\" is required.");
    await Task.Delay(100);
}