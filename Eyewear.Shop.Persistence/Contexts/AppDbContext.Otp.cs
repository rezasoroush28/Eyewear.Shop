using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Eyewear.Shop.Persistence.Contexts
{

    public partial class AppDbContext : IOtpRepository
    {

        public async Task AddOtpAsync(OtpCode otp)
        {
            await OtpCodes.AddAsync(otp);
        }

        public async Task<OtpCode> GetUserLastOtpAsync(string phoneNumber, string code)
        {
            return await OtpCodes.AsNoTracking().Where(o => o.PhoneNumber == phoneNumber &&
             o.Code == code && o.Expiration > DateTime.Now).
             OrderByDescending(o => o.Id).FirstOrDefaultAsync();
        }
    }

}
