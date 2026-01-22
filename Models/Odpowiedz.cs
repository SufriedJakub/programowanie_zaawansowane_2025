public class Odpowiedz : IAnswer
{
    public int Id { get; set; }
    public int PytanieId { get; set; }
    public string Tresc { get; set; } = "";
    public bool CzyPoprawna { get; set; }

    public Odpowiedz() { }
    
    public Odpowiedz(string tresc, bool poprawna)
    {
        Tresc = tresc;
        CzyPoprawna = poprawna;
    }
}
