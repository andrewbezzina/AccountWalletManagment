using Application.DataLayer.Entities;
using Application.DataLayer.Requests;
using Application.DataLayer.Responses;

namespace Application.Services
{
    public interface IAccountService
    {
        Task<GetAccountResponse> AddGBPWalletToAccountAsync(Guid uuid);
        Task<GetAccountResponse> ConvertEurToGbpAsync(Guid uuid, decimal amount);
        Task<GetAccountResponse> CreateAccountAsync(CreateAccountRequest createAccountRequest);
        Task<GetAccountResponse> Credit10EurToAccountAsync(Guid uuid);
        Task<GetAccountResponse> DebitGbpFromAccountAsync(Guid uuid, decimal amount);
        Task<GetAccountResponse> GetAccountAsync(Guid uuid);
        Task<IReadOnlyCollection<Account>> GetAllAccountsAsync();
        Task<IReadOnlyCollection<GetTransactionResponse>> GetTransactionsForAccountAndCurrencyAsync(Guid Uuid, string currency);
        Task<GetAccountResponse> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest);
    }
}