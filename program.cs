using Avalonia;
using System;
using System.Threading.Tasks;

namespace QuizApp;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        // 1. Logika Bazy Danych (wykonana w tle)
        Task.Run(async () => 
        {
            Console.WriteLine("=== Startowanie Quizu... ===");
            QuizManager manager = new QuizManager();
            var pytania = await manager.PobierzWszystkiePytaniaAsync();
            
            if (pytania.Count == 0)
            {
                // Dodajemy pytania startowe, jeśli baza jest pusta
                 var p1 = new Pytanie { 
                    TrescPytania = "Stolicą tego kraju jest Berlin. O jakim państwie mowa?", 
                    SciezkaObrazka = "niemcy.png", Kategoria = "Europa" 
                };
                p1.Odpowiedzi.Add(new Odpowiedz("Niemcy", true));
                p1.Odpowiedzi.Add(new Odpowiedz("Belgia", false));
                await manager.DodajPytanieAsync(p1);
                
                Console.WriteLine("Dodano pytania do bazy.");
            }
        }).Wait();

        // 2. Uruchomienie Okna Graficznego
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
