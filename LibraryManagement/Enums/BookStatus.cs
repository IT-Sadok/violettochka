using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Enums
{
    internal enum BookStatus
    {
        Available,
        Unavailable
    }

    internal static class BookStatusExtensions
    {
        public static string ToUkrainian(this BookStatus status) => status switch
        {
            BookStatus.Available => "доступно",
            BookStatus.Unavailable => "недоступно",
            _ => status.ToString()
        };
    }
}
