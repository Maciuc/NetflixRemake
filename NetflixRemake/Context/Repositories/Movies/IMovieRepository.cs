using Infrastructure.Entities;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.Movies
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
    }
}
