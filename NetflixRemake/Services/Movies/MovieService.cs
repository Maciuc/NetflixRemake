using Infrastructure.Entities;
using Infrastructure.Repositories.Movies;
using Microsoft.EntityFrameworkCore;
using NetflixRemake.Models;
using NetflixRemake.Models.Helpers;

namespace Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
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
                StartPrice = (int)model.StartPrice,
            };

            await _movieRepository.Add(movie);
        }

        public async Task UpdateMovie(int id, CreateMovieViewModel model)
        {
            var movie = await _movieRepository.GetAsync(id);

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

        public async Task DeleteMovie(int id)
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
