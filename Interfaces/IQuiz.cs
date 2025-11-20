using System.Collections.Generic;

public interface IQuiz
{
    string Nazwa { get; set; }
    List<IQuestion> Pytania { get; set; }

    void DodajPytanie(IQuestion pytanie);
}
