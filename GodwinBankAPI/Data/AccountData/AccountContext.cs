using Microsoft.EntityFrameworkCore;

namespace GodwinBankAPI.Data.AccountData;

public class AccountContext:DbContext
{
    public AccountContext(DbContextOptions<AccountContext>options)
        :base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
}