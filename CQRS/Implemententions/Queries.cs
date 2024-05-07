using LevServer.models;
using LevServer.Services;
using LevServer.Transformations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient.DataClassification;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using LevServer.CQRS.Interfaces;

namespace LevServer.CQRS.Implemententions
{
    public class Queries : IQueries
    {
        private readonly MovieContext _dbContext;
        public Queries(MovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            var movies = await _dbContext.Movies.ToListAsync();
            return movies;
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);
            return movie;
        }

        public async Task<Movie?> GetMovieByImdbIdAsync(string imdbID)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.ImdbID == imdbID);
            return movie;
        }
        

    }
}
