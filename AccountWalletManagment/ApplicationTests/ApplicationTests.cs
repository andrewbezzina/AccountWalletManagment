using Application.DataLayer;
using Application.DataLayer.Entities;
using Application.DataLayer.Requests;
using Application.Services;
using Application.Services.PasswordHasher;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System.Data.Common;


namespace PhoneBookTests
{
    [TestClass]
    public class ApplicationTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AccountDbContext> _contextOptions;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IExchangeRateService _exchangeRateService;

        // Sample Db data:
        private static readonly Guid Accout1Uuid = Guid.NewGuid();
        private static readonly Account account1 = new Account { Id = 1, Uuid = Accout1Uuid, IdCard = "0123187M", FirstName = "Andrew", LastName = "Bezzina", DOB = DateTime.Parse("12/02/1987"), Email = "andrewbezzina@gmail.com", Username = "andrewbezzina", PasswordHash = "10000.xxxx.xxxx", CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow };
        private static readonly Wallet wallet1 = new Wallet { Id = 1, AccountId = account1.Id, Balance = 0, CreatedDate = DateTime.UtcNow, Currency = "EUR", Uuid = Guid.NewGuid() };
     
        #region ConstructorAndDispose
        public ApplicationTests()
        {
            _passwordHasher = new PasswordHasher(Options.Create(new HashingOptions()));
            _exchangeRateService = new ExchangeRateService();

            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<AccountDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new AccountDbContext(_contextOptions);

            context.Database.EnsureCreated();

            context.AddRange(account1, wallet1);
            context.SaveChanges();


        }

        AccountDbContext CreateContext() => new AccountDbContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion

        [TestMethod]
        public async Task Account_Create_Valid()
        {
            using var context = CreateContext();

            CreateAccountRequest request = new("0123456M", "Francesca", "Borg", DateOnly.Parse("12/02/1987"), "francescaborg@gmail.com", "franc1", "password");

            var mockWalletService = new Mock<IWalletService>();
            mockWalletService.Setup(s => s.CreateWalletAsync(
                It.IsAny<int>(),
                It.Is<string>(s => s == "EUR"))).Returns(Task.FromResult(new Wallet()));

            var mockPasswordHasher = new Mock<IPasswordHasher>();
            mockPasswordHasher.Setup(s => s.Hash(
                It.Is<string>(s => s == "password"))).Returns("hashedpassword");

            var accountService = new AccountService(context, mockPasswordHasher.Object, mockWalletService.Object, null, _exchangeRateService);
            var accountResult = await accountService.CreateAccountAsync(request);
            
            Assert.IsNotNull(accountResult);
            Assert.AreEqual(request.FirstName, accountResult.FirstName);
            Assert.AreEqual(context.Accounts.Count(), 2);
            Assert.IsTrue(context.Accounts.Any(c => c.IdCard == request.IdCard));
            Assert.AreEqual(context.Accounts.FirstOrDefault(c => c.IdCard == request.IdCard).FirstName, request.FirstName);
            Assert.AreEqual(context.Accounts.FirstOrDefault(c => c.IdCard == request.IdCard).LastName, request.LastName);
            Assert.AreEqual(context.Accounts.FirstOrDefault(c => c.IdCard == request.IdCard).Email, request.Email);
            Assert.AreEqual(context.Accounts.FirstOrDefault(c => c.IdCard == request.IdCard).Username, request.Username);
            Assert.AreEqual(context.Accounts.FirstOrDefault(c => c.IdCard == request.IdCard).PasswordHash, "hashedpassword");

            mockWalletService.Verify(x => x.CreateWalletAsync(It.IsAny<int>(), "EUR"), Times.Once());
            mockPasswordHasher.VerifyAll();
        }
    }

}