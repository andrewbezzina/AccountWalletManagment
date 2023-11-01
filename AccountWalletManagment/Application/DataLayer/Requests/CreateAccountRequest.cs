using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataLayer.Requests
{
    public record CreateAccountRequest (string IdCard, string FirstName, string LastName, DateOnly DOB, string Email, string Username, string Password);
}
