
using Microsoft.AspNetCore.Mvc.RazorPages;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Models
{
    public class BookCategoriesPageModel : PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList { get; set; }
            = new List<AssignedCategoryData>();

        public void PopulateAssignedCategoryData(
            Costin_Catalina_Lab2Context context, Book book)
        {
            var allCategories = context.Category.ToList();

            var bookCategories = new HashSet<int>(
                book.BookCategories?
                    .Select(c => c.CategoryID)
                ?? Enumerable.Empty<int>());

            AssignedCategoryDataList = new List<AssignedCategoryData>();

            foreach (var cat in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = cat.ID,
                    Name = cat.CategoryName,
                    Assigned = bookCategories.Contains(cat.ID)
                });
            }
        }

        public void UpdateBookCategories(
            Costin_Catalina_Lab2Context context,
            string[] selectedCategories,
            Book bookToUpdate)
        {
            if (selectedCategories == null)
            {
                bookToUpdate.BookCategories = new List<BookCategory>();
                return;
            }

            var selectedCategoriesHS =
                new HashSet<string>(selectedCategories);

            var bookCategories = new HashSet<int>(
                bookToUpdate.BookCategories?
                    .Select(c => c.CategoryID)
                ?? Enumerable.Empty<int>());

            foreach (var cat in context.Category)
            {
                if (selectedCategoriesHS.Contains(cat.ID.ToString()))
                {
                    if (!bookCategories.Contains(cat.ID))
                    {
                        bookToUpdate.BookCategories ??= new List<BookCategory>();

                        bookToUpdate.BookCategories.Add(new BookCategory
                        {
                            BookID = bookToUpdate.ID,
                            CategoryID = cat.ID
                        });
                    }
                }
                else
                {
                    if (bookCategories.Contains(cat.ID))
                    {
                        var categoryToRemove =
                            bookToUpdate.BookCategories!
                                .FirstOrDefault(
                                    c => c.CategoryID == cat.ID);

                        if (categoryToRemove != null)
                        {
                            context.Set<BookCategory>().Remove(categoryToRemove);
                        }
                    }
                }
            }
        }
    }
}
