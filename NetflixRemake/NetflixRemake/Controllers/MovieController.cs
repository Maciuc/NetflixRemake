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

        [HttpPost("addMovie")]
        public async Task<IActionResult> AddMovie(CreateMovieViewModel model)
        {
            await _movieService.AddMovie(model);
            return Ok("Movie added successfully!");
        }
    }
}
