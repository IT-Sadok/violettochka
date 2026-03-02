using LibraryManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    internal class Book
    {
        public int UniqCode { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public DateOnly ReleaseYear { get; set; }

        public BookStatus BookStatus { get; set; }

    }
}
