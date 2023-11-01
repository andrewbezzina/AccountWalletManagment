using AcmePayLtdAPI.Validators;
using API.Endpoints;
using Application.DataLayer;
using Application.DataLayer.Requests;
using Application.Services;
using Application.Services.PasswordHasher;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.json");
            builder.Services.AddDbContext<AccountDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AccountSqlDatabase"));
                options.EnableDetailedErrors();
            });

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IWalletService, WalletService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
            builder.Services.AddScoped<IValidator<CreateAccountRequest>, CreateAccountRequestValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapAccountEndpoints();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}