using Contracts.Models;
using Contracts.RiskEngine.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Postgres
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
        : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RiskAssessmentResult> RiskAssessmentResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RiskAssessmentResult>()
                .HasKey(x => x.TransactionId);

            modelBuilder.Entity<RiskAssessmentResult>()
                .Property(x => x.Reasons)
                .HasColumnType("jsonb");
        }
    }
}
