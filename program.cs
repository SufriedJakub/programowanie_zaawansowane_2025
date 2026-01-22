using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== System Quizów: Flagi i Stolice ===");
        
        QuizManager manager = new QuizManager();

        // Sprawdzamy czy baza jest pusta, jeśli tak - dodajemy startowe pytania
        var istniejące = await manager.PobierzWszystkiePytaniaAsync();
        if (istniejące.Count == 0)
        {
            Console.WriteLine("Inicjalizacja bazy danych o flagach...");
            
            var p1 = new Pytanie 
            { 
                TrescPytania = "Jakie miasto jest stolicą tego kraju?", 
                SciezkaObrazka = "polska.png", // Plik w folderze Assets
                Kategoria = "Europa" 
            };
            p1.Odpowiedzi.Add(new Odpowiedz("Kraków", false));
            p1.Odpowiedzi.Add(new Odpowiedz("Warszawa", true));
            p1.Odpowiedzi.Add(new Odpowiedz("Wrocław", false));
            p1.Odpowiedzi.Add(new Odpowiedz("Gdańsk", false));

            await manager.DodajPytanieAsync(p1);

            var p2 = new Pytanie 
            { 
                TrescPytania = "Rozpoznaj flagę tego państwa:", 
                SciezkaObrazka = "francja.png",
                Kategoria = "Europa" 
            };
            p2.Odpowiedzi.Add(new Odpowiedz("Włochy", false));
            p2.Odpowiedzi.Add(new Odpowiedz("Holandia", false));
            p2.Odpowiedzi.Add(new Odpowiedz("Francja", true));

            await manager.DodajPytanieAsync(p2);
            
            Console.WriteLine("Dodano pytania startowe.");
        }

        // Test wymogu JSON
        await manager.EksportujDoJsonAsync("backup_quizu.json");
        Console.WriteLine("Wyeksportowano quiz do pliku JSON.");

        Console.WriteLine("\nSystem gotowy do pracy z interfejsem graficznym!");
    }
}
