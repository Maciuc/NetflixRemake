using exp.Services.Generic;
using Infrastructure.Entities;
using Infrastructure.Repositories.Movies;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Helpers;

namespace Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenericService _genericService;

        public MovieService(
            IMovieRepository movieRepository,
            IGenericService genericService)
        {
            _movieRepository = movieRepository;
            _genericService = genericService;
        }

        public async Task<MovieViewModel> GetMovie(int id)
        {
            var movie = await _movieRepository.GetAsync(id);

            if (movie == null)
            {
                throw new NotFoundException("Movie not found!");
            }

            var returnMovie = new MovieViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                VideoPath = _genericService.CreateImageUrl(movie.VideoPath),
                StartPrice = movie.StartPrice,
                CurrentPrice = movie.StartPrice + (float)movie.ViewsValue * (movie.Views / movie.ViewsThreshold),
                Views = movie.Views,
                ViewsValue = (float)movie.ViewsValue,
                ViewsThreshold = movie.ViewsThreshold
            };

            return returnMovie;
        }

        public async Task<List<MovieViewModel>> GetMovies()
        {
            var movies = _movieRepository.GetAllQuerable();

            var moviesList = await movies.Select(x => new MovieViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                VideoPath = _genericService.CreateImageUrl(x.VideoPath),
                StartPrice = x.StartPrice,
                CurrentPrice = x.StartPrice + (float)x.ViewsValue * (x.Views / x.ViewsThreshold),
                Views = x.Views,
                ViewsValue = (float)x.ViewsValue,
                ViewsThreshold = x.ViewsThreshold
            }).ToListAsync();

            return moviesList;
        }

        public async Task AddMovie(CreateMovieViewModel model)
        {
            var movie = new Movie()
            {
                Name = model.Name,
                Description = model.Description,
                VideoPath = _genericService.CreateImagePath(model.ImageBase64, null, "Movies"),
                StartPrice = model.StartPrice,
                ViewsValue = (decimal)model.ViewsValue,
                ViewsThreshold = model.ViewsThreshold
            };

            await _movieRepository.Add(movie);
        }

        public async Task UpdateMovie(UpdateMovieViewModel model)
        {
            var movie = await _movieRepository.GetAsync(model.Id);

            if (movie == null)
            {
                throw new NotFoundException("Movie not found!");
            }

            movie.Name = model.Name;
            movie.Description = model.Description;
            movie.StartPrice = model.StartPrice;
            movie.VideoPath = _genericService.UpdateImagePath(model.ImageBase64, movie.VideoPath, "Movie");
            movie.ViewsValue = (decimal)model.ViewsValue;
            movie.ViewsThreshold = model.ViewsThreshold;

            await _movieRepository.Update(movie);
        }

        public async Task AddView(int id)
        {
            var movie = await _movieRepository.GetAsync(id);

            if (movie == null)
            {
                throw new NotFoundException("Movie not found!");
            }

            movie.Views += 1;

            await _movieRepository.Update(movie);
        }

        /*public async Task UpdateViewsValueOfMovie(UpdateViewsValueViewModel model)
        {
            var movie = await _movieRepository.GetAsync(model.MovieId);

            if (movie == null)
            {
                throw new NotFoundException("Movie not found!");
            }

            var viewsValue = await _viewsValueRepository.GetAllQuerable().FirstOrDefaultAsync(x => x.MovieId == model.MovieId);

            if (viewsValue == null)
            {
                throw new NotFoundException("Views value not found!");
            }

            viewsValue.Value = (decimal)model.Value;
            viewsValue.ViewsThreshold = model.ViewsThreshold;

            await _viewsValueRepository.Update(viewsValue);
        }*/

        public async Task DeleteMovie(int id)
        {
            var movie = await _movieRepository.GetAsync(id);

            if (movie == null)
            {
                throw new NotFoundException("Movie not found!");
            }

            movie.IsDeleted = true;
            await _movieRepository.Update(movie);
        }

        public async Task PhysicalDeleteMovie(int id)
        {
            var movie = await _movieRepository.GetAsync(id);

            if (movie == null)
            {
                throw new NotFoundException("Movie not found!");
            }

            await _movieRepository.DeleteAsync(movie);
        }
    }
}
