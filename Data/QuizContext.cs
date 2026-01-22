using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

public class QuizContext : DbContext
{
    public DbSet<Quiz> Quizy { get; set; }
    public DbSet<Pytanie> Pytania { get; set; }
    public DbSet<Odpowiedz> Odpowiedzi { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Path.Combine zadba, żeby na Twoim Macu ścieżka była poprawna
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FlagsQuiz.db");
        options.UseSqlite($"Data Source={dbPath}");
    }
}
