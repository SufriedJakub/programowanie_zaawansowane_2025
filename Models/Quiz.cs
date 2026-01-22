using System.Collections.Generic;

public class Quiz : IQuiz<Pytanie, Odpowiedz>
{
    public int Id { get; set; }
    public string Nazwa { get; set; } = "";
    public List<Pytanie> Pytania { get; set; } = new();
}
