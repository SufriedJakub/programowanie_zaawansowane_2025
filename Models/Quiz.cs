using System.Collections.Generic;

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
