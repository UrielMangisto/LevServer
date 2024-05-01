using LevServer.models;
using LevServer.Transformations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.Data.SqlClient.DataClassification;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using LevServer.Services;

namespace LevServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieContext _dbContext;
        private readonly HttpClient _httpClient;

        public MovieController(MovieContext dbContext)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
        }

        //GET: api/Movie (read all)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if(_dbContext.Movies == null)
            {
                return NotFound();
            }
            //return await _dbContext.Movies.ToListAsync();
            return Ok(await _dbContext.Movies.ToListAsync());
        }

        //GET: api/Movie/5 (read)
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        //GET: api/Movie/search/tt0050083 (read)
        [HttpGet("search/{imdbID}")]
        public async Task<ActionResult<Movie>> GetMovie(string imdbID)
        {
            var movie = await Services.OmdbApiService.GetMovieByIDAsync(imdbID);

            if (movie == null)
            {
                return BadRequest();
            }
            var convertor = new JsonToMovie();
            var movieDto = convertor.GetMovieFromJson(movie);
            //return Ok(movie);
            return Ok(movieDto);
        }

        // GET - /api/Movies/search/?s=[SEARCH_TERM]&y=[YEAR] (read)
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
            // check if the movie is already in the DB
            var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.ImdbID == imdbID);

            if (existingMovie != null)
            {
                return Ok(existingMovie);
            }

            var movie = await Services.OmdbApiService.GetMovieByIDAsync(imdbID);

            if (movie == null)
            {
                return BadRequest();
            }

            // TODO: JsonToMovie
            var convertor = new JsonToMovie();
            var movieDto = convertor.GetMovieFromJson(movie);
            _dbContext.Movies.Add(movieDto);
            await _dbContext.SaveChangesAsync();

            return Ok(movieDto);
        }

        ////PUT: api/Movie/5 (update)
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMovie (int id, Movie movie)
        //{
        //    if(id != movie.Id)
        //    {
        //        return BadRequest();
        //    }
        //    _dbContext.Entry(movie).State = EntityState.Modified;

        //    try
        //    {
        //        await _dbContext.SaveChangesAsync();
        //    }catch (DbUpdateConcurrencyException) 
        //    {
        //        if (!MovieExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return NoContent();
        //}
        

        //DELETE: api/Movie/5 (delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if(_dbContext.Movies == null)
            {
                return NotFound();
            }
            var movie = await _dbContext.Movies.FindAsync(id);
            if(movie == null)
            {
                return NotFound();
            }
            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return (_dbContext.Movies?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        
    }
}
