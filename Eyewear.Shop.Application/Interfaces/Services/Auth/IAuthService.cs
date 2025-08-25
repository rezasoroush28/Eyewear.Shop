using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<Result> RequestOtpAsync(string phoneNumber, CancellationToken cancellationToken);
        Task<Result<string>> VerifyOtpAsync(string phoneNumber, string code, CancellationToken cancellationToken);
        Task<Result> CompleteProfileAsync(Guid userId, string name, string email, CancellationToken cancellationToken);
    }
}
