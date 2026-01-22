using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
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
        _ = InicjalizujGreAsync(); // Uruchomienie asynchroniczne
    }

    private async Task InicjalizujGreAsync()
    {
        _pytania = await _manager.PobierzWszystkiePytaniaAsync();
        PokazPytanie();
    }

    private void PokazPytanie()
    {
        if (_aktualnyIndeks >= _pytania.Count)
        {
            PokazKoniec();
            return;
        }

        var pytanie = _pytania[_aktualnyIndeks];
        QuestionText.Text = pytanie.TrescPytania;
        FeedbackText.Text = "";

        // Wymóg: Wyświetlanie obrazka flagi
        if (!string.IsNullOrEmpty(pytanie.SciezkaObrazka))
        {
            try {
                FlagDisplay.Source = new Bitmap($"Assets/{pytanie.SciezkaObrazka}");
            } catch {
                // Jeśli nie znajdzie pliku, flaga zostanie pusta
            }
        }

        AnswersPanel.Children.Clear();

        // Dynamiczne tworzenie przycisków
        foreach (var odp in pytanie.Odpowiedzi)
        {
            var btn = new Button
            {
                Content = odp.Tresc,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Padding = new Avalonia.Thickness(15, 10),
                Background = Brushes.White,
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Avalonia.Thickness(1)
            };
            
            btn.Click += (s, e) => SprawdzOdpowiedz(odp);
            AnswersPanel.Children.Add(btn);
        }
    }

    private void SprawdzOdpowiedz(Odpowiedz wybrana)
    {
        if (wybrana.CzyPoprawna)
        {
            _wynik++;
            FeedbackText.Text = "Poprawnie! 🇵🇱";
            FeedbackText.Foreground = Brushes.Green;
        }
        else
        {
            FeedbackText.Text = "Błąd! ❌";
            FeedbackText.Foreground = Brushes.Red;
        }

        ScoreText.Text = $"Wynik: {_wynik}";
        
        // Czekamy chwilę, żeby użytkownik widział wynik, i następne pytanie
        _aktualnyIndeks++;
        Task.Delay(1000).ContinueWith(_ => Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(PokazPytanie));
    }

    private void PokazKoniec()
    {
        FlagDisplay.IsVisible = false;
        QuestionText.Text = "KONIEC QUIZU!";
        AnswersPanel.Children.Clear();
        FeedbackText.Text = $"Twój końcowy wynik to: {_wynik} / {_pytania.Count}";
        FeedbackText.Foreground = Brushes.DarkBlue;
    }
}
