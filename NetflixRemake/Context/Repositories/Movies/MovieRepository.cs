using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.Movies
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public NetflixRemakeContext _netflixContext { get; set; }
        public MovieRepository(NetflixRemakeContext context) : base(context)
        {
            _netflixContext = context;
        }

    }
}
