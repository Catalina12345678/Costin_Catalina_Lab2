
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
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
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.ID == id);

        if (book == null)
        {
            return NotFound();
        }

        Book = book;

        PopulateAssignedCategoryData(_context, Book);

        await LoadDropdownsAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        int? id, string[] selectedCategories)
    {
        int bookID = id ?? Book.ID;

        var bookToUpdate = await _context.Book
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.ID == bookID);

        if (bookToUpdate == null)
        {
            return NotFound();
        }

        ModelState.Remove("Book.Author");
        ModelState.Remove("Book.Publisher");
        ModelState.Remove("Book.BookCategories");

        if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            b => b.Title,
            b => b.AuthorID,
            b => b.Price,
            b => b.PublishingDate,
            b => b.PublisherID))
        {
            UpdateBookCategories(
                _context,
                selectedCategories,
                bookToUpdate);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        Book = bookToUpdate;

        PopulateAssignedCategoryData(_context, bookToUpdate);

        await LoadDropdownsAsync();

        return Page();
    }
}
