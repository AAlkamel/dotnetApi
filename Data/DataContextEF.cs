using dotnetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetApi.Data
{
    public class DataContextEF: DbContext
  {
    private readonly IConfiguration _config;
    public DataContextEF(IConfiguration config)
    {
      _config = config;
    }

    public DbSet<User> Users { get; set;}
    public DbSet<UserJobInfo> UserJobInfos { get; set;}
    public DbSet<UserSalary> UserSalaries { get; set;}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"), sqlServerOptions =>
      {
        sqlServerOptions.EnableRetryOnFailure(
          maxRetryCount: 5,
          maxRetryDelay: TimeSpan.FromSeconds(10),
          errorNumbersToAdd: null);
      });
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("AppSchema");
      modelBuilder.Entity<User>().HasKey(u => u.UserId);
      modelBuilder.Entity<UserJobInfo>().ToTable("UserJobInfo").HasKey(u => u.JobId);
      modelBuilder.Entity<UserSalary>().ToTable("UserSalary").HasKey(u => u.SalaryId);
    }
  
}
}