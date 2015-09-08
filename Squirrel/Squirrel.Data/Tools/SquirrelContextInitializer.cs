using System.Data.Entity;
using Squirrel.Domain.Enititis;
using Squirrel.Utility.Async;
using Squirrel.Utility.Cryptography;

namespace Squirrel.Data.Tools
{
    internal class SquirrelContextInitializer : CreateDatabaseIfNotExists<SquirrelContext>
    {
        protected override void Seed(SquirrelContext context)
        {
            var adminUser = new User
            {
                AccessFailed = 0,
                Email = "admin@admin.com",
                IsActive = true,
                IsAdmin = true,
                IsLock = false,
                PasswordHash = AsyncTools.ConvertToSync(() => SquirrelHashSystem.EncryptAsync("admin")),
                Username = "admin"
            };
            context.Users.Add(adminUser);
            base.Seed(context);
        }
    }
}
