using Eyewear.Shop.Application.Interfaces.Services;

public class FakeSmsSender : ISmsService
{
    public async Task<Result> SendAsync(string phoneNumber, string message)
    {
        Console.WriteLine($"[Fake SMS] To: {phoneNumber} | Message: {message}");
        return Result.Success();
    }
}