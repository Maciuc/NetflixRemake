using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Repositories.Generic;

namespace Infrastructure.Repositories.ViewsValueRepository
{
    public class ViewsValueRepository : GenericRepository<ViewsValue>, IViewsValueRepository
    {
        public NetflixRemakeContext _netflixContext { get; set; }
        public ViewsValueRepository(NetflixRemakeContext context) : base(context)
        {
            _netflixContext = context;
        }

    }
}
