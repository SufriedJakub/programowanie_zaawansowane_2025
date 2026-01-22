using System.Collections.Generic;

namespace QuizWeb.Interfaces;

public interface IQuestion<T>
{
    string TrescPytania { get; }
    List<T> Odpowiedzi { get; }
}
