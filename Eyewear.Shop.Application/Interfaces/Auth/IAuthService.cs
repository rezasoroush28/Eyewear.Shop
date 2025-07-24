using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task RequestOtpAsync(string phoneNumber);
        Task<string> VerifyOtpAsync(string phoneNumber, string code);
        Task CompleteProfileAsync(Guid userId, string name, string email);
    }
}
