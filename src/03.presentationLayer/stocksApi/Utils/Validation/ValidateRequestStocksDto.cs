using stocksApi.Models.Request;
using System.ComponentModel.DataAnnotations;

namespace stocksApi.Utils.Validation;

public class ValidateRequestStocksDto : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var stock = (RequestStocksDTO)validationContext.ObjectInstance;

        if (stock.OrderOrientation != null && stock.FieldToOrderCase == null)
        {
            return new ValidationResult("If field OrderOrientation is filled, please fill field FieldToOrderCase.");
        }

        return ValidationResult.Success;
    }
}
