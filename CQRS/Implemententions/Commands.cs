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

namespace LevServer.models.CQRS.Implemententions
{
    public class Commands : ICommands
    {
        private readonly MovieContext _dbContext;

        public Commands(MovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie> UpdateMovieAsync(Movie movie)
        {
            _dbContext.Movies.Update(movie);
            await _dbContext.SaveChangesAsync();
            return movie;
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
        }

    }
}
