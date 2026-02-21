namespace StockApp.Application.ServiceContracts
{
    public interface IRequestValidator<T>
    {
        void Validate(T request);
    }
}
