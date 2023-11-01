namespace Application.Services
{
    public interface IExchangeRateService
    {
        Task<decimal> GetEurToGbpExchangeRate();
    }
}