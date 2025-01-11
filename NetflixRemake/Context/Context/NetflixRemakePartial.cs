using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public partial class NetflixRemakeContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
