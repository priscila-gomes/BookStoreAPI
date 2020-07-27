using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStoreAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookStoreAPI.Models
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookStoreAPIContext(
                serviceProvider.GetRequiredService<DbContextOptions<BookStoreAPIContext>>()))
            {
                // Look for any book
                if (context.Books.Any())
                {
                    return;   // Data was already seeded
                }

                context.Books.AddRange(
                    new Book
                    {
                        Id = 1,
                        Title = "C# in Depth",
                        Description = "C# in Depth, Fourth Edition is your key to unlocking the powerful new features added to the language " +
                                "in C# 5, 6, and 7. Following the expert guidance of C# legend Jon Skeet, you'll master asynchronous functions, " +
                                "expression-bodied members, interpolated strings, tuples, and much more.",
                        Author = "Jon Skeet",
                        CoverImage = "CoverImage/CSharp.jpg",
                        Price = 37.49M
                    },
                    new Book
                    {
                        Id = 2,
                        Title = "JavaScript: The Good Parts",
                        Description = "Considered the JavaScript expert by many people in the development community, author Douglas Crockford " +
                                "identifies the abundance of good ideas that make JavaScript an outstanding object-oriented programming " +
                                "language-ideas such as functions, loose typing, dynamic objects, and an expressive object literal notation. " +
                                "Unfortunately, these good ideas are mixed in with bad and downright awful ideas, like a programming model " +
                                "based on global variables.",
                        Author = "Douglas Crockford",
                        CoverImage = "CoverImage/Javascript.jpg",
                        Price = 21.59M
                    });

                context.SaveChanges();
            }
        }
        
    }
}
