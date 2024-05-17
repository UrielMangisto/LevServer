using LevServer.models;
using LevServer.Transformations;
using LevServer.CQRS.Implemententions;
using LevServer.CQRS.Interfaces;
using LevServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient.DataClassification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.Http;
using System.Windows.Input;
using LevServer.models.CQRS.Implemententions;

namespace LevServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieContext _dbContext;
        private readonly ICommands _commands;
        private readonly IQueries _queries;

        public MovieController(MovieContext dbContext)
        {
            _dbContext = dbContext;
            _commands = new Commands(_dbContext);
            _queries = new Queries(_dbContext);
        }

        //GET: api/Movie (read all)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return Ok(await _queries.GetMoviesAsync());
        }

        //GET: api/Movie/5 (read)
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _queries.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        //GET: api/Movie/search/tt0050083 (read)
        [HttpGet("search/{imdbID}")]
        public async Task<ActionResult<Movie>> GetMovie(string imdbID)
        {
            var dbmovie = await _queries.GetMovieByImdbIdAsync(imdbID);
            if (dbmovie == null)
            {
                var movie = await Services.OmdbApiService.GetMovieByIDAsync(imdbID);

                if (movie == null)
                {
                    return BadRequest();
                }
                var convertor = new JsonToMovie();
                var movieDto = convertor.GetMovieFromJson(movie);
                return Ok(movieDto);
            }
            return Ok(dbmovie);
        }

        // GET - /api/Movies/search/?s=supermab&y=2016 (read)
        [HttpGet("search")]
        public async Task<ActionResult<Movie>> GetMovie(string s, int? y = null)
        {
            var movies = await Services.OmdbApiService.GetMoviesBySearchAsync(s, y);
            var convertor = new JsonToMovie();
            var movieDtoLst = convertor.GetMovieObjFromJson(movies);
            return Ok(movieDtoLst);
        }
        
        //POST: api/Movie (create)
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(string imdbID)
        {
            var existingMovie = await _queries.GetMovieByImdbIdAsync(imdbID);

            if (existingMovie != null)
            {
                return Ok(existingMovie);
            }

            var movie = await Services.OmdbApiService.GetMovieByIDAsync(imdbID);

            if (movie == null)
            {
                return BadRequest();
            }

            var convertor = new JsonToMovie();
            var movieDto = convertor.GetMovieFromJson(movie);

            if (movieDto == null) 
            {
                return BadRequest();
            }

            await _commands.AddMovieAsync(movieDto);

            return Ok(movieDto);
        }        

        //DELETE: api/Movie/5 (delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _queries.GetMovieByIdAsync(id);
            if(movie == null)
            {
                return NotFound();
            }
            await _commands.DeleteMovieAsync(movie);
            return NoContent();
        }
        //PUT: api/Movie/5 (update)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }
            await _commands.UpdateMovieAsync(movie);
            return NoContent();
        }
    }
}
