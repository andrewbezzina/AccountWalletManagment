using Application.DataLayer;
using Application.DataLayer.Entities;
using Application.DataLayer.Requests;
using Application.DataLayer.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly AccountDbContext _context;
        private readonly DbSet<Wallet> _wallets;

        public WalletService(AccountDbContext context)
        {
            _context = context;
            _wallets = _context.Wallets;
        }

        public async Task<Wallet> CreateWalletAsync(int accountId, string currency)
        {
            if (_context.Wallets == null)
            {
                return null; ;
            }

            Wallet wallet = new()
            {
                Uuid = Guid.NewGuid(),
                AccountId = accountId,
                Currency = currency,
                Balance = 0,
                CreatedDate = DateTime.UtcNow,
            };

            var addedWallet = _context.Wallets.Add(wallet);

            await _context.SaveChangesAsync();

            return addedWallet.Entity;
        }

        public async Task<IReadOnlyCollection<GetWalletResponse>> GetWalletsResponseForAccountId(int id)
        {
            var wallets = await _wallets
                .Where(w  => w.AccountId == id)
                .Select(w => new GetWalletResponse(w.Currency, w.Balance))
                .ToListAsync();

            return wallets;
        }

        public Task<List<Wallet>> GetWalletsForAccountId(int id)
        {
             return _wallets.Where(w => w.AccountId == id).ToListAsync();
        }

        public Task<Wallet?> GetWalletForAccountIdandCurrency(int id, string currency)
        {
            return _wallets.FirstOrDefaultAsync(w => w.AccountId == id && w.Currency == currency);
        }

        public Task<Wallet?> GetWalletAsync(int walletId)
        {
            return _wallets.FirstOrDefaultAsync(w => w.Id == walletId);
        }
    }
}
