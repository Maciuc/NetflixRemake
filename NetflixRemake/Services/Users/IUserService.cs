using Models;

namespace Services.Users
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser(string id);
        Task<List<UserViewModel>> GetUsers();
        Task<List<MovieViewModel>> GetUserMovies(string userId);
        Task AddToBank(float amount, string id);
        Task BuyMovie(string userId, int movieId);
        Task SellMovie(string userId, int movieId);
    }
}
