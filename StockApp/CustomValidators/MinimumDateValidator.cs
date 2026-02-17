using System.ComponentModel.DataAnnotations;

namespace StockApp.CustomValidators
{
    public class MinimumDateValidator : ValidationAttribute
    {
        public DateTime MinimumDate { get; }

        public MinimumDateValidator(string minimumDate)
        {
            MinimumDate = DateTime.Parse(minimumDate);
            ErrorMessage = $"Date should not be older than {MinimumDate:yyyy-MM-dd}.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime < MinimumDate)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
