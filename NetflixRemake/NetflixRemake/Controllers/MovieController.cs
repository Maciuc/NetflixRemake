using Microsoft.AspNetCore.Mvc;
using NetflixRemake.Models;
using Services.Movies;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(
            IMovieService movieService,
            ILogger<MovieController> logger)
        {
            _movieService = movieService;
        }

        [HttpGet("getMovie")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovie(id);
            return Ok(movie);
        }

        [HttpGet("getMovies")]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _movieService.GetMovies();
            return Ok(movies);
        }

        [HttpPost("addMovie")]
        public async Task<IActionResult> AddMovie(CreateMovieViewModel model)
        {
            await _movieService.AddMovie(model);
            return Ok("Movie added successfully!");
        }
    }
}
