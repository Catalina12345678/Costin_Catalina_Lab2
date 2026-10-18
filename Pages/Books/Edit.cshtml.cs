
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Pages.BookPages;

public class EditModel : PageModel
{
    private readonly Costin_Catalina_Lab2Context _context;

    public EditModel(Costin_Catalina_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    private async Task LoadDropdownsAsync()
    {
        ViewData["AuthorID"] = new SelectList(
            await _context.Author.ToListAsync(),
            "ID",
            "LastName",
            Book.AuthorID
        );

        ViewData["PublisherID"] = new SelectList(
            await _context.Publisher.ToListAsync(),
            "ID",
            "PublisherName",
            Book.PublisherID
        );
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (book is null)
        {
            return NotFound();
        }

        Book = book;

        await LoadDropdownsAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Book.Author");
        ModelState.Remove("Book.Publisher");

        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return Page();
        }

        _context.Attach(Book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(Book.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}
