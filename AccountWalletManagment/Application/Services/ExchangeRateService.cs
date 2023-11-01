using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        public ExchangeRateService() { }

        public Task<decimal> GetEurToGbpExchangeRate()
        {
            return Task.FromResult(0.87m);
        }
    }
}
