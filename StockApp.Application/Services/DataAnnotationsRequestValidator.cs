using StockApp.Application.ServiceContracts;

namespace StockApp.Application.Services
{
    public class DataAnnotationsRequestValidator<T> : IRequestValidator<T>
    {
        public void Validate(T request)
        {
            ValidationHelper.ModelValidation(request!);
        }
    }
}
