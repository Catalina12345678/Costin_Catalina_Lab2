
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Costin_Catalina_Lab2Context _context;

    public EditModel(Costin_Catalina_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    public SelectList MemberList { get; set; } = default!;
    public SelectList BookList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing
            .FirstOrDefaultAsync(m => m.ID == id);

        if (borrowing is null)
        {
            return NotFound();
        }

        Borrowing = borrowing;

        await LoadListsAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            return Page();
        }

        _context.Attach(Borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BorrowingExists(Borrowing.ID))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private async Task LoadListsAsync()
    {
        var members = await _context.Member.ToListAsync();

        MemberList = new SelectList(
            members.Select(m => new
            {
                m.ID,
                Name = m.FullName?.Trim() is { Length: > 0 } name
                    ? name
                    : m.Email
            }),
            "ID",
            "Name"
        );

        var books = await _context.Book
            .Include(b => b.Author)
            .ToListAsync();

        BookList = new SelectList(
            books.Select(b => new
            {
                b.ID,
                Name = b.Title + " - " + (b.Author != null ? b.Author.FullName : "")
            }),
            "ID",
            "Name"
        );
    }

    private bool BorrowingExists(int id)
    {
        return _context.Borrowing.Any(e => e.ID == id);
    }
}
