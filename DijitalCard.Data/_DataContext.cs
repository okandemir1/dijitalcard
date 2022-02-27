namespace DijitalCard.Data
{
    using Microsoft.EntityFrameworkCore;
    using DijitalCard.Data.Infrastructure;

    public class DataContext : DbContext
    {
        public DataContext(string connectionString) : base (new DbContextOptionsBuilder().UseMySQL(connectionString).Options)
        {
        }

        public DbSet<Model.Account> Accounts { get; set; }
        public DbSet<Model.AccountPlatform> AccountPlatforms { get; set; }
        public DbSet<Model.Platform> Platforms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Model.Account>(entity => entity.ToTable("digi_accounts"));
            builder.Entity<Model.AccountPlatform>(entity => entity.ToTable("digi_accountplatforms"));
            builder.Entity<Model.Platform>(entity => entity.ToTable("digi_platforms"));

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool))
                    {
                        property.SetValueConverter(new BoolToIntConverter());
                    }
                }
            }
        }
    }
}
