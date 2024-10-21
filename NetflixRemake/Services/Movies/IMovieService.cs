namespace Services.Movies
{
    public interface IMovieService
    {
        Task AddPlayer(CreateMovieViewModel model);
    }
}
