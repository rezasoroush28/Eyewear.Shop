using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Interfaces.Persistance.Repository
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(Guid id);
        Task<AppUser> GetUserByPhoneNumber(string phoneNumber);
        Task AddUserAsync(AppUser user);
    }
}
