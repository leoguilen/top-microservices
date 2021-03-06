﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TOP.ProfileService.Infra.Data.Context;

namespace TOP.ProfileService.Infra.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210310173159_initial_migration")]
    partial class initial_migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TOP.ProfileService.Domain.Entities.UserProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("AcademicLevel")
                        .HasColumnType("int");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("LastName")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("TOP.ProfileService.Domain.Entities.UserProfileDetails", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bio")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<byte[]>("ProfileImage")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserProfileDetails");
                });

            modelBuilder.Entity("TOP.ProfileService.Domain.Entities.UserProfileDetails", b =>
                {
                    b.HasOne("TOP.ProfileService.Domain.Entities.UserProfile", "UserProfile")
                        .WithOne("UserProfileDetails")
                        .HasForeignKey("TOP.ProfileService.Domain.Entities.UserProfileDetails", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("TOP.ProfileService.Domain.ValueObject.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("UserProfileDetailsUserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Cep")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("District")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserProfileDetailsUserId");

                            b1.ToTable("UserProfileDetails");

                            b1.WithOwner()
                                .HasForeignKey("UserProfileDetailsUserId");
                        });

                    b.Navigation("Address");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("TOP.ProfileService.Domain.Entities.UserProfile", b =>
                {
                    b.Navigation("UserProfileDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
