
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Costin_Catalina_Lab2Context _context;

    public IndexModel(Costin_Catalina_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = new List<Category>();

    public int CategoryID { get; set; }

    public IList<Book> Books { get; set; } = new List<Book>();

    public async Task OnGetAsync(int? id)
    {
        Category = await _context.Category
            .AsNoTracking()
            .OrderBy(c => c.CategoryName)
            .ToListAsync();

        if (id != null)
        {
            CategoryID = id.Value;

            Books = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                .AsNoTracking()
                .Where(b => b.BookCategories != null &&
                    b.BookCategories.Any(bc => bc.CategoryID == id.Value))
                .OrderBy(b => b.Title)
                .ToListAsync();
        }
    }
}
