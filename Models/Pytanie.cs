using System.Collections.Generic;

public class Pytanie : IQuestion<Odpowiedz>
{
    public int Id { get; set; }
    public string TrescPytania { get; set; } = "";
    public string? SciezkaObrazka { get; set; }
    public string Kategoria { get; set; } = "Ogólne";
    public List<Odpowiedz> Odpowiedzi { get; set; } = new();
}
