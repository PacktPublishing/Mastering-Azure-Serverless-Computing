using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class BooksResponseModel
    {
        public IEnumerable<BookModel> Books { get; set; }
    }
}
