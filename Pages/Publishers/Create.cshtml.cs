
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Pages.PublisherPages;

public class CreateModel : PageModel
{
    private readonly Costin_Catalina_Lab2Context _context;

    public CreateModel(Costin_Catalina_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Publisher Publisher { get; set; } = default!;

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Publisher.Books");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Publisher.Add(Publisher);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
