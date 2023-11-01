using Application.DataLayer.Entities;
using Application.DataLayer.Responses;

namespace Application.Services
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(int accountId, string currency);
        Task<Wallet?> GetWalletAsync(int walletId);
        Task<Wallet?> GetWalletForAccountIdandCurrency(int id, string currency);
        Task<List<Wallet>> GetWalletsForAccountId(int id);
        Task<IReadOnlyCollection<GetWalletResponse>> GetWalletsResponseForAccountId(int id);
    }
}