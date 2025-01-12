using Infrastructure.Entities;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.Users
{
    public interface IUserRepository : IGenericRepository<AspNetUser>
    {
    }
}
