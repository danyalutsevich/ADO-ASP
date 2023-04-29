namespace ASP.Services.Validation;

public interface IValidation
{
    bool Validate(string input, params ValidationTerms[] terms);
}