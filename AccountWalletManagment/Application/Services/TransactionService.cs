using Application.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TransactionService
    {
        private readonly AccountDbContext _accountDbContext;

        public TransactionService(AccountDbContext accountDbContext)
        {
            _accountDbContext = accountDbContext;
        }
    }
}
