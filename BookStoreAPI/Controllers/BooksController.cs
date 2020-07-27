using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreAPI.Data;
using BookStoreAPI.Models;
using System.IO;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private BookStoreAPIContext _context;        

        public BooksController(BookStoreAPIContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public IEnumerable<Book> GetBook()
        {
            return _context.Books;
        }

        // GET: api/Books/2
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            /*byte[] bytes = ReadFile(book.CoverImage);
            var imageDataStream = new MemoryStream(bytes);
            imageDataStream.Position = 0;             
            return File(bytes, "image/png");*/

            return Ok(book);
        }

        // PUT: api/Books/2
        [HttpPut("{id}")]
        public IActionResult PutBook(int id, [FromForm]BookCover bcover)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Getting Name
            Book book = bcover.book;

            // Getting Image
            var image = bcover.image;

            if (id != book.Id)
            {
                return BadRequest();
            }

            // Saving Image on Server
            if (image != null && image.Length > 0) 
            {
                //Delete old file
                if (System.IO.File.Exists(book.CoverImage))
                {
                    try
                    {
                        System.IO.File.Delete(book.CoverImage);
                    }
                    catch
                    { 
                    }
                }

                    string path = "CoverImage/" + image.FileName;
                book.CoverImage = path;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }

            _context.Entry(book).State = EntityState.Modified;
            
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
            return Ok(new { status = true, message = "Book updated successfully." });
        }

        // POST: api/Books
        [HttpPost]
        public IActionResult PostBook([FromForm]BookCover bcover)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Getting Name
            Book book = bcover.book;
            
            // Getting Image
            var image = bcover.image;

            //Determine the next ID
            var newID = _context.Books.Select(x => x.Id).Max() + 1;
            book.Id = newID;            

            // Saving Image on Server
            if (image!=null && image.Length > 0)
            {
                string path = "CoverImage/" + image.FileName;
                book.CoverImage = path;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            

            _context.Books.Add(book);
            _context.SaveChanges();

            //return CreatedAtAction("GetBook", new { id = book.Id }, book);
            return Ok(new { status = true, message = "Book added successfully." });
        }

        // DELETE: api/Books/2
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            //return Ok(book);
            return Ok(new { status = true, message = "Book deleted successfully." });
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);            
        }

        public static byte[] ReadFile(string sPath)
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to supply number of bytes 
            //to read from file.
            //In this case we want to read entire file. 
            //So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);

            return data;
        }
    }
}