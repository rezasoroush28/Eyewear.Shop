using Eyewear.Shop.Application.Interfaces.Auth;
using Eyewear.Shop.Application.Interfaces.Repository;
using Eyewear.Shop.Persistence.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Eyewear.Shop.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IOtpRepository otpRepository, IConfiguration configuration, IUserRepository userRepository)
        {
            _otpRepository = otpRepository;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task CompleteProfileAsync(Guid userId, string name, string email)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Name = name;
            user.Email = email;

            await _unitOfWork.SaveChangesAsync();

        }
        public async Task RequestOtpAsync(string phoneNumber)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            var code = new Random().Next(10000, 99999).ToString();
            var otp = new OtpCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                Expiration = DateTime.UtcNow.AddMinutes(2)
            };

            try
            {
                await _otpRepository.AddOtpAsync(otp);
                await _unitOfWork.SaveChangesAsync();

                // external call
                //await _smsSender.SendAsync(phoneNumber, $"Your OTP code is: {code}");

                //await transaction.CommitAsync(); // success
            }

            catch
            {
                await _unitOfWork.RollbackTransaction(transaction); // remove saved OTP
                //add log 
            }
        }
        public async Task<string> VerifyOtpAsync(string phoneNumber, string code)
        {
            var otp = await _otpRepository.GetUserLastOtpAsync(phoneNumber, code);
            if (otp == null)
                throw new Exception("Invalid or expired OTP");

            otp.Used = true;
            
            var user = await _userRepository.GetUserByPhoneNumber(phoneNumber);
            if (user == null)
            {
                user = new AppUser
                {
                    PhoneNumber = phoneNumber,
                    Role = "User"
                };

                await _userRepository.AddUserAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }

            return GenerateToken(user);

        }
        private string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.Role)
            }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }


}
