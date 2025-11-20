using System;

public class Odpowiedz : IAnswer
{
    public string Tresc { get; set; }
    public bool CzyPoprawna { get; set; }

    public Odpowiedz(string tresc, bool poprawna)
    {
        Tresc = tresc;
        CzyPoprawna = poprawna;
    }
}
