public class AppUser
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsProfileComplete => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Email);
}
