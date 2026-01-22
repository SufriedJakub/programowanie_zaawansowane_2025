using System.Collections.Generic;

public interface IQuiz<T, TAns> where T : IQuestion<TAns> where TAns : IAnswer
{
    string Nazwa { get; set; }
    List<T> Pytania { get; set; }
}
