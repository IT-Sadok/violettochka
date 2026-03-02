using LibraryManagement.DAL.Interfaces;
using LibraryManagement.Enums;
using LibraryManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL.Repositories
{
    internal class JsonBookRepository : IBookRepository
    {
        private readonly string _filePath;

        public JsonBookRepository(string filePath)
        {
            _filePath = filePath;
        }

        public bool AddBook(Book book)
        {
            List<Book> allCurrentBooks = ReadAllFromFile();

            if(allCurrentBooks.Any(b => b.UniqCode == book.UniqCode))
            {
                return false;
            }

            allCurrentBooks.Add(book);

            SaveToFile(allCurrentBooks);

            return true;
        }

        public List<Book> GetAllBooks()
        {
            return ReadAllFromFile();
        }

        public Book? GetBookById(int id)
        {
            List<Book> allCurrentBooks = ReadAllFromFile();
            return allCurrentBooks.FirstOrDefault(b => b.UniqCode == id);
        }

        public bool RemoveBook(int id)
        {
            List<Book> allCurrentBooks = ReadAllFromFile();
            Book? bookToRemove = allCurrentBooks.FirstOrDefault(b => b.UniqCode == id);
            if (bookToRemove == null)
            {
                return false;
            }

            allCurrentBooks.Remove(bookToRemove);

            SaveToFile(allCurrentBooks);

            return true;
        }

        public bool UpdateBook(Book book)
        {
            List<Book> allCurrentBooks = ReadAllFromFile();
            Book? bookToUpdate = allCurrentBooks.FirstOrDefault(b => b.UniqCode == book.UniqCode);

            if (bookToUpdate == null)
            {
                return false;
            }

            bookToUpdate.Author = book.Author;
            bookToUpdate.Title = book.Title;
            bookToUpdate.BookStatus = book.BookStatus;
            bookToUpdate.ReleaseYear = book.ReleaseYear;

            SaveToFile(allCurrentBooks);

            return true;
        }

        private List<Book> ReadAllFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<Book>();
                }

                string jsonBooks = File.ReadAllText(_filePath);

                if (string.IsNullOrWhiteSpace(jsonBooks))
                {
                    return new List<Book>();
                }

                return JsonConvert.DeserializeObject<List<Book>>(jsonBooks) ?? new List<Book>();
            }
            catch (Exception ex) when (ex is IOException or JsonException)
            {
                return new List<Book>();
            }
        }

        private void SaveToFile(List<Book> books)
        {
            try
            {
                string? directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string serializedBook = JsonConvert.SerializeObject(books, Formatting.Indented);
                File.WriteAllText(_filePath, serializedBook);
            }
            catch (Exception ex) when (ex is IOException or JsonException)
            {
                throw new InvalidOperationException($"Не вдалося зберегти дані у файл: {ex.Message}", ex);
            }
        }

    }
}
