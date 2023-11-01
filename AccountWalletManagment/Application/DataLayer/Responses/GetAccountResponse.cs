using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataLayer.Responses
{
    public record GetAccountResponse(Guid Id, string IdCard, string FirstName, string LastName, DateOnly DOB, string Email, string Username, IReadOnlyCollection<GetWalletResponse> Wallets);
}
