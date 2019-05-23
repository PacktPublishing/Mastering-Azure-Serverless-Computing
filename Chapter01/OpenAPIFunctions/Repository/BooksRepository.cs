using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class BooksRepository
    {
        private static IEnumerable<BookModel> books = new[] {
            new BookModel(){  Title="The Great Gatsby",Price=10.5M,Author="F. Scott Fitzgerald"},
            new BookModel(){  Title="The Scarlet Letter",Price=20.0M,Author="Nathaniel Hawthorne"},
            new BookModel(){  Title="The Adventures of Huckleberry Finn",Price=25.0M,Author="Mark Twain"},
            new BookModel(){  Title="Fahrenheit 451",Price=15.0M,Author="Ray Bradbury"},
            new BookModel(){  Title="The Old Man and the Sea",Price=35.5M,Author="Ernest Hemingway"},
        };

        public static IEnumerable<BookModel> Books
        {
            get => books;
        }
    }
}
