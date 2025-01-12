using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.UserMovies
{
    public class UserMoviesRepository : GenericRepository<UserMovie>, IUserMoviesRepository
    {
        public NetflixRemakeContext _netflixContext { get; set; }
        public UserMoviesRepository(NetflixRemakeContext context) : base(context)
        {
            _netflixContext = context;
        }

    }
}
