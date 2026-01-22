using Microsoft.EntityFrameworkCore;
using QuizWeb.Models;

namespace QuizWeb.Data;

public class QuizContext : DbContext
{
    // Konfiguracja bazy SQLite
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=QuizWeb.db");
    }

    // Tabela z Pytaniami
    public DbSet<Pytanie> Pytania { get; set; }
    // Tabela z Odpowiedziami
    public DbSet<Odpowiedz> Odpowiedzi { get; set; }
}
