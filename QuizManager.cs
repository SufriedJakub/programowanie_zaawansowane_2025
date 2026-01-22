using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Niezbędne dla LINQ
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class QuizManager
{
    private readonly QuizContext _context;

    public QuizManager()
    {
        _context = new QuizContext();
        _context.Database.EnsureCreated();
    }

    // --- WYMÓG: LINQ (Filtrowanie i wyszukiwanie) ---
    public async Task<List<Pytanie>> SzukajPytanAsync(string fraza, string kategoria = "")
    {
        var query = _context.Pytania.Include(p => p.Odpowiedzi).AsQueryable();

        if (!string.IsNullOrWhiteSpace(fraza))
        {
            // LINQ: Filtrowanie po treści pytania
            query = query.Where(p => p.TrescPytania.Contains(fraza));
        }

        if (!string.IsNullOrWhiteSpace(kategoria))
        {
            // LINQ: Filtrowanie po kategorii
            query = query.Where(p => p.Kategoria == kategoria);
        }

        return await query.ToListAsync();
    }
    
    // Pobieranie wszystkich pytań (również asynchronicznie)
    public async Task<List<Pytanie>> PobierzWszystkiePytaniaAsync()
    {
        return await _context.Pytania.Include(p => p.Odpowiedzi).ToListAsync();
    }

    public async Task DodajPytanieAsync(Pytanie pytanie)
    {
        _context.Pytania.Add(pytanie);
        await _context.SaveChangesAsync();
    }
}
