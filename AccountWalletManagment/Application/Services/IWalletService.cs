using Application.DataLayer.Entities;
using Application.DataLayer.Responses;

namespace Application.Services
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(int accountId, string currency);
        Task<IReadOnlyCollection<GetWalletResponse>> GetWalletsForAccountID(int id);
    }
}