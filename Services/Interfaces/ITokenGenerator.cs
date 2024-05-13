public interface ITokenGenerator
{
    public string GenerateUserToken(Guid userId);
}