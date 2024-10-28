using NetflixRemake.Models;

namespace Services.Movies
{
    public interface IMovieService
    {
        Task AddMovie(CreateMovieViewModel model);
    }
}
