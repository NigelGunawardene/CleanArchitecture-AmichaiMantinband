using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using FluentResults;
using OneOf;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    //public OneOf<AuthenticationResult, IError> Register(string firstName, string lastName, string email, string password) // oneof library
    //public Result<AuthenticationResult> Register(string firstName, string lastName, string email, string password) // fluentresults library
    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password) // ErrorOr
    {
        // Validate that the user doesnt exist
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            //return new AuthenticationResult(false, "User already exists");
            //throw new DuplicateEmailException(); //Exception("User with given email already exists - normal exception")
            //return new DuplicateEmailError(); // when using OneOf, we could return it like this. Now using FluentResults
            //return Result.Fail<AuthenticationResult>(new[] { new DuplicateEmailError() });  // fluentresults library
            return Errors.User.DuplicateEmail;
        }

        // create user (generate unique ID) and persist to db
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        _userRepository.Add(user);

        // generate token
        var token = _jwtTokenGenerator.GenerateJwtToken(user);

        return new AuthenticationResult(
            user,
            token);
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {

        // In a single method, we can return either the desired object, an error or a list of errors
        // Validate that user exists
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            //throw new Exception("User does not exist");
            return Errors.Authentication.InvalidCredentials;
        }

        // Validate password is correct
        if (user.Password != password)
        {
            return new[] { Errors.Authentication.InvalidCredentials };
        }


        // Create jwt and return to user

        var token = _jwtTokenGenerator.GenerateJwtToken(user);
        return new AuthenticationResult(
            user,
            token);
    }
}

