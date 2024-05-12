namespace MiddleApi.Models;

public class UserData
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required Guid UserId { get; set; }
    public required User UserOwner { get; set; }
}