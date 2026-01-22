using System.Collections.Generic;

public interface IQuestion<T> where T : IAnswer
{
    int Id { get; set; }
    string TrescPytania { get; set; }
    string? SciezkaObrazka { get; set; } // Pod flagi
    List<T> Odpowiedzi { get; set; }
}
