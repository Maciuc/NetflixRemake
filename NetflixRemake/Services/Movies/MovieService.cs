using Infrastructure.Entities;
using Infrastructure.Repositories.Movies;
using Infrastructure.Repositories.ViewsValueRepository;
using Microsoft.EntityFrameworkCore;
using NetflixRemake.Models;
using NetflixRemake.Models.Helpers;

namespace Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IViewsValueRepository _viewsValueRepository;

        public MovieService(IMovieRepository movieRepository,
            IViewsValueRepository viewsValueRepository)
        {
            _movieRepository = movieRepository;
            _viewsValueRepository = viewsValueRepository;
        }

        public async Task<MovieViewModel> GetMovie(int id)
        {
            var movie = await _movieRepository.GetAsync(id);

            if (movie == null)
            {
                throw new NotFoundException("Player not found!");
            }

            var returnMovie = new MovieViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                VideoPath = movie.VideoPath,
                StartPrice = movie.StartPrice
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
                VideoPath = x.VideoPath,
                StartPrice = x.StartPrice
            }).ToListAsync();

            return moviesList;
        }

        public async Task AddMovie(CreateMovieViewModel model)
        {
            var movie = new Movie()
            {
                Name = model.Name,
                Description = model.Description,
                VideoPath = model.VideoPath,
                StartPrice = model.StartPrice
            };

            await _movieRepository.Add(movie);

            //We add views value by default (the 100th part of start price for 1000 views)
            var viewsValue = new ViewsValue()
            {
                MovieId = movie.Id,
                Value = model.StartPrice / 100,
                ViewsThreshold = 1000
            };

            await _viewsValueRepository.Add(viewsValue);
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
            movie.StartPrice = (int)model.StartPrice;
            movie.VideoPath = model.VideoPath;

            await _movieRepository.Update(movie);
        }

        public async Task UpdateViewsValueOfMovie(UpdateViewsValueViewModel model)
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
        }

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
