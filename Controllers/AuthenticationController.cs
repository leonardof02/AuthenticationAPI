using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiddleApi.Models;
using MiddleApi.Exceptions;
using MiddleApi.DTOs;
using MiddleApi.Services.Models;

namespace MiddleApi.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{

    private readonly ApplicationDbContext _appDbContext;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IMailService _mailService;

    public AuthenticationController(
        ApplicationDbContext appDbContext,
        ITokenGenerator tokenGenerator,
        IMailService mailService
    )
    {
        _appDbContext = appDbContext;
        _tokenGenerator = tokenGenerator;
        _mailService = mailService;
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

        var baseUrl = Request.Scheme + "://" + Request.Host;

        var verificationMail = new MailData
        {
            To = newUser.Email,
            Subject = "Email verification account",
            Body = $"Confirm your email with this link: {baseUrl}/emailconfirmation?userId={newUser.Id}&code={newUser.EmailConfirmationCode}"
        };

        await _mailService.SendMailAsync(mailData: verificationMail);

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

        if (existingUser.EmailConfirmed == false)
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

    [HttpGet]
    [Route("/emailconfirmation")]
    public async Task<IActionResult> EmailConfirmation(Guid userId, string code)
    {
        var existingUser = await _appDbContext.Users.FindAsync(userId);
        var userConfirmationCode = existingUser?.EmailConfirmationCode;

        if (existingUser is null || userConfirmationCode != code)
            throw new HttpResponseException(
                StatusCodes.Status404NotFound,
                "User doesn't exists"
            );

        existingUser.EmailConfirmed = true;
        await _appDbContext.SaveChangesAsync();

        var loginResponse = new LoginResponse()
        {
            Email = existingUser.Email,
            Token = _tokenGenerator.GenerateUserToken(existingUser.Id)
        };

        return Ok(loginResponse);
    }
}