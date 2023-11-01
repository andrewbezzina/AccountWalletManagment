using Application.DataLayer.Entities;
using Application.DataLayer.Requests;
using Application.DataLayer.Responses;

namespace Application.Services
{
    public interface IAccountService
    {
        Task<GetAccountResponse> AddGBPWalletToAccountAsync(Guid uuid);
        Task<GetAccountResponse> CreateAccountAsync(CreateAccountRequest createAccountRequest);
        Task<GetAccountResponse> GetAccountAsync(Guid uuid);
        Task<IReadOnlyCollection<Account>> GetAllAccountsAsync();
        Task<GetAccountResponse> UpdateAccountAsync(UpdateAccountRequest updateAccountRequest);
    }
}