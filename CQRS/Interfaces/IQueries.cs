using LevServer.models;

namespace LevServer.CQRS.Interfaces
{
    public interface IQueries
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<Movie?> GetMovieByImdbIdAsync(string imdbID);
        Task<Movie?> GetMovieByIdAsync(int id);
    }
}
