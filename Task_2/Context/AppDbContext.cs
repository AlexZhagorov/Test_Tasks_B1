using Microsoft.EntityFrameworkCore;
using Task_2.Models;
using System.Collections.Generic;

namespace Task_2.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<FileRecord> FileRecords { get; set; }      //коллекции для хранения сущностей связанных с БД
        public DbSet<AccountRecord> AccountRecords { get; set; }
        public DbSet<TotalAccountRecord> TotalAccountRecords { get; set; }
        public DbSet<ClassAccountRecord> ClassAccountRecords { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Task2;Trusted_Connection=True;"); // Указываем строку подключения к базе данных
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileRecord>().ToTable("Uploaded_Files");        //связь сущностей с таблицами БД
            modelBuilder.Entity<AccountRecord>().ToTable("Accounts");
            modelBuilder.Entity<TotalAccountRecord>().ToTable("Total_Accounts");
            modelBuilder.Entity<ClassAccountRecord>().ToTable("Class_Accounts");

            modelBuilder.Entity<FileRecord>().HasKey(e => e.FileId);
            modelBuilder.Entity<AccountRecord>().HasKey(e => e.Id);
            modelBuilder.Entity<TotalAccountRecord>().HasKey(e => e.Id);
            modelBuilder.Entity<ClassAccountRecord>().HasKey(e => e.Id);
        }
    }
}
