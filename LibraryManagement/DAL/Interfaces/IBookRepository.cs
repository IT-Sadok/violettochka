using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL.Interfaces
{
    internal interface IBookRepository
    {
        public bool AddBook(Book book);
        public bool UpdateBook(Book book);
        public bool RemoveBook(int id);
        public Book? GetBookById(int id);
        public List<Book> GetAllBooks();

    }
}
