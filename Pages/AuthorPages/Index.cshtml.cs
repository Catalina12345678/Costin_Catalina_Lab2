using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Pages.AuthorPages;

public class IndexModel : PageModel
{
    private readonly Costin_Catalina_Lab2Context _context;

    public IndexModel(Costin_Catalina_Lab2Context context)
    {
        _context = context;
    }

    public IList<Author> Author { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Author = await _context.Author.ToListAsync();
    }
}
