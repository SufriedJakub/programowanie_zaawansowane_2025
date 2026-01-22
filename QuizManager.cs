using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public async Task<List<Pytanie>> SzukajPytanAsync(string fraza, string kategoria = "")
    {
        var query = _context.Pytania.Include(p => p.Odpowiedzi).AsQueryable();

        if (!string.IsNullOrWhiteSpace(fraza))
        {
            query = query.Where(p => p.TrescPytania.Contains(fraza));
        }

        if (!string.IsNullOrWhiteSpace(kategoria))
        {
            query = query.Where(p => p.Kategoria == kategoria);
        }

        return await query.ToListAsync();
    }
    
    public async Task<List<Pytanie>> PobierzWszystkiePytaniaAsync()
    {
        return await _context.Pytania.Include(p => p.Odpowiedzi).ToListAsync();
    }

    public async Task DodajPytanieAsync(Pytanie pytanie)
    {
        _context.Pytania.Add(pytanie);
        await _context.SaveChangesAsync();
    }

    public async Task EksportujDoJsonAsync(string sciezkaPliku)
    {
        var pytania = await PobierzWszystkiePytaniaAsync();
        var opcje = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(pytania, opcje);
        await File.WriteAllTextAsync(sciezkaPliku, jsonString);
    }
}
