using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BookStoreAPI.Models
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        public string CoverImage { get; set; }
        
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }

    public class BookCover
    {
        public IFormFile image { get; set; }
        public Book book { get; set; }
    }
}
