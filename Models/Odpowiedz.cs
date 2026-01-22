public class Odpowiedz : IAnswer
{
    public int Id { get; set; }
    public string Tresc { get; set; } = "";
    public bool CzyPoprawna { get; set; }
    public int PytanieId { get; set; } // Klucz obcy dla EF Core
}
