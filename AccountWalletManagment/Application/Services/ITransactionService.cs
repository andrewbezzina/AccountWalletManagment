using Application.DataLayer.Entities;
using Application.DataLayer.Responses;

namespace Application.Services
{
    public interface ITransactionService
    {
        Task<WalletTransaction> AddWalletTransaction(decimal amount, int walletId, string reference);
        Task<IReadOnlyCollection<GetTransactionResponse>> GetTransactions(int walletId);
    }
}