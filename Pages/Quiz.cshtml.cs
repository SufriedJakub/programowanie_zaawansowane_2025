using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class QuizWebModel : PageModel
{
    private readonly QuizContext _context;

    public QuizWebModel()
    {
        _context = new QuizContext();
    }

    public List<Pytanie> Pytania { get; set; } = new();

    // --- WYMÓG: Programowanie asynchroniczne ---
    public async Task OnGetAsync()
    {
        Pytania = await _context.Pytania
            .Include(p => p.Odpowiedzi)
            .ToListAsync();
    }
}
