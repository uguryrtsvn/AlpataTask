using AlpataBLL.Services.Abstracts;
using AlpataBLL.Services.Base;
using AlpataDAL.IRepositories;
using AlpataEntities.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.Concretes
{
    public class UserService : BaseService<AppUser>, IUserService
    {
        readonly IAppUserRepository _userRepository;
        public UserService(IAppUserRepository entityRepository) : base(entityRepository)
        {
            _userRepository = entityRepository;
        }

    }
}
