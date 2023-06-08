﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RentCar.data;

#nullable disable

namespace RentCar.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Purchase", b =>
                {
                    b.Property<int>("PurchaseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<int>("Multiplier")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(255)");

                    b.HasKey("PurchaseID");

                    b.HasIndex("CarId");

                    b.HasIndex("PhoneNumber");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("RentCar.models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Brand")
                        .HasColumnType("longtext");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("CreatedByEmail")
                        .HasColumnType("longtext");

                    b.Property<int>("FuelCapacity")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl1")
                        .HasColumnType("longtext");

                    b.Property<string>("ImageUrl2")
                        .HasColumnType("longtext");

                    b.Property<string>("ImageUrl3")
                        .HasColumnType("longtext");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.Property<string>("Model")
                        .HasColumnType("longtext");

                    b.Property<int>("Multiplier")
                        .HasColumnType("int");

                    b.Property<string>("OwnerPhoneNumber")
                        .HasColumnType("varchar(255)");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Transmission")
                        .HasColumnType("longtext");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerPhoneNumber")
                        .IsUnique();

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("RentCar.models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("MessageText")
                        .HasColumnType("longtext");

                    b.Property<string>("UserPhoneNumber")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserPhoneNumber");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("RentCar.models.User", b =>
                {
                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.HasKey("PhoneNumber");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserFavoriteCar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFavoriteCars");
                });

            modelBuilder.Entity("Purchase", b =>
                {
                    b.HasOne("RentCar.models.Car", "Car")
                        .WithMany("Purchases")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RentCar.models.User", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("PhoneNumber");

                    b.Navigation("Car");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RentCar.models.Car", b =>
                {
                    b.HasOne("RentCar.models.User", "Owner")
                        .WithOne("Car")
                        .HasForeignKey("RentCar.models.Car", "OwnerPhoneNumber");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("RentCar.models.Message", b =>
                {
                    b.HasOne("RentCar.models.User", null)
                        .WithMany("Message")
                        .HasForeignKey("UserPhoneNumber");
                });

            modelBuilder.Entity("UserFavoriteCar", b =>
                {
                    b.HasOne("RentCar.models.Car", "Car")
                        .WithMany("FavoritedByUsers")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RentCar.models.User", "User")
                        .WithMany("FavoriteCars")
                        .HasForeignKey("UserId");

                    b.Navigation("Car");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RentCar.models.Car", b =>
                {
                    b.Navigation("FavoritedByUsers");

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("RentCar.models.User", b =>
                {
                    b.Navigation("Car");

                    b.Navigation("FavoriteCars");

                    b.Navigation("Message");

                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
