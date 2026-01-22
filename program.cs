using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Task.Run(async () => 
        {
            QuizManager manager = new QuizManager();
            var pytania = await manager.PobierzWszystkiePytaniaAsync();
            
            // Jeśli baza jest pusta, generujemy pytania
            if (pytania.Count == 0)
            {
                Console.WriteLine("Generowanie trudnych pytań...");
                
                var baza = new List<(string Kraj, string Stolica, string Plik)>
                {
                    ("Polska", "Warszawa", "pl.png"), ("Niemcy", "Berlin", "de.png"),
                    ("Francja", "Paryż", "fr.png"), ("Włochy", "Rzym", "it.png"),
                    ("Hiszpania", "Madryt", "es.png"), ("Wlk. Brytania", "Londyn", "gb.png"),
                    ("USA", "Waszyngton", "us.png"), ("Japonia", "Tokio", "jp.png"),
                    ("Chiny", "Pekin", "cn.png"), ("Brazylia", "Brasilia", "br.png"),
                    ("Meksyk", "Meksyk", "mx.png"), ("Korea Płd.", "Seul", "kr.png"),
                    ("Holandia", "Amsterdam", "nl.png"), ("Belgia", "Bruksela", "be.png"),
                    ("Portugalia", "Lizbona", "pt.png"), ("Wietnam", "Hanoi", "vn.png"),
                    ("Tajlandia", "Bangkok", "th.png"), ("Czechy", "Praga", "cz.png"),
                    ("Dania", "Kopenhaga", "dk.png"), ("Finlandia", "Helsinki", "fi.png"),
                    ("Kanada", "Ottawa", "ca.png"), ("Argentyna", "Buenos Aires", "ar.png"),
                    ("Indie", "New Delhi", "in.png"), ("Egipt", "Kair", "eg.png"),
                    ("Australia", "Canberra", "au.png"), ("Turcja", "Ankara", "tr.png"),
                    ("Szwecja", "Sztokholm", "se.png"), ("Norwegia", "Oslo", "no.png"),
                    ("Ukraina", "Kijów", "ua.png"), ("Grecja", "Ateny", "gr.png")
                };

                foreach (var item in baza)
                {
                    // PYTANIE O KRAJ (Bez podpowiedzi)
                    var zleKraje = baza.Where(x => x.Kraj != item.Kraj).OrderBy(x => Guid.NewGuid()).Take(3).ToList();
                    var p1 = new Pytanie 
                    { 
                        TrescPytania = "Jaki to kraj?", // Krótko i zwięźle
                        SciezkaObrazka = item.Plik, 
                        Kategoria = "KRAJE" 
                    };
                    p1.Odpowiedzi.Add(new Odpowiedz(item.Kraj, true));
                    foreach (var z in zleKraje) p1.Odpowiedzi.Add(new Odpowiedz(z.Kraj, false));
                    await manager.DodajPytanieAsync(p1);

                    // PYTANIE O STOLICĘ (Trudne - bez nazwy kraju!)
                    var zleStolice = baza.Where(x => x.Stolica != item.Stolica).OrderBy(x => Guid.NewGuid()).Take(3).ToList();
                    var p2 = new Pytanie 
                    { 
                        TrescPytania = "Jaka to stolica?", // Nie podajemy nazwy kraju!
                        SciezkaObrazka = item.Plik, 
                        Kategoria = "STOLICE" 
                    };
                    p2.Odpowiedzi.Add(new Odpowiedz(item.Stolica, true));
                    foreach (var z in zleStolice) p2.Odpowiedzi.Add(new Odpowiedz(z.Stolica, false));
                    await manager.DodajPytanieAsync(p2);
                }
                Console.WriteLine("Baza gotowa!");
            }
        }).Wait();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace();
}
