using LibraryManagement.BLL.Interfaces;
using LibraryManagement.BLL.Services;
using LibraryManagement.DAL.Interfaces;
using LibraryManagement.DAL.Repositories;
using LibraryManagement.UI;
using System.Text;

namespace LibraryManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            string filePath = Path.Combine(AppContext.BaseDirectory, "books.json");
            IBookRepository repository = new JsonBookRepository(filePath);
            ILibraryService libraryService = new LibraryService(repository);

            var menu = new MenuPresentationBook(libraryService);
            menu.ShowMenu();
        }
    }
}
