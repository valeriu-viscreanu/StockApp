using StockApp.ServiceContracts;

namespace StockApp.Services
{
    public class DataAnnotationsRequestValidator<T> : IRequestValidator<T>
    {
        public void Validate(T request)
        {
            ValidationHelper.ModelValidation(request!);
        }
    }
}
