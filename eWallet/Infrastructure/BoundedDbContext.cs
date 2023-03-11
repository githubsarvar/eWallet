using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure;

public class BoundedDbContext<TContext> : DbContext where TContext : DbContext
{
    public BoundedDbContext() { }
    public BoundedDbContext(DbContextOptions<TContext> options) : base(options)
    {
        Database.Migrate();
    }
}

