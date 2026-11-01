
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Costin_Catalina_Lab2.Models;

namespace Costin_Catalina_Lab2.Models
{
    public class Costin_Catalina_Lab2Context : DbContext
    {
        public DbSet<Costin_Catalina_Lab2.Models.Category> Category { get; set; } = default!;
        public Costin_Catalina_Lab2Context(
            DbContextOptions<Costin_Catalina_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Book> Book { get; set; } = default!;

        public DbSet<Author> Author { get; set; } = default!;

        public DbSet<Publisher> Publisher { get; set; } = default!;
    }
}
