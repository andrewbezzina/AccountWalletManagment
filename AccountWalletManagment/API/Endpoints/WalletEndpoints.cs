using Application.DataLayer.Requests;
using Application.Services;
using Microsoft.Identity.Client;

namespace API.Endpoints
{
    public static class WalletEndpoints
    {
        public static void MapWalletEndpoints(this IEndpointRouteBuilder app)
        {
            //app.MapGet("api/wallet/{id}", async (Guid id, IAccountService accountService) =>
            //{
            //    var account = await accountService.GetAccountAsync(id);

            //    return Results.Ok(account);
            //});

            //app.MapPost("api/account", async (CreateAccountRequest requst, IAccountService accountService) =>
            //{
            //    var account = await accountService.CreateAccountAsync(requst);

            //    return Results.Ok(account);
            //});

            //app.MapPut("api/account/{id}", async (Guid id, UpdateAccountRequest requst, IAccountService accountService) =>
            //{
            //    //TODO validate id == request.id
            //    var account = await accountService.UpdateAccountAsync(requst);

            //    return Results.Ok(account);
            //});
        }
    }
}
