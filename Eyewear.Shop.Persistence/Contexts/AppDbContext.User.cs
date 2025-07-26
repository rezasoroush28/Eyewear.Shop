using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Eyewear.Shop.Persistence.Contexts;

public partial class AppDbContext : IUserRepository
{
    public async Task AddUserAsync(AppUser user)
    {
        await Users.AddAsync(user);
    }

    public async Task<AppUser> GetUserByIdAsync(Guid id)
    {
        return await Users.AsNoTracking().AsSplitQuery().Where(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<AppUser> GetUserByPhoneNumber(string phoneNumber)
    {
        return await Users.AsNoTracking().AsSplitQuery().
            OrderByDescending(u => u.Id).Where(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
    }
}

