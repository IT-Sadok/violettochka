using LibraryManagement.BLL.Interfaces;
using LibraryManagement.Enums;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LibraryManagement.UI
{
    internal class MenuPresentationBook
    {
        private readonly ILibraryService _libraryService;

        public MenuPresentationBook(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("1. Показати всі книги");
                Console.WriteLine("2. Показати доступні книги");
                Console.WriteLine("3. Додати книгу");
                Console.WriteLine("4. Взяти книгу");
                Console.WriteLine("5. Повернути книгу");
                Console.WriteLine("6. Видалити книгу");
                Console.WriteLine("7. Пошукати книгу");
                Console.WriteLine("0. Вийти");

                Console.Write("Оберіть дію: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllBooks(); break;
                    case "2": ShowAvailableBooks(); break;
                    case "3": AddBook(); break;
                    case "4": BorrowBook(); break;
                    case "5": ReturnBook(); break;
                    case "6": DeleteBook(); break;
                    case "7": SearchBooks(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }

                Console.WriteLine(); 
            }
        }

        private void ShowAllBooks()
        {
            List<Book> allBooks = _libraryService.GetAllBooks();

            foreach (var book in allBooks)
            {
                Console.WriteLine($"{book.UniqCode} | {book.Title} | {book.Author} | {book.BookStatus.ToUkrainian()}");
            }
        }

        private void ShowAvailableBooks()
        {
            List<Book> availableBook = _libraryService.GetAvailableBooks();

            foreach(var book in availableBook)
            {
                Console.WriteLine($"{book.UniqCode} | {book.Title} | {book.Author} | {book.BookStatus.ToUkrainian()}");
            }
        }

        private void AddBook()
        {
            Console.WriteLine("Для створення нової книги введіть необхідні параметри:");

            string title;
            do
            {
                Console.Write("Назва книги: ");
                title = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(title))
                    Console.WriteLine("Назва книги не може бути порожньою!");
            } while (string.IsNullOrWhiteSpace(title));

            string author;
            do
            {
                Console.Write("Автор книги: ");
                author = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(author))
                    Console.WriteLine("Автор не може бути порожнім!");
            } while (string.IsNullOrWhiteSpace(author));

            DateOnly releaseYear;
            while (true)
            {
                Console.Write("Рік випуску (формат YYYY): ");
                string? input = Console.ReadLine()?.Trim();

                if (int.TryParse(input, out int yearInt) && yearInt >= 1 && yearInt <= DateTime.Now.Year)
                {
                    releaseYear = new DateOnly(yearInt, 1, 1); 
                    break;
                }
                Console.WriteLine("Некоректний рік. Спробуйте ще раз.");
            }

            int uniqCode = _libraryService.GetNextUniqCode();

            var newBook = new Book
            {
                UniqCode = uniqCode,
                Title = title,
                Author = author,
                ReleaseYear = releaseYear,
                BookStatus = BookStatus.Available
            };

            bool result = _libraryService.AddBook(newBook);

            if (result)
            {
                Console.WriteLine($"Книгу '{newBook.Title}' успішно додано з кодом {newBook.UniqCode}!");
            }
            else
            {
                Console.WriteLine("Книга з таким унікальним кодом вже існує або невірно вказані параметри. Не вдалося додати.");
            }

        }

        private void BorrowBook()
        {
            Console.WriteLine("Взяти книгу у користування:");

            int uniqCode;
            while (true)
            {
                Console.Write("Введіть унікальний код книги: ");
                string? input = Console.ReadLine()?.Trim();

                if (int.TryParse(input, out uniqCode))
                    break;

                Console.WriteLine("Некоректний код. Спробуйте ще раз.");
            }

            bool isBorrowed = _libraryService.BorrowBook(uniqCode);

            if (!isBorrowed)
            {
                Console.WriteLine($"Книгу з кодом {uniqCode} не знайдено або книга вже зайнята");
                return;
            }

            Console.WriteLine($"Ви успішно взяли книгу на користування!");

        }

        private void ReturnBook()
        {
            Console.WriteLine("Повернення книги:");

            int uniqCode;
            while (true)
            {
                Console.Write("Введіть унікальний код книги, яку повертаєте: ");
                string? input = Console.ReadLine()?.Trim();

                if (int.TryParse(input, out uniqCode))
                    break;

                Console.WriteLine("Некоректний код. Спробуйте ще раз.");
            }

            bool isReturned = _libraryService.ReturnBook(uniqCode);

            if (!isReturned)
            {
                Console.WriteLine($"Книгу з кодом {uniqCode} не знайдено або вже доступна. Повернення не потрібно.");
                return;
            }

            Console.WriteLine($"Ви успішно повернули книгу!");

        }

        private void DeleteBook()
        {
            Console.WriteLine("Видалення книги з бібліотеки:");

            int uniqCode;
            while (true)
            {
                Console.Write("Введіть унікальний код книги для видалення: ");
                string? input = Console.ReadLine()?.Trim();

                if (int.TryParse(input, out uniqCode))
                    break;

                Console.WriteLine("Некоректний код. Спробуйте ще раз.");
            }

            bool result = _libraryService.DeleteBook(uniqCode);

            if (result)
                Console.WriteLine($"Книга з кодом {uniqCode} успішно видалена з бібліотеки!");
            else
                Console.WriteLine($"Книга з кодом {uniqCode} не знайдена. Видалення не виконано.");
        }

        private void SearchBooks()
        {
            Console.WriteLine("Пошук книг (залиште порожнім для пропуску):");

            Console.Write("Назва книги: ");
            string? title = Console.ReadLine()?.Trim();
            Console.Write("Автор книги: ");
            string? author = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("Введіть назву або автора для пошуку.");
                return;
            }

            List<Book> results = _libraryService.SearchBooks(title, author);

            if (results.Count == 0)
            {
                Console.WriteLine("Книги не знайдено.");
                return;
            }

            Console.WriteLine($"Знайдено {results.Count} книг(и):");
            foreach (var book in results)
            {
                Console.WriteLine($"{book.UniqCode} | {book.Title} | {book.Author} | {book.BookStatus.ToUkrainian()}");
            }
        }
    }
}
