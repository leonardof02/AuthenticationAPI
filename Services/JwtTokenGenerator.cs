public class JwtTokenGenerator : ITokenGenerator
{
    public JwtTokenGenerator() {}

    public string GenerateUserToken(Guid userId)
    {
        return "Token";
    }
}