using Context.Entities;
using Context.Repositories.Generic;

namespace Context.Repositories.Movies
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
    }
}
