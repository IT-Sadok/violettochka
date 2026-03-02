using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BLL.Interfaces
{
    internal interface ILibraryService
    {
        public bool BorrowBook(int id);
        public bool ReturnBook(int id);
        public List<Book> SearchBooks(string? title, string? author);
        public List<Book> GetAvailableBooks();
        public List<Book> GetAllBooks();

        public bool AddBook(Book book);
        public bool UpdateBook(Book book);
        public bool DeleteBook(int id);
        public int GetNextUniqCode();

    }
}
