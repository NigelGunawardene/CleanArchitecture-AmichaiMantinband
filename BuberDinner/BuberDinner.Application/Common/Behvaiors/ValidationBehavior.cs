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
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> // IRequest comes from Mediator
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validator is null)
        {
            return await next();
        }
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {

            return await next(); // call handler if no errors
        }

        // each time you find yourself selecting and tolist, we can just use convertall
        //var errors = validationResult.Errors.Select(validationFailure => Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage)).ToList();
        var errors = validationResult.Errors.ConvertAll(validationFailure => Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage));

        return (dynamic)errors; // error here because the compiler doesnt know that there is an implicit converter from list of errors to erroror object. this dynamic is a workaround. at runtime, this will check if there is a way to convert list of errors to errorOr, otherwise it will throw a runtime exception. Even if there is an unforseen error, we still have our global error handler to catch it 

        // or you can use reflection like this - https://github.com/amantinband/error-or
        //response = (TResponse?)typeof(TResponse)
        //.GetMethod(
        //    name: nameof(ErrorOr<object>.From),
        //    bindingAttr: BindingFlags.Static | BindingFlags.Public,
        //    types: new[] { typeof(List<Error>) })?
        //.Invoke(null, new[] { errors })!;

        //return response is not null;

    }
}

// Old class, only registercommand
//public class ValidateRegisterCommandBehavior : IPipelineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>
//{
//    private readonly IValidator<RegisterCommand> _validator;

//    public ValidateRegisterCommandBehavior(IValidator<RegisterCommand> validator)
//    {
//        _validator = validator;
//    }

//    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<ErrorOr<AuthenticationResult>> next)
//    {
//        // before the handler
//        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
//        if (validationResult.IsValid)
//        {
//            // after the handler
//            return await next(); // call handler if no errors
//        }

//        // each time you find yourself selecting and tolist, we can just use convertall
//        //var errors = validationResult.Errors.Select(validationFailure => Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage)).ToList();
//        var errors = validationResult.Errors.ConvertAll(validationFailure => Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage));

//        return errors;
//    }
//}
