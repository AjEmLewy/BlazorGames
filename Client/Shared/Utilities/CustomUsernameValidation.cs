using System.ComponentModel.DataAnnotations;

namespace Gejms.Client.Shared.Utilities;

public class CustomUsernameValidation : ValidationAttribute
{
    private readonly int max;
    private readonly int min;

    public CustomUsernameValidation(int min, int max)
    {
        this.max = max;
        this.min = min;
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;
        var textValue = value.ToString();
        if (textValue?.Length > max || textValue?.Length < min) return new ValidationResult("3 to 8 letters. ");
        var keyCodes = textValue.ToCharArray();
        foreach (var key in keyCodes)
            if (!(isBetween(key, 65, 90) || isBetween(key, 97, 122))) return new ValidationResult("Only letters.");

        return ValidationResult.Success;
    }

    private bool isBetween(int val, int min, int max)
    {
        return val >= min && val <= max;
    }
}
