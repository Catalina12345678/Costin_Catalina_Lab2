
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Costin_Catalina_Lab2Context _context;

    public IndexModel(Costin_Catalina_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = new List<Book>();

    public BookData BookD { get; set; } = new BookData();

    public int BookID { get; set; }

    public int CategoryID { get; set; }

    public string TitleSort { get; set; } = string.Empty;

    public string AuthorSort { get; set; } = string.Empty;

    public string CurrentFilter { get; set; } = string.Empty;

    public async Task OnGetAsync(
        int? id,
        int? categoryID,
        string? sortOrder,
        string? searchString)
    {
        BookD = new BookData();

        CurrentFilter = searchString ?? string.Empty;

        TitleSort = string.IsNullOrEmpty(sortOrder)
            ? "title_desc"
            : "";

        AuthorSort = sortOrder == "author"
            ? "author_desc"
            : "author";

        BookD.Books = await _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .ToListAsync();

        // Cautare dupa titlul cartii sau numele autorului
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            BookD.Books = BookD.Books.Where(b =>
                (b.Title != null &&
                 b.Title.Contains(searchString,
                     StringComparison.OrdinalIgnoreCase)) ||

                (b.Author != null &&
                 b.Author.FullName.Contains(searchString,
                     StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        if (id != null)
        {
            BookID = id.Value;

            var book = BookD.Books
                .SingleOrDefault(b => b.ID == id.Value);

            if (book != null)
            {
                BookD.Categories = book.BookCategories?
                    .Where(bc => bc.Category != null)
                    .Select(bc => bc.Category!)
                    .ToList() ?? new List<Category>();
            }
        }

        if (categoryID != null)
        {
            CategoryID = categoryID.Value;
        }

        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(b => b.Title);
                break;

            case "author_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(b => b.Author?.FullName);
                break;

            case "author":
                BookD.Books = BookD.Books
                    .OrderBy(b => b.Author?.FullName);
                break;

            default:
                BookD.Books = BookD.Books
                    .OrderBy(b => b.Title);
                break;
        }

        Book = BookD.Books.ToList();
    }
}
