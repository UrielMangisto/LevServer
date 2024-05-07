using LevServer.models;

namespace LevServer.CQRS.Interfaces
{
    public interface ICommands
    {
        Task<Movie> AddMovieAsync(Movie movie);
        Task<Movie> UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(Movie movie);
    }
}
