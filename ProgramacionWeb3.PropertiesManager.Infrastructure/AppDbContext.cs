using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Building> Buildings { get; set; }  
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseDistribution> ExpenseDistributions { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<Owner> Owners{ get; set; }
        public DbSet<Province> Provinces{ get; set; }
        public DbSet<Role> Roles{ get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitBill> UnitBills { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Building>()
                .HasOne(b => b.AdministratorUser)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Building)
                .WithMany()
                .HasForeignKey(e => e.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpenseDistribution>()
                .HasOne(ed => ed.Expense)
                .WithMany(e => e.ExpenseDistributions)
                .HasForeignKey(ed => ed.ExpenseId);

            modelBuilder.Entity<ExpenseDistribution>()
                .HasOne(ed => ed.Unit)
                .WithMany(u => u.ExpenseDistributions)
                .HasForeignKey(ed => ed.UnitId);

            modelBuilder.Entity<Owner>()
                .HasOne(o => o.User)
                .WithOne()
                .HasForeignKey<Owner>(o => o.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Owner)
                .WithMany(o => o.Units)
                .HasForeignKey(u => u.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Building)
                .WithMany(b => b.Units)
                .HasForeignKey(u => u.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            OnSeeding(modelBuilder);
        }

        private static void OnSeeding(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Properties Administrator" },
                new Role { Id = 3, Name = "Owner" }
            );

            modelBuilder.Entity<Province>().HasData(
                new Province { Id = 1, Name = "Ciudad Autónoma de Buenos Aires" },
                new Province { Id = 2, Name = "Buenos Aires" },
                new Province { Id = 3, Name = "Catamarca" },
                new Province { Id = 4, Name = "Chaco" },
                new Province { Id = 5, Name = "Chubut" },
                new Province { Id = 6, Name = "Córdoba" },
                new Province { Id = 7, Name = "Corrientes" },
                new Province { Id = 8, Name = "Entre Ríos" },
                new Province { Id = 9, Name = "Formosa" },
                new Province { Id = 10, Name = "Jujuy" },
                new Province { Id = 11, Name = "La Pampa" },
                new Province { Id = 12, Name = "La Rioja" },
                new Province { Id = 13, Name = "Mendoza" },
                new Province { Id = 14, Name = "Misiones" },
                new Province { Id = 15, Name = "Neuquén" },
                new Province { Id = 16, Name = "Río Negro" },
                new Province { Id = 17, Name = "Salta" },
                new Province { Id = 18, Name = "San Juan" },
                new Province { Id = 19, Name = "San Luis" },
                new Province { Id = 20, Name = "Santa Cruz" },
                new Province { Id = 21, Name = "Santa Fe" },
                new Province { Id = 22, Name = "Santiago del Estero" },
                new Province { Id = 23, Name = "Tierra del Fuego" },
                new Province { Id = 24, Name = "Tucumán" }
            );

            // This type of expenses ???
            modelBuilder.Entity<ExpenseType>().HasData(
                new ExpenseType { Id = 1, Name = "Ordinary" },
                new ExpenseType { Id = 2, Name = "Extraordinary" }
            );

            // Seed para la entidad User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@propertiesmanager.com",
                    Password = "123", // TODO: En el futuro acá irá la contraseña hacheada
                    IsActive = true,
                    CreationDate = new DateTime(2026, 1, 1), // Es mejor fijar una fecha en el seed para evitar migraciones cambiantes
                    RoleId = 2 // Acorde a tus roles, 2 es "Properties Administrator"
                }
            );
        }
    }
}
