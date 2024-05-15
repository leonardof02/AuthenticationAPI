using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiddleApi.Models;
using MiddleApi.Exceptions;

namespace MiddleApi.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{

    private readonly ApplicationDbContext _appDbContext;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthenticationController(ApplicationDbContext appDbContext, ITokenGenerator tokenGenerator)
    {
        _appDbContext = appDbContext;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var existingUser = await _appDbContext.Users
            .FirstOrDefaultAsync(user => user.Email == registerRequest.Email);

        if (existingUser is not null)
            throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                "Email already in use"
            );

        var newUser = new User
        {
            Email = registerRequest.Email,
            HashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(registerRequest.Password),
            EmailConfirmationCode = new Random().Next(1000, 10000).ToString()
        };

        await _appDbContext.Users.AddAsync(newUser);
        await _appDbContext.SaveChangesAsync();

        var registerResponse = new RegisterResponse()
        {
            Email = newUser.Email,
            Message = $"We have sent an email to {newUser.Email}, go there and verify your account"
        };

        return Ok(registerResponse);
    }

    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Login(LoginRequest registerRequest)
    {
        var existingUser = await _appDbContext.Users
            .FirstOrDefaultAsync(user => user.Email == registerRequest.Email);

        if (existingUser is null)
            throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                "The email and password doesn't match"
            );

        if(existingUser.EmailConfirmed == false)
            throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                "User not verified by email"
            );

        if (!BCrypt.Net.BCrypt.EnhancedVerify(registerRequest.Password, existingUser.HashedPassword))
            throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                "The email and password doesn't match"
            );
        
        var loginResponse = new LoginResponse()
        {
            Email = existingUser.Email,
            Token = _tokenGenerator.GenerateUserToken(existingUser.Id)
        };

        return Ok(loginResponse);
    }
}