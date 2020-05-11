using Microsoft.EntityFrameworkCore;
using IdentityMapKlyuchnikovaPolina;

namespace AppContext
{
    class ApplicationContext : DbContext
    {
        public DbSet<Student> students { get; set; }
        public DbSet<Rates> Rates { get; set; }


        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Students;Trusted_Connection=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("NumberGradeBook").StartsAt(100000)
                            .IncrementsBy(1);

            modelBuilder.Entity<Student>()
                    .Property(o => o.NumberGradeBook)
                    .HasDefaultValueSql("NEXT VALUE FOR NumberGradeBook");

            modelBuilder.HasSequence<int>("Id");

            modelBuilder.Entity<Rates>()
                       .HasOne(p => p.Student)
                       .WithMany(t => t.Rates)
                       .HasForeignKey(p => p.StudentNumberGradeBook)
                       .HasPrincipalKey(t => t.NumberGradeBook);
            modelBuilder.Entity<Rates>()
                       .Property(o => o.Id)
                       .HasDefaultValueSql("NEXT VALUE FOR Id");
        }

    }
}
