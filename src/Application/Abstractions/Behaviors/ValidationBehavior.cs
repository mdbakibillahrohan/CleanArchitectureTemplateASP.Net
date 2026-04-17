using Domain.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new Error(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage,
                ErrorType.Validation))
            .ToList();

        if (validationErrors.Any())
        {
            // This is a bit tricky with generic TResponse. 
            // We need to create a failure Result of the correct type.
            return CreateValidationResult<TResponse>(validationErrors);
        }

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(List<Error> errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;

        return (TResult)validationResult;
    }
}

// Helper classes to support multiple validation errors in a Result
public class ValidationResult : Result, IValidationResult
{
    private ValidationResult(List<Error> errors)
        : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public List<Error> Errors { get; }

    public static ValidationResult WithErrors(List<Error> errors) => new(errors);
}

public class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(List<Error> errors)
        : base(default, false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public List<Error> Errors { get; }

    public static ValidationResult<TValue> WithErrors(List<Error> errors) => new(errors);
}

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation error occurred.",
        ErrorType.Validation);

    List<Error> Errors { get; }
}
