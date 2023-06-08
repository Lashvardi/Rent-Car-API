using Microsoft.EntityFrameworkCore;
using RentCar.models;
using RentCar.DTO;

namespace RentCar.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<UserFavoriteCar> UserFavoriteCars { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.PhoneNumber);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired();
            });

            modelBuilder.Entity<UserFavoriteCar>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.FavoriteCars)
                .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<UserFavoriteCar>()
                .HasOne(uf => uf.Car)
                .WithMany(c => c.FavoritedByUsers)
                .HasForeignKey(uf => uf.CarId);



            modelBuilder.Entity<Car>()
                .HasOne(c => c.Owner)
                .WithOne(u => u.Car)
                .HasForeignKey<Car>(c => c.OwnerPhoneNumber);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.PhoneNumber);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Car)
                .WithMany(c => c.Purchases)
                .HasForeignKey(p => p.CarId);




        }

}
}