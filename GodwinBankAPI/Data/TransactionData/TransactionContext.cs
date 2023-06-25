using GodwinBankAPI.Data.AccountData;
using Microsoft.EntityFrameworkCore;

namespace GodwinBankAPI.Data.TransactionData;

public class TransactionContext:DbContext
{
    public TransactionContext(DbContextOptions<TransactionContext>options)
        :base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
}