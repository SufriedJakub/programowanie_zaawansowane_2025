using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class QuizManager
{
    private readonly QuizContext _context;

    public QuizManager()
    {
        _context = new QuizContext();
        _context.Database.EnsureCreated(); // Tworzy bazę jeśli nie istnieje
    }

    // --- WYMÓG: Programowanie asynchroniczne (Async/Await) ---
    public async Task<List<Pytanie>> PobierzWszystkiePytaniaAsync()
    {
        return await _context.Pytania
            .Include(p => p.Odpowiedzi)
            .ToListAsync();
    }

    // --- WYMÓG: Obsługa plików i serializacja JSON ---
    public async Task EksportujDoJsonAsync(string sciezkaPliku)
    {
        var pytania = await PobierzWszystkiePytaniaAsync();
        var opcje = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(pytania, opcje);
        await File.WriteAllTextAsync(sciezkaPliku, jsonString);
    }

    // --- WYMÓG: Entity Framework Core (Operacja CRUD - Create) ---
    public async Task DodajPytanieAsync(Pytanie pytanie)
    {
        _context.Pytania.Add(pytanie);
        await _context.SaveChangesAsync();
    }
}
