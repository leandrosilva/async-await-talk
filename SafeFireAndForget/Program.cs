using System;
using System.Threading.Tasks;

try
{
    Console.WriteLine("#1 Before call");
    DownloadAsync(null).SafeFireAndForget();
    Console.WriteLine("#1 After call");

    Console.WriteLine("#2 Before call");
    DownloadAsync(null).SafeFireAndForget((e) => Console.WriteLine($"#2 Exception: {e.Message}"));
    Console.WriteLine("#2 After call");
}
catch (Exception e)
{
    Console.WriteLine($"Global exception: {e}"); // that would never happen
}

Console.WriteLine("Let's wait a bit...");
await Task.Delay(3000);

Console.WriteLine("See? Exception was safely caught");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static async Task<string> DownloadAsync(string url)
{
    Console.WriteLine(">> Starting to download...");
    await Task.Delay(1000); // this downdload thing is so hard 
    throw new Exception("Failed to download"); // oh no!
}

public static class TaskExtensions
{
    public static void SafeFireAndForget(this Task task, Action<Exception> onException = null)
    {
        if (!task.IsCompleted || task.IsFaulted)
        {
            _ = ForgetAwaited(task, onException); // discart the returnet task, forget it
        }

        async static Task ForgetAwaited(Task task, Action<Exception> onException)
        {
            try
            {
                await task.ConfigureAwait(false); // no need to back to the original synchronization context
            }
            catch (Exception e)
            {
                onException?.Invoke(e); // catch and notify the specific exception
            }
        }
    }
}