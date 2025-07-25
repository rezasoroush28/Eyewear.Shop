using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Interfaces.Services
{
    public interface ISmsService
    {
        Task<Result> SendAsync(string phoneNumber, string message);
    }
}
