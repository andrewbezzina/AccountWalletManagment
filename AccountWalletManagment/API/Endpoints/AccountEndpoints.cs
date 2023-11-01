using Application.DataLayer.Requests;
using Application.Services;
using FluentValidation;
using Microsoft.Identity.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Endpoints
{
    public static class AccountEndpoints
    {
        public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/account/{id}", async (Guid id, IAccountService accountService) =>
            {
                var account = await accountService.GetAccountAsync(id);

                return Results.Ok(account);
            });

            // just adding this endpoint for convenience even though not requested in task.
            app.MapGet("api/account/all", async (IAccountService accountService) =>
            {
                var accounts = await accountService.GetAllAccountsAsync();

                return Results.Ok(accounts);
            });

            app.MapPost("api/account", async (CreateAccountRequest requst, IAccountService accountService, IValidator<CreateAccountRequest> validator) =>
            {
                var validationResult = validator.Validate(requst);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage));
                    return Results.BadRequest(errors);
                }

                var account = await accountService.CreateAccountAsync(requst);

                return Results.Ok(account);
            });

            app.MapPut("api/account/{id}", async (Guid id, UpdateAccountRequest requst, IAccountService accountService) =>
            {
                if ( id != requst.Uuid)
                {
                    return Results.BadRequest("Id in url must match uuid in body.");
                }
                var account = await accountService.UpdateAccountAsync(requst);

                return Results.Ok(account);
            });

            app.MapPost("api/account/{id}/add_gbp_wallet", async (Guid id, IAccountService accountService) =>
            {
                var account = await accountService.AddGBPWalletToAccountAsync(id);

                return Results.Ok(account);
            });

            app.MapPost("api/account/{id}/credit_10eur", async (Guid id, IAccountService accountService) =>
            {
                var account = await accountService.Credit10EurToAccountAsync(id);

                return Results.Ok(account);
            });

            app.MapPost("api/account/{id}/debit_gbp/{amount}", async (Guid id, decimal amount, IAccountService accountService) =>
            {
                var account = await accountService.DebitGbpFromAccountAsync(id, amount);

                return Results.Ok(account);
            });

            app.MapPost("api/account/{id}/convert_eur_to_gbp/{amount}", async (Guid id, decimal amount, IAccountService accountService) =>
            {
                var account = await accountService.ConvertEurToGbpAsync(id, amount);

                return Results.Ok(account);
            });

            app.MapGet("api/account/{id}/get_transactions/{currency}", async (Guid id, string currency, IAccountService accountService) =>
            {
                var account = await accountService.GetTransactionsForAccountAndCurrencyAsync(id, currency);

                return Results.Ok(account);
            });
        }
    }

    public record Error(string errorCode, string errorMessage);
}
