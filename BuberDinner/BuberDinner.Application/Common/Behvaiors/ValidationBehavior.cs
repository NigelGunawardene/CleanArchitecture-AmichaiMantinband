using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Services.Authentication.Common;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace BuberDinner.Application.Common.Behvaiors;
public class ValidateRegisterCommandBehavior : IPipelineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IValidator<RegisterCommand> _validator;

    public ValidateRegisterCommandBehavior(IValidator<RegisterCommand> validator)
    {
        _validator = validator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<ErrorOr<AuthenticationResult>> next)
    {
        // before the handler
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {
            // after the handler
            return await next(); // call handler if no errors
        }

        // each time you find yourself selecting and tolist, we can just use convertall
        //var errors = validationResult.Errors.Select(validationFailure => Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage)).ToList();
        var errors = validationResult.Errors.ConvertAll(validationFailure => Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage));

        return errors;
    }
}
