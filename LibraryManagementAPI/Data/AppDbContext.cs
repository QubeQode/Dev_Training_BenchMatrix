using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data;

public class AppDbContext : DbContext
{
    /*
        DbContextOptions holds the config data for the db connection:
            - Db provider
            - Connection string
            - Query tracking settings
        Comes from the DI in Program.cs AddDbContext();
    */
    public AppDbContext(
        DbContextOptions<AppDbContext> options
    ): base(options) // Sends options to Parent constructor before construction
    {}

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Author> Authors => Set<Author>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<BookTag> BookTags => Set<BookTag>();

    /*
        Using FluentAPI because use case of composite key has been fulfilled by my
        model structure. If necessary can create a surrogate key in BookTag model.
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Config executes standard build before modification with custom rules
        base.OnModelCreating(modelBuilder);

        // Customized configuration for BookTag composite key
        modelBuilder.Entity<BookTag>()
            .HasKey(bt => new {bt.BookId, bt.TagId});
        
        // Seed Author Data
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, Name = "J.R.R Tolkien" },
            new Author { Id = 2, Name = "Haruki Murakami" },
            new Author { Id = 3, Name = "Elizabeth Moon" }
        );

        // Seed Book Data
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "The Fellowship of the Ring", Year = 1954, PageCount = 423, AuthorId = 1 },
            new Book { Id = 2, Title = "The Two Towers", Year = 1954, PageCount = 352, AuthorId = 1 },
            new Book { Id = 3, Title = "The Return of the King", Year = 1956, PageCount = 416, AuthorId = 1 },
            new Book { Id = 4, Title = "Kafka on the Shore", Year = 2002, PageCount = 480, AuthorId = 2 },
            new Book { Id = 5, Title = "Killing Commendatore", Year = 2017, PageCount = 681, AuthorId = 2 },
            new Book { Id = 6, Title = "Sheepfarmer's Daughter", Year = 1988, PageCount = 416, AuthorId = 3 },
            new Book { Id = 7, Title = "Divided Allegiance", Year = 1988, PageCount = 480, AuthorId = 3 },
            new Book { Id = 8, Title = "Oath of Gold", Year = 1989, PageCount = 480, AuthorId = 3 }
        );

        // Seed Tag Data
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "Fantasy" },
            new Tag { Id = 2, Name = "Surrealism" },
            new Tag { Id = 3, Name = "Classic" },
            new Tag { Id = 4, Name = "Postmodern" }
        );

        // Seed BookTag Data
        modelBuilder.Entity<BookTag>().HasData(
            new BookTag { BookId = 1, TagId = 1 },
            new BookTag { BookId = 1, TagId = 3 },
            new BookTag { BookId = 4, TagId = 2 },
            new BookTag { BookId = 4, TagId = 4 },
            new BookTag { BookId = 6, TagId = 1 },
            new BookTag { BookId = 7, TagId = 1 }
        );
    }
}
