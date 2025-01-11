using NetflixRemake.Models;

namespace Services.Movies
{
    public interface IMovieService
    {
        Task<MovieViewModel> GetMovie(int id);
        Task<List<MovieViewModel>> GetMovies();
        Task AddMovie(CreateMovieViewModel model);
        Task UpdateMovie(UpdateMovieViewModel model);
        Task UpdateViewsValueOfMovie(UpdateViewsValueViewModel model);
        Task DeleteMovie(int id);
        Task PhysicalDeleteMovie(int id);
    }
}
