using Microsoft.EntityFrameworkCore;

namespace CovidSystem.DbContexts
{
    public class CovidDbContext: DbContext
    {
        public CovidDbContext(DbContextOptions<CovidDbContext> options) : base(options)
        {
        }

        // Define DbSet properties for your entities
        public DbSet<Member> Members { get; set; }
    }
}
