using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.Users
{
    public class UserRepository : GenericRepository<AspNetUser>, IUserRepository
    {
        public NetflixRemakeContext _netflixContext { get; set; }
        public UserRepository(NetflixRemakeContext context) : base(context)
        {
            _netflixContext = context;
        }

    }
}
