
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Costin_Catalina_Lab2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Costin_Catalina_Lab2.Pages.BookPages;

public class CreateModel : PageModel
{
    private readonly Costin_Catalina_Lab2Context _context;

    public CreateModel(Costin_Catalina_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public IActionResult OnGet()
    {
        LoadDropdowns();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return Page();
        }

        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private void LoadDropdowns()
    {
        ViewData["AuthorID"] = new SelectList(
            _context.Author,
            "ID",
            "LastName",
            Book?.AuthorID
        );

        ViewData["PublisherID"] = new SelectList(
            _context.Publisher,
            "ID",
            "PublisherName",
            Book?.PublisherID
        );
    }
}
