﻿// <auto-generated />
using System;
using LearnApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LearnApi.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20230929122045_Third migration")]
    partial class Thirdmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("LearnApi.Entity.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Creditlimit")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<int?>("Taxcode")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("LearnApi.Entity.RefreshToken", b =>
                {
                    b.Property<int>("Tokenid")
                        .ValueGeneratedOnAdd()
                        .IsUnicode(false)
                        .HasColumnType("INTEGER")
                        .HasColumnName("tokenid");

                    b.Property<string>("Refreshtoken")
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("refreshtoken");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("TEXT")
                        .HasColumnName("ValidTo");

                    b.HasKey("Tokenid");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("LearnApi.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<bool?>("Isactive")
                        .HasColumnType("INTEGER")
                        .HasColumnName("isactive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LearnApi.Entity.RefreshToken", b =>
                {
                    b.HasOne("LearnApi.Entity.User", "User")
                        .WithOne("RefreshToken")
                        .HasForeignKey("LearnApi.Entity.RefreshToken", "Tokenid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnApi.Entity.User", b =>
                {
                    b.Navigation("RefreshToken")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
