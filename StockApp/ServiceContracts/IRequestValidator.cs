namespace StockApp.ServiceContracts
{
    public interface IRequestValidator<T>
    {
        void Validate(T request);
    }
}
