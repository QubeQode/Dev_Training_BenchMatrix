using LibraryManagementTask.Models;

namespace LibraryManagementTask.Services;

public interface IBookService
{
    IEnumerable<Book> GetAll();

    Book? GetById(int id);

    Book Create(Book book);

    bool Update(Book book);

    bool Delete(int id);
}