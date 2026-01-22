using Microsoft.EntityFrameworkCore;
using QuizWeb.Data;
using QuizWeb.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<QuizContext>();

var app = builder.Build();

// === NOWA, INTELIGENTNA BAZA DANYCH ===
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QuizContext>();
    db.Database.EnsureCreated();

    if (!db.Pytania.Any())
    {
        Console.WriteLine("--> Generowanie inteligentnych pytań...");
        
        var baza = new List<(string Kraj, string Plik)> 
        {
            ("Polska", "pl.png"), ("Niemcy", "de.png"),
            ("Francja", "fr.png"), ("Włochy", "it.png"),
            ("Hiszpania", "es.png"), ("USA", "us.png"),
            ("Japonia", "jp.png"), ("Brazylia", "br.png"),
            ("Wlk. Brytania", "gb.png"), ("Kanada", "ca.png"),
            ("Argentyna", "ar.png"), ("Chiny", "cn.png"),
            ("Meksyk", "mx.png"), ("Australia", "au.png")
        };

        foreach(var item in baza)
        {
            // Losujemy 3 błędne odpowiedzi z tej samej listy
            var bledne = baza.Where(x => x.Kraj != item.Kraj)
                             .OrderBy(x => Guid.NewGuid())
                             .Take(3)
                             .ToList();

            var p = new Pytanie 
            { 
                TrescPytania = "Jaki to kraj?", 
                SciezkaObrazka = item.Plik, 
                Kategoria = "Kraje" 
            };
            
            p.Odpowiedzi.Add(new Odpowiedz(item.Kraj, true));
            foreach(var b in bledne) 
            {
                p.Odpowiedzi.Add(new Odpowiedz(b.Kraj, false));
            }
            
            db.Pytania.Add(p);
        }
        db.SaveChanges();
        Console.WriteLine("--> Gotowe!");
    }
}
// ============================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapBlazorHub();

app.Run();
