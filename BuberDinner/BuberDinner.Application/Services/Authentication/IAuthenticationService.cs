using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Errors;
using ErrorOr;
using FluentResults;
using OneOf;


// since this project does not reference the contracts project, we need to create a new model (AuthenticationResult) in order to use here. we dont want the definition of our api to leak into the application layer
namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    //AuthenticationResult Register(string firstName, string lastName, string email, string password);
    //AuthenticationResult Login(string email, string password);
    //OneOf<AuthenticationResult, IError> Register(string firstName, string lastName, string email, string password);
    //Result<AuthenticationResult> Register(string firstName, string lastName, string email, string password); // this is fluentresults. commenting cos we are gonna use ErrorOr
    ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password);
    ErrorOr<AuthenticationResult> Login(string email, string password);
}

