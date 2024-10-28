using Infrastructure.Entities;
using Infrastructure.Repositories.Movies;
using NetflixRemake.Models;

namespace Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task AddMovie(CreateMovieViewModel model)
        {
            var movie = new Movie()
            {
                Name = model.Name
            };

            await _movieRepository.Add(movie);
        }
    }
}
