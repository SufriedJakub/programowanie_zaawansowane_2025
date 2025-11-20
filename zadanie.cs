using System;
using System.Collections.Generic;


public interface IAnswer
{
    string Tresc { get; set; }
    bool CzyPoprawna { get; set; }
}

public interface IQuestion
{
    string TrescPytania { get; set; }
    List<IAnswer> Odpowiedzi { get; set; }

    void DodajOdpowiedz(IAnswer odp);
}

public interface IQuiz
{
    string Nazwa { get; set; }
    List<IQuestion> Pytania { get; set; }

    void DodajPytanie(IQuestion pytanie);
}

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

public class Quiz : IQuiz
{
    public string Nazwa { get; set; }
    public List<IQuestion> Pytania { get; set; }

    public Quiz(string nazwa)
    {
        Nazwa = nazwa;
        Pytania = new List<IQuestion>();
    }

    public void DodajPytanie(IQuestion pytanie)
    {
        Pytania.Add(pytanie);
    }
}
