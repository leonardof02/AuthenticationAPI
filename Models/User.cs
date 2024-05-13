namespace MiddleApi.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string HashedPassword { get; set; }
    public UserData? UserData { get; set; }
    public IEnumerable<Article> Articles { get; set; } = [];
}