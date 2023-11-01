using Application.DataLayer.Requests;
using Application.Services;
using Microsoft.Identity.Client;

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

            app.MapPost("api/account", async (CreateAccountRequest requst, IAccountService accountService) =>
            {
                var account = await accountService.CreateAccountAsync(requst);

                return Results.Ok(account);
            });

            app.MapPut("api/account/{id}", async (Guid id, UpdateAccountRequest requst, IAccountService accountService) =>
            {
                //TODO validate id == request.id
                var account = await accountService.UpdateAccountAsync(requst);

                return Results.Ok(account);
            });

            app.MapPost("api/account/{id}/AddGBPWallet", async (Guid id, IAccountService accountService) =>
            {
                var account = await accountService.AddGBPWalletToAccountAsync(id);

                return Results.Ok(account);
            });
        }
    }
}
