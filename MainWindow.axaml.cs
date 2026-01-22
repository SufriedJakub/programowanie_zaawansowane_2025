using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApp;

public partial class MainWindow : Window
{
    private QuizManager _manager;
    private List<Pytanie> _wszystkiePytaniaZBazy;
    private List<Pytanie> _aktualnePytaniaGry;
    
    private int _index = 0;
    private int _punkty = 0;
    private int _bledy = 0;
    private int _targetScore = 5; 
    
    private bool _isHardcore = false;
    private static int _highScore = 0;

    public MainWindow()
    {
        InitializeComponent();
        LadowanieDanych();
    }

    private async void LadowanieDanych()
    {
        _manager = new QuizManager();
        _wszystkiePytaniaZBazy = await _manager.PobierzWszystkiePytaniaAsync();
    }

    // --- MENU ---
    private void Menu_Kraje_Click(object sender, RoutedEventArgs e) => UruchomGre("KRAJE");
    private void Menu_Stolice_Click(object sender, RoutedEventArgs e) => UruchomGre("STOLICE");
    private void Menu_Mieszany_Click(object sender, RoutedEventArgs e) => UruchomGre("MIESZANY");
    private void Menu_Hardcore_Click(object sender, RoutedEventArgs e) => UruchomGre("HARDCORE");

    // --- SILNIK GRY ---
    private void UruchomGre(string tryb)
    {
        _index = 0;
        _punkty = 0;
        _bledy = 0;
        _isHardcore = false;
        
        // Reset wyglądu (Domyślny tekst czarny)
        QuestionText.Foreground = Brush.Parse("Black");
        HighScoreText.IsVisible = false; // Domyślnie ukrywamy rekord

        if (tryb == "HARDCORE")
        {
            _isHardcore = true;
            _aktualnePytaniaGry = _wszystkiePytaniaZBazy.OrderBy(x => Guid.NewGuid()).ToList();
            _targetScore = 9999;
            
            ModeTitle.Text = "🔥 NAGŁA ŚMIERĆ 🔥";
            
            // Wygląd HARDCORE
            MainRoot.Background = Brush.Parse("#8B0000"); // Ciemna czerwień
            QuestionText.Foreground = Brush.Parse("White");
            HighScoreText.IsVisible = true; // Tu pokazujemy rekord!
            HighScoreText.Foreground = Brush.Parse("White");
        }
        else if (tryb == "MIESZANY")
        {
            _aktualnePytaniaGry = _wszystkiePytaniaZBazy.OrderBy(x => Guid.NewGuid()).ToList();
            _targetScore = 15;
            ModeTitle.Text = "TRYB: ŁĄCZONY";
            MainRoot.Background = Brush.Parse("#F3E5F5"); // Jasny Fiolet
        }
        else if (tryb == "STOLICE")
        {
            _aktualnePytaniaGry = _wszystkiePytaniaZBazy.Where(p => p.Kategoria == "STOLICE").OrderBy(x => Guid.NewGuid()).ToList();
            _targetScore = 5;
            ModeTitle.Text = "TRYB: ZGADNIJ STOLICĘ";
            MainRoot.Background = Brush.Parse("#FFF3E0"); // Jasny Pomarańcz
        }
        else // KRAJE
        {
            _aktualnePytaniaGry = _wszystkiePytaniaZBazy.Where(p => p.Kategoria == "KRAJE").OrderBy(x => Guid.NewGuid()).ToList();
            _targetScore = 5;
            ModeTitle.Text = "TRYB: ZGADNIJ KRAJ";
            MainRoot.Background = Brush.Parse("#E3F2FD"); // Jasny Niebieski
        }

        if (_aktualnePytaniaGry.Count == 0) return;

        MenuPanel.IsVisible = false;
        GameOverPanel.IsVisible = false;
        GamePanel.IsVisible = true;

        PokazPytanie();
    }

    private void PokazPytanie()
    {
        FlagImage.Source = null;
        
        if (_isHardcore)
            StatsTop.Text = $"SERIA: {_punkty}";
        else
            StatsTop.Text = $"Punkty: {_punkty}/{_targetScore} | Błędy: {_bledy}";

        if (_index >= _aktualnePytaniaGry.Count)
        {
            PokazKoniec(true);
            return;
        }

        var p = _aktualnePytaniaGry[_index];
        QuestionText.Text = p.TrescPytania;

        try
        {
            string sciezka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", p.SciezkaObrazka);
            if (File.Exists(sciezka)) FlagImage.Source = new Bitmap(sciezka);
        } catch { }

        AnswersList.ItemsSource = p.Odpowiedzi.OrderBy(x => Guid.NewGuid()).ToList();
    }

    private void Answer_Click(object sender, RoutedEventArgs e)
    {
        var btn = (Button)sender;
        var odp = (Odpowiedz)btn.DataContext;

        if (odp.CzyPoprawna)
        {
            _punkty++;
            
            if (_isHardcore && _punkty > _highScore)
            {
                _highScore = _punkty;
                HighScoreText.Text = $"🏆 Rekord: {_highScore}";
            }

            if (_punkty >= _targetScore)
            {
                PokazKoniec(true);
                return;
            }
        }
        else
        {
            if (_isHardcore)
            {
                PokazKoniec(false);
                return;
            }
            _bledy++;
        }
        
        _index++;
        PokazPytanie();
    }

    private void PokazKoniec(bool sukces)
    {
        GamePanel.IsVisible = false;
        GameOverPanel.IsVisible = true;
        
        // Reset tła na domyślny
        MainRoot.Background = Brush.Parse("#F5F5F5"); 

        if (_isHardcore)
        {
            if (sukces)
            {
                EndTitle.Text = "👑 JESTEŚ BOGIEM! 👑";
                EndTitle.Foreground = Brush.Parse("Gold");
                FinalScoreText.Text = $"Bezbłędnie!\nWynik: {_punkty}";
            }
            else
            {
                EndTitle.Text = "💀 ZGINĄŁEŚ! 💀";
                EndTitle.Foreground = Brush.Parse("Red");
                FinalScoreText.Text = $"Wynik serii: {_punkty}\nTwój Rekord: {_highScore}";
            }
        }
        else
        {
            EndTitle.Text = "ZWYCIĘSTWO!";
            EndTitle.Foreground = Brush.Parse("Green");
            FinalScoreText.Text = $"Błędnych odpowiedzi: {_bledy}";
        }
    }

    private void BackToMenu_Click(object sender, RoutedEventArgs e)
    {
        GameOverPanel.IsVisible = false;
        MenuPanel.IsVisible = true;
        
        // W menu zawsze pokazujemy rekord i szare tło
        HighScoreText.IsVisible = true;
        HighScoreText.Foreground = Brush.Parse("#555555");
        MainRoot.Background = Brush.Parse("#F5F5F5");
        HighScoreText.Text = $"🏆 Rekord: {_highScore}";
    }
}
