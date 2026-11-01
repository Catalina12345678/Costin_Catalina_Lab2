
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Pages.BookPages;

public class CreateModel : BookCategoriesPageModel
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

        var book = new Book
        {
            BookCategories = new List<BookCategory>()
        };

        PopulateAssignedCategoryData(_context, book);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        string[] selectedCategories)
    {
        ModelState.Remove("Book.Author");
        ModelState.Remove("Book.Publisher");
        ModelState.Remove("Book.BookCategories");

        if (!ModelState.IsValid)
        {
            LoadDropdowns();

            var book = new Book
            {
                BookCategories = new List<BookCategory>()
            };

            PopulateAssignedCategoryData(_context, book);

            foreach (var category in AssignedCategoryDataList)
            {
                category.Assigned = selectedCategories != null &&
                    selectedCategories.Contains(
                        category.CategoryID.ToString());
            }

            return Page();
        }

        Book.BookCategories = new List<BookCategory>();

        if (selectedCategories != null)
        {
            var validCategoryIDs = _context.Set<Category>()
                .Select(c => c.ID)
                .ToHashSet();

            foreach (var cat in selectedCategories)
            {
                if (int.TryParse(cat, out int categoryID) &&
                    validCategoryIDs.Contains(categoryID) &&
                    !Book.BookCategories.Any(
                        bc => bc.CategoryID == categoryID))
                {
                    Book.BookCategories.Add(new BookCategory
                    {
                        CategoryID = categoryID
                    });
                }
            }
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
