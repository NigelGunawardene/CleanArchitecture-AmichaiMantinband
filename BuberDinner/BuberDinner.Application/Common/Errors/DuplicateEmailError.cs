using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace BuberDinner.Application.Common.Errors;
public record struct DuplicateEmailError : IError // using fluentResults IError and not my own // was record struct, changing to class
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public string ErrorMessage => "Email already exists - thrown from DuplicateEmailError, extending IError and library OneOf";
}
