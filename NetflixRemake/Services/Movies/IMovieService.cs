using NetflixRemake.Models;

namespace Services.Movies
{
    public interface IMovieService
    {
        Task<MovieViewModel> GetMovie(int id);
        Task<List<MovieViewModel>> GetMovies();
        Task AddMovie(CreateMovieViewModel model);
        Task UpdateMovie(int id, CreateMovieViewModel model);
        Task DeleteMovie(int id);
    }
}
