using LibraryManagement.BLL.Interfaces;
using LibraryManagement.DAL.Interfaces;
using LibraryManagement.Enums;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BLL.Services
{
    internal class LibraryService : ILibraryService
    {
        private readonly IBookRepository _bookRepository;
        public LibraryService(IBookRepository bookRepository) 
        {
            _bookRepository = bookRepository;
        }

        public bool AddBook(Book book)
        {
            if(book == null)
            {
                return false;
            }
            return _bookRepository.AddBook(book);
        }

        public bool BorrowBook(int id)
        {
            Book bookToBorrow = _bookRepository.GetBookById(id);

            if(bookToBorrow == null || bookToBorrow.BookStatus == BookStatus.Unavailable)
            {
                return false;
            }

            bookToBorrow.BookStatus = BookStatus.Unavailable;
            _bookRepository.UpdateBook(bookToBorrow);

            return true;
        }

        public bool DeleteBook(int id)
        {
            return _bookRepository.RemoveBook(id);
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public List<Book> GetAvailableBooks()
        {
            return _bookRepository
                .GetAllBooks()
                .Where(b => b.BookStatus == BookStatus.Available)
                .ToList();
        }

        public bool ReturnBook(int id)
        {
            Book? bookToReturn = _bookRepository.GetBookById(id);

            if(bookToReturn == null || bookToReturn.BookStatus == BookStatus.Available)
            {
                return false;
            }

            bookToReturn.BookStatus = BookStatus.Available;
            _bookRepository.UpdateBook(bookToReturn);

            return true;
        }

        public List<Book> SearchBooks(string? title, string? author)
        {
            return _bookRepository
                .GetAllBooks()
                .Where(b => 
                    (!string.IsNullOrWhiteSpace(title) && b.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                    || 
                    (!string.IsNullOrWhiteSpace(author) && b.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
                ).ToList();
        }

        public bool UpdateBook(Book book)
        {
            if(book == null)
            {
                return false;
            }
            return _bookRepository.UpdateBook(book);
        }

        public int GetNextUniqCode()
        {
            var allBooks = _bookRepository.GetAllBooks();
            if (allBooks.Count == 0) return 1;

            return allBooks.Max(b => b.UniqCode) + 1;
        }
    }
}
