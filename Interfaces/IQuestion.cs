using System.Collections.Generic;

public interface IQuestion
{
    string TrescPytania { get; set; }
    List<IAnswer> Odpowiedzi { get; set; }

    void DodajOdpowiedz(IAnswer odp);
}
