using Application.DataLayer;
using Application.DataLayer.Entities;
using Application.DataLayer.Requests;
using Application.DataLayer.Responses;
using Application.Services.PasswordHasher;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services
{
    public class AccountService : IAccountService

    {
        private static readonly string EUR_CURRENCY_CODE = "EUR";
        private static readonly string GBP_CURRENCY_CODE = "GBP";
        private static readonly string DEFAULT_ACCOUNT_CURRENCY = EUR_CURRENCY_CODE;
        
        private readonly AccountDbContext _context;
        private readonly DbSet<Account> _accounts;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IWalletService _walletService;

        public AccountService(AccountDbContext context, IPasswordHasher passwordHasher, IWalletService walletService)
        {
            _context = context;
            _accounts = context.Accounts;
            _passwordHasher = passwordHasher;
            _walletService = walletService;
        }

        public async Task<GetAccountResponse> AddGBPWalletToAccountAsync(Guid uuid)
        {
            if (_context.Accounts == null)
            {
                return null;
            }

            var account = await _accounts.FirstOrDefaultAsync(x => x.Uuid == uuid);
            if (account == null)
            {
                return null;
            }

            var wallets = await _walletService.GetWalletsForAccountID(account.Id);
            if (wallets == null || wallets.Any(w => w.Currency == GBP_CURRENCY_CODE))
            {
                return null;
            }

            await _walletService.CreateWalletAsync(account.Id, GBP_CURRENCY_CODE);

            return await GetAccountAsync(uuid);
        }

        public async Task<GetAccountResponse> CreateAccountAsync(CreateAccountRequest createAccountRequest)
        {
            if (_context.Accounts == null)
            {
                return null; ;
            }

            Account account = new()
            {
                Uuid = Guid.NewGuid(),
                IdCard = createAccountRequest.IdCard,
                FirstName = createAccountRequest.FirstName,
                LastName = createAccountRequest.LastName,
                DOB = createAccountRequest.DOB.ToDateTime(TimeOnly.MinValue),
                Email = createAccountRequest.Email,
                Username = createAccountRequest.Username,
                PasswordHash = _passwordHasher.Hash(createAccountRequest.Password),
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow,  
            };

            var addedAccount = _accounts.Add(account);
            await _context.SaveChangesAsync();

            await _walletService.CreateWalletAsync(addedAccount.Entity.Id, DEFAULT_ACCOUNT_CURRENCY);

            return await GetAccountAsync(addedAccount.Entity.Uuid);
        }

        public async Task<GetAccountResponse> GetAccountAsync(Guid uuid)
        {
            if (_context.Accounts == null)
            {
                return null; 
            }

            var account = await _accounts.FirstOrDefaultAsync(x => x.Uuid == uuid);
            if (account == null)
            {
                return null;
            }

            var wallets = await _walletService.GetWalletsForAccountID(account.Id);

            return  new GetAccountResponse( account.Uuid,
                                            account.IdCard,
                                            account.FirstName,
                                            account.LastName,
                                            DateOnly.FromDateTime(account.DOB),
                                            account.Email,
                                            account.Username,
                                            wallets);
        }

        public async Task<IReadOnlyCollection<Account>> GetAllAccountsAsync()
        {
            if (_context.Accounts == null)
            {
                return null;
            }

            return await _accounts.ToListAsync();
        }

        public async Task<GetAccountResponse> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest)
        {
            if (_context.Accounts == null)
            {
                return null;
            }

            var account = await _accounts.FirstOrDefaultAsync(x => x.Uuid == updateAccountRequest.Uuid);
            if (account == null)
            {
                return null;
            }
            account.IdCard = updateAccountRequest.IdCard ?? account.IdCard;
            account.FirstName = updateAccountRequest?.FirstName ?? account.FirstName;
            account.LastName = updateAccountRequest?.LastName ?? account.LastName;
            account.DOB = updateAccountRequest.DOB.HasValue ? updateAccountRequest.DOB.Value.ToDateTime(TimeOnly.MinValue) : account.DOB;
            account.Email = updateAccountRequest?.Email ?? account.Email;
            account.Username = updateAccountRequest?.Username ?? account.Username;
            account.PasswordHash = updateAccountRequest.Password != null ? _passwordHasher.Hash(updateAccountRequest.Password) : account.PasswordHash;
            account.LastUpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetAccountAsync(account.Uuid);
        }
    }
}
