using Application.DataLayer;
using Application.DataLayer.Entities;
using Application.DataLayer.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AccountDbContext _context;
        private readonly IWalletService _walletService;
        private readonly DbSet<WalletTransaction> _transactions;

        public TransactionService(AccountDbContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
            _transactions = context.WalletTransactions;
        }

        public async Task<WalletTransaction> AddWalletTransaction(decimal amount, int walletId, string reference)
        {
            if (_transactions == null)
            {
                return null;
            }

            var wallet = await _walletService.GetWalletAsync(walletId);

            // check we have enough balance if amount is debit
            if (wallet.Balance + amount < 0)
            {
                throw new Exception("Not enough balance in account");
            }

            wallet.Balance += amount;

            WalletTransaction walletTransaction = new()
            {
                Uuid = Guid.NewGuid(),
                WalletId = walletId,
                Amount = amount,
                RemainingBalance = wallet.Balance,
                TransactionReference = reference,
                Created = DateTime.UtcNow
            };

            var addedTransaction = await _transactions.AddAsync(walletTransaction);

            await _context.SaveChangesAsync();

            return (addedTransaction).Entity;
        }

        public async Task<IReadOnlyCollection<GetTransactionResponse>> GetTransactions(int walletId)
        {
            if (_transactions == null)
            {
                return null;
            }

            var transactionList = await _transactions.Where(t => t.WalletId == walletId)
                                                     .Select(t => new GetTransactionResponse(t.Uuid, t.Amount, t.RemainingBalance, t.TransactionReference, t.Created))
                                                     .ToListAsync();

            return transactionList;
        }
    }
}
