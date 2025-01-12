using exp.Services.Generic;
using Infrastructure.Repositories.Movies;
using Infrastructure.Repositories.UserMovies;
using Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Helpers;

namespace Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserMoviesRepository _userMovieRepository;
        private readonly IGenericService _genericService;

        public UserService(
            IUserRepository userRepository,
            IMovieRepository movieRepository,
            IUserMoviesRepository userMovieRepository,
            IGenericService genericService)
        {
            _userRepository = userRepository;
            _movieRepository = movieRepository;
            _userMovieRepository = userMovieRepository;
            _genericService = genericService;
        }

        public async Task<UserViewModel> GetUser(string id)
        {
            var user = await _userRepository.GetAllQuerable().FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User not found!");
            }

            var returnUser = new UserViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Bank = (float)user.Bank
            };

            return returnUser;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            var users = _userRepository.GetAllQuerable();

            var usersList = await users.Select(x => new UserViewModel
            {
                Id = x.Id,
                Name = x.UserName,
                Email = x.Email,
                Bank = (float)x.Bank
            }).ToListAsync();

            return usersList;
        }

        public async Task<List<MovieViewModel>> GetUserMovies(string userId)
        {
            var user = await _userRepository.GetAllQuerable().FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found!");
            }

            var movies = _userMovieRepository.GetAllQuerable().Where(x => x.UserId == userId).Select(x => x.Movie);

            var moviesList = movies.Select(x => new MovieViewModel
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
            }).ToList();

            return moviesList;
        }

        public async Task AddToBank(float amount, string id)
        {
            var user = await _userRepository.GetAllQuerable().FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User not found!");
            }


            user.Bank = user.Bank + (decimal)amount;

            await _userRepository.Update(user);
        }

        public async Task BuyMovie(string userId, int movieId)
        {
            var user = await _userRepository.GetAllQuerable().FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found!");
            }

            var movie = await _movieRepository.GetAsync(movieId);

            if (movie == null)
            {
                throw new NotFoundException("Movie not found!");
            }

            if (user.Bank < movie.StartPrice + movie.ViewsValue * (movie.Views / movie.ViewsThreshold))
            {
                throw new NotFoundException("Insuficient bank to buy");
            }

            user.Bank -= movie.StartPrice + movie.ViewsValue * (movie.Views / movie.ViewsThreshold);

            await _userRepository.Update(user);
            await _userMovieRepository.Add(new Infrastructure.Entities.UserMovie()
            {
                UserId = userId,
                MovieId = movieId
            });
        }

        public async Task SellMovie(string userId, int movieId)
        {
            var user = await _userRepository.GetAllQuerable().FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found!");
            }

            var movie = await _movieRepository.GetAsync(movieId);

            if (movie == null)
            {
                throw new NotFoundException("Movie not found!");
            }

            var userMovie = await _userMovieRepository.GetAllQuerable().FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);

            if (userMovie == null)
            {
                throw new NotFoundException("User did not have this movie!");
            }

            userMovie.IsDeleted = true;
            user.Bank += movie.StartPrice + movie.ViewsValue * (movie.Views / movie.ViewsThreshold);

            await _userRepository.Update(user);
            await _userMovieRepository.Update(userMovie);
        }
    }
}
