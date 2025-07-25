using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Interfaces.Persistance.Repository
{
    public interface IOtpRepository
    {
        Task<OtpCode> GetUserLastOtpAsync(string phoneNumber, string code);
        Task AddOtpAsync(OtpCode otp);
    }
}
