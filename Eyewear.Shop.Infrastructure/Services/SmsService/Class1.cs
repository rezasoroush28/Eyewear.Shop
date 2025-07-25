using Eyewear.Shop.Application.Interfaces.Services;

public class FakeSmsSender : ISmsService
{
    public Task SendAsync(string phoneNumber, string message)
    {
        Console.WriteLine($"[Fake SMS] To: {phoneNumber} | Message: {message}");
        return Task.CompletedTask;
    }
}