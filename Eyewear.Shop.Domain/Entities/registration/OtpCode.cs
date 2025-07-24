public class OtpCode
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DateTime Expiration { get; set; }
    public bool Used { get; set; } = false;
}
