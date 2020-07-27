using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookStoreAPI.Models;

namespace BookStoreAPI.Data
{
    public class BookStoreAPIContext : DbContext
    {
        public BookStoreAPIContext (DbContextOptions<BookStoreAPIContext> options)
            : base(options)
        {
        }

        public DbSet<BookStoreAPI.Models.Book> Books { get; set; }        
    }
}
