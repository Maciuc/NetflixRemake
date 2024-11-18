using Microsoft.AspNetCore.Mvc;
using NetflixRemake.Models;
using NetflixRemake.Models.Helpers;
using Services.Movies;

namespace NetflixRemake.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetMovies();
            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var movie = await _movieService.GetMovie(id);
                return View(movie);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _movieService.AddMovie(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var movie = await _movieService.GetMovie(id);
                var editModel = new UpdateMovieViewModel
                {
                    Name = movie.Name,
                    Description = movie.Description,
                    StartPrice = movie.StartPrice,
                    VideoPath = movie.VideoPath
                };
                return View(editModel);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateMovieViewModel model)
        {
            if (ModelState.IsValid)
            {

                await _movieService.UpdateMovie(id, model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var movie = await _movieService.GetMovie(id);
                return View(movie);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieService.DeleteMovie(id);
            return RedirectToAction(nameof(Index)); 
        }
    }
}
