namespace app.Services;

interface IHelloService
{
    string GetMessage();
    Task<string> GetUrlAsync(string url);
}

public class HelloService : IHelloService
{
    public string GetMessage() => $"Hello from {nameof(HelloService)} Service!";

    public async Task<string> GetUrlAsync(string url)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", $"C# App ({nameof(HelloService)})");
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}
