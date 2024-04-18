using Microsoft.AspNetCore.Mvc;

namespace LevServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

/*
// הממשק של השרת
using LevServer.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetMovies();
    Task<Movie> GetMovie(int id);
    Task<Movie> CreateMovie(Movie movie);
    Task<Movie> UpdateMovie(int id, Movie movie);
    Task<bool> DeleteMovie(int id);
}

// קונטרקט שמייצג את הממשק של השירות
public class MovieService : IMovieService
{
    private readonly MovieContext _dbContext;

    public MovieService(MovieContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Movie>> GetMovies()
    {
        return await _dbContext.Movies.ToListAsync();
    }

    public async Task<Movie> GetMovie(int id)
    {
        return await _dbContext.Movies.FindAsync(id);
    }

    public async Task<Movie> CreateMovie(Movie movie)
    {
        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync();
        return movie;
    }

    public async Task<Movie> UpdateMovie(int id, Movie movie)
    {
        _dbContext.Entry(movie).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return movie;
    }

    public async Task<bool> DeleteMovie(int id)
    {
        var movie = await _dbContext.Movies.FindAsync(id);
        if (movie == null)
        {
            return false;
        }
        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

// הבקר שמקשר את השירות ל-Controller
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        var movies = await _movieService.GetMovies();
        if (movies == null)
        {
            return NotFound();
        }
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _movieService.GetMovie(id);
        if (movie == null)
        {
            return NotFound();
        }
        return Ok(movie);
    }

    [HttpPost]
    public async Task<ActionResult<Movie>> PostMovie(Movie movie)
    {
        var createdMovie = await _movieService.CreateMovie(movie);
        return CreatedAtAction(nameof(GetMovie), new { id = createdMovie.Id }, createdMovie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest();
        }
        await _movieService.UpdateMovie(id, movie);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var result = await _movieService.DeleteMovie(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}*/