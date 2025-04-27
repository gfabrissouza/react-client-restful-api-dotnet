using RestApiDotNet.Data.Converter.Contract;
using RestApiDotNet.Model;

namespace RestApiDotNet.Data.VO
{
    public class BookConverter : IParser<BookVO, Book>, IParser<Book, BookVO>
    {
        public Book Parse(BookVO origin)
        {
            if (origin == null) return null;

            return new Book
            {
                Id = origin.Id,
                Author = origin.Author,
                LaunchDate = origin.LaunchDate,
                Price = origin.Price,
                Title = origin.Title
            };
        }

        public BookVO Parse(Book destination)
        {
            if (destination == null) return null;

            return new BookVO
            {
                Id = destination.Id,
                Author = destination.Author,
                LaunchDate = destination.LaunchDate,
                Title = destination.Title,
                Price = destination.Price
            };
        }

        public List<Book> ParseList(List<BookVO> origin)
        {
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<BookVO> ParseList(List<Book> destination)
        {
            return destination.Select(item => Parse(item)).ToList();
        }
    }
}
