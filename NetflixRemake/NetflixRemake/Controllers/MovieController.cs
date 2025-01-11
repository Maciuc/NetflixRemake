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

        [HttpPut("updateMovie")]
        public async Task<IActionResult> UpdateMovie(UpdateMovieViewModel model)
        {
            await _movieService.UpdateMovie(model);
            return Ok("Movie updated successfully!");
        }

        [HttpPut("updateViewsValueOfMovie")]
        public async Task<IActionResult> UpdateViewsValueOfMovie(UpdateViewsValueViewModel model)
        {
            await _movieService.UpdateViewsValueOfMovie(model);
            return Ok("View values updated successfully!");
        }


        [HttpDelete("deleteMovie")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            await _movieService.DeleteMovie(id);
            return Ok("Movie deleted successfully!");
        }

        [HttpDelete("physicalDeleteMovie")]
        public async Task<IActionResult> PhysicalDeleteMovie(int id)
        {
            await _movieService.PhysicalDeleteMovie(id);
            return Ok("Movie deleted successfully!");
        }
    }
}
