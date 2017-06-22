using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_Net
{
    public class Library
    {
        string connectionString;
        List<Book> books = new List<Book>();
        public Library(string connection)
        {
            this.connectionString = connection;
        }

        public void InsertBook(string name, string author, string publisher, int year)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                string sqlExpression = $"INSERT INTO Books (Name,Author,Publisher,Year) " +
                    $"VALUES ('{name}', '{author}', '{publisher}', {year})";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                Console.WriteLine($"Добавлено объектов: {number}");

            }
        }

        public void DeleteBook(string name)
        {
            string sqlExpression = $"DELETE  FROM Books WHERE Name='{name}'";

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                Console.WriteLine($"Удалено объектов: {number}");
            }
        }

        public void UpdateBook(string oldname, string newname, string author, string publisher, int year)
        {
            string sqlExpression = $"UPDATE Books" +
                $"SET Name = {newname}, Author = {author}, Publisher = {publisher}, Year = {year}" +
                $"WHERE Name={oldname}";
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                Console.WriteLine($"Обновлено объектов: {number}");
            }
        }

        public void GetAllBooksOfUser(string name)
        {
            string sqlExpression = $"SELECT * FROM Books b" +
                 $"JOIN Users u ON b.UserID = u.Id" +
                 $"WHERE u.Name = {name}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var book = new Book();
                    book.Name = reader.GetString(0);
                    book.Author = reader.GetString(1);
                    book.Publisher = reader.GetString(2);
                    book.Year = reader.GetInt32(3);
                    books.Add(book);
                }
            }
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Name} - {book.Author} - {book.Publisher} - {book.Year}");
            }
        }
    }
}
