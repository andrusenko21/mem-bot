using MemBotModels.DataModels;
using Microsoft.EntityFrameworkCore;

namespace MemBotDataAccess
{
    public class MemBotDbContext : DbContext
    {

        public MemBotDbContext() { }

        public MemBotDbContext(DbContextOptions<MemBotDbContext> options)
            :base(options)
        {
        }

        public DbSet<MemData> Memes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureTableNames(modelBuilder);
            ConfigurePrivateKeys(modelBuilder);
            ConfigureRelations(modelBuilder);
            ConfigureConstraints(modelBuilder);
        }

        private void ConfigureTableNames(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemData>().ToTable("Mem");
        }

        private void ConfigurePrivateKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemData>()
                .HasKey(m => m.Id);
        }

        private void ConfigureRelations(ModelBuilder modelBuilder)
        {

        }

        private void ConfigureConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemData>()
                .HasAlternateKey(m => m.Command);
        }
    }
}
