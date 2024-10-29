using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task_1
{
    internal class DataRecord
    {
        public int Id { get; set; }  // Первичный ключ
        public DateTime Date { get; set; }  // Дата
        public string LatinChars { get; set; }  // Латинские символы
        public string RussianChars { get; set; }  // Русские символы
        public int EvenNumber { get; set; }  // Чётное число
        public double FractionalNumber { get; set; }  // Дробное число
    }

    internal class DataContext: DbContext
    {
        public DbSet<DataRecord> DataRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Task1;Trusted_Connection=True;"); // Указываем строку подключения к базе данных
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataRecord>().ToTable("MainTable"); // Настройка таблицы
            modelBuilder.Entity<DataRecord>().HasKey(d => d.Id);
        }
    }

    internal class DataBaseImport
    {
        public static void ImportData()
        {
            Console.WriteLine("Enter the path to file to import:");
            string filePath = Console.ReadLine();

            using (var context = new DataContext())
            {
                if (context.Database.CanConnect()) // Проверка подключения к базе данных
                {
                    Console.WriteLine("Connection successful!"); 
                }

                using (StreamReader reader = new StreamReader(filePath)) // Считываем данные из файла и записываем в базу данных
                {
                    string line;
                    int lineCount = 0;
                    int allLines = File.ReadAllLines(filePath).Length;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split("||");
                        parts = parts.Where((e, i) => i != parts.Length - 1).ToArray();

                        var record = new DataRecord // Создаём объект записи
                        {
                            Date = DateTime.Parse(parts[0]),
                            LatinChars = parts[1],
                            RussianChars = parts[2],
                            EvenNumber = int.Parse(parts[3]),
                            FractionalNumber = double.Parse(parts[4])
                        };

                        context.DataRecords.Add(record); // Добавляем запись в контекст

                        if (++lineCount % 100 == 0) // Сохраняем данные в базу каждые 100 строк для оптимизации
                        {
                            context.SaveChanges();
                            allLines -= 100;
                            Console.WriteLine($"Imported {lineCount} lines, {allLines} left...");
                        }
                    }
                    
                    context.SaveChanges();  // Сохраняем оставшиеся данные
                    Console.WriteLine("Data import completed.");
                    Console.WriteLine($"Total of {lineCount} lines imported.");
                }
            }
        }
    }
}