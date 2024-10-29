﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task_2.Context;

#nullable disable

namespace Task_2.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241029022532_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Task_2.Models.AccountRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<double>("ActiveIn")
                        .HasColumnType("float");

                    b.Property<double>("ActiveOut")
                        .HasColumnType("float");

                    b.Property<double>("Credit")
                        .HasColumnType("float");

                    b.Property<double>("Debet")
                        .HasColumnType("float");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<double>("PassiveIn")
                        .HasColumnType("float");

                    b.Property<double>("PassiveOut")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("Task_2.Models.ClassAccountRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<double>("ActiveIn")
                        .HasColumnType("float");

                    b.Property<double>("ActiveOut")
                        .HasColumnType("float");

                    b.Property<double>("Credit")
                        .HasColumnType("float");

                    b.Property<double>("Debet")
                        .HasColumnType("float");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<double>("PassiveIn")
                        .HasColumnType("float");

                    b.Property<double>("PassiveOut")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Class_Accounts", (string)null);
                });

            modelBuilder.Entity("Task_2.Models.FileRecord", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FileId"));

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FileId");

                    b.ToTable("Uploaded_Files", (string)null);
                });

            modelBuilder.Entity("Task_2.Models.TotalAccountRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<double>("ActiveIn")
                        .HasColumnType("float");

                    b.Property<double>("ActiveOut")
                        .HasColumnType("float");

                    b.Property<double>("Credit")
                        .HasColumnType("float");

                    b.Property<double>("Debet")
                        .HasColumnType("float");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<double>("PassiveIn")
                        .HasColumnType("float");

                    b.Property<double>("PassiveOut")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Total_Accounts", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}