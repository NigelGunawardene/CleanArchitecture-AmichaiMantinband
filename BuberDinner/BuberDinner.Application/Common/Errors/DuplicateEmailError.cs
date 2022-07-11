using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentResults;

namespace BuberDinner.Application.Common.Errors;
public class DuplicateEmailError : IError // using fluentResults IError and not my own // was record struct, changing to class
{
    public List<IError> Reasons => throw new NotImplementedException();

    public string Message => throw new NotImplementedException();

    public Dictionary<string, object> Metadata => throw new NotImplementedException();
}
