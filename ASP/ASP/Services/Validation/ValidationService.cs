using System.Text.RegularExpressions;

namespace ASP.Services.Validation;

public class ValidationService : IValidation
{
    public bool Validate(string input, params ValidationTerms[] terms)
    {
        if (terms.Length == 0)
        {
            throw new ArgumentException("No validation terms provided");
        }

        if (terms.Length == 1 && terms[0] == ValidationTerms.None)
        {
            return true;
        }

        bool result = true;
        if (terms.Contains(ValidationTerms.NonEmpty))
        {
            result &= !string.IsNullOrEmpty(input);
        }

        if (terms.Contains(ValidationTerms.Username))
        {
            result &= this.ValidateUsername(input);
        }

        if (terms.Contains(ValidationTerms.Email))
        {
            result &= this.ValidateEmail(input);
        }

        if (terms.Contains(ValidationTerms.Password))
        {
            result &= this.ValidatePassword(input);
        }

        if (terms.Contains(ValidationTerms.PhoneNumber))
        {
            result &= this.ValidatePhoneNumber(input);
        }

        if (terms.Contains(ValidationTerms.Login))
        {
            result &= this.ValidateLogin(input);
        }

        return result;
    }

    private bool ValidateUsername(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
    }

    private bool ValidateEmail(string input)
    {
        return Regex.IsMatch(input, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
    }

    private bool ValidatePassword(string input)
    {
        return Regex.IsMatch(input, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");
    }

    private bool ValidatePhoneNumber(string input)
    {
        return Regex.IsMatch(input, @"^(\+?)([0-9]{2})?([0-9]{3})([0-9]{3})([0-9]{3})$");
    }

    private bool ValidateLogin(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
    }

    private bool ValidateNonEmpty(string input)
    {
        return !string.IsNullOrEmpty(input);
    }
}