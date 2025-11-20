using System.Collections.Generic;

public class Pytanie : IQuestion
{
    public string TrescPytania { get; set; }
    public List<IAnswer> Odpowiedzi { get; set; }

    public Pytanie(string tresc)
    {
        TrescPytania = tresc;
        Odpowiedzi = new List<IAnswer>();
    }

    public void DodajOdpowiedz(IAnswer odp)
    {
        Odpowiedzi.Add(odp);
    }
}
