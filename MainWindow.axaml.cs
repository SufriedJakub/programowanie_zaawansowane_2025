using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform; // Potrzebne do ładowania zasobów
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp;

public partial class MainWindow : Window
{
    private readonly QuizManager _manager = new QuizManager();
    private List<Pytanie> _pytania = new();
    private int _aktualnyIndeks = 0;
    private int _wynik = 0;

    public MainWindow()
    {
        InitializeComponent();
        _ = InicjalizujGreAsync();
    }

    private async Task InicjalizujGreAsync()
    {
        // Wykorzystanie managera do pobrania danych
        _pytania = await _manager.PobierzWszystkiePytaniaAsync();
        PokazPytanie();
    }

    private void PokazPytanie()
    {
        if (_aktualnyIndeks >= _pytania.Count)
        {
            PokazPodsumowanie();
            return;
        }

        var pytanie = _pytania[_aktualnyIndeks];
        QuestionText.Text = pytanie.TrescPytania;
        
        // --- WYMÓG: Obsługa obrazków (Flagi) ---
        if (!string.IsNullOrEmpty(pytanie.SciezkaObrazka))
        {
            try 
            {
                // Ścieżka relatywna do folderu uruchomieniowego na Macu
                string imagePath = System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", pytanie.SciezkaObrazka);
                if (System.IO.File.Exists(imagePath))
                {
                    FlagDisplay.Source = new Bitmap(imagePath);
                    FlagDisplay.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd ładowania obrazka: {ex.Message}");
                FlagDisplay.IsVisible = false;
            }
        }
        else 
        {
            FlagDisplay.IsVisible = false;
        }

        GenerujPrzyciski(pytanie.Odpowiedzi);
    }

    private void GenerujPrzyciski(List<Odpowiedz> odpowiedzi)
    {
        AnswersPanel.Children.Clear();
        foreach (var odp in odpowiedzi)
        {
            var btn = new Button
            {
                Content = odp.Tresc,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Margin = new Avalonia.Thickness(0, 5)
            };
            btn.Click += (s, e) => ObslugaWyboru(odp);
            AnswersPanel.Children.Add(btn);
        }
    }

    private void ObslugaWyboru(Odpowiedz wybrana)
    {
        if (wybrana.CzyPoprawna)
        {
            _wynik++;
            FeedbackText.Text = "Dobrze!";
            FeedbackText.Foreground = Brushes.Green;
        }
        else
        {
            FeedbackText.Text = "Źle!";
            FeedbackText.Foreground = Brushes.Red;
        }

        ScoreText.Text = $"Wynik: {_wynik}";
        
        // Przejście do następnego pytania po sekundzie
        _aktualnyIndeks++;
        Task.Delay(1000).ContinueWith(_ => 
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(PokazPytanie));
    }

    private void PokazPodsumowanie()
    {
        FlagDisplay.IsVisible = false;
        QuestionText.Text = "Koniec Quizu!";
        AnswersPanel.Children.Clear();
        FeedbackText.Text = $"Twój wynik: {_wynik} / {_pytania.Count}";
        FeedbackText.Foreground = Brushes.Blue;
    }
}
