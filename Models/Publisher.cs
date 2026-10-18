
using System.Collections.Generic;

namespace Costin_Catalina_Lab2.Models
{
    public class Publisher
    {
        public int ID { get; set; }

        public string PublisherName { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
