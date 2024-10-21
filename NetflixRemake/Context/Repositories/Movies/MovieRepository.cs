using Context.Context;
using Context.Entities;
using Context.Repositories.Generic;

namespace Context.Repositories.Movies
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
