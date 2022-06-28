using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Authentication;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        //check if user already exists

        // create user (generate unique ID)

        // generate token
        Guid userId = Guid.NewGuid();
        var token = _jwtTokenGenerator.GenerateJwtToken(userId, firstName, lastName);

        return new AuthenticationResult(
            userId,
            firstName,
            lastName,
            email,
            token);
    }

    public AuthenticationResult Login(string email, string password)
    {
        return new AuthenticationResult(
            Guid.NewGuid(),
            "firstName",
            "lastName",
            email,
            "token");
    }
}

