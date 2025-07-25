using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Eyewear.Shop.Application.Interfaces.Services;
using Eyewear.Shop.Application.Interfaces.Services.Auth;
using Eyewear.Shop.Persistence.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly ISmsService _smsService;

        public AuthService(IOtpRepository otpRepository, IConfiguration configuration, IUserRepository userRepository, ISmsService smsService)
        {
            _otpRepository = otpRepository;
            _configuration = configuration;
            _userRepository = userRepository;
            _smsService = smsService;
        }

        public async Task<Result> CompleteProfileAsync(Guid userId, string name, string email)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return Result.Failure("user not found", 400);

            user.Name = name;
            user.Email = email;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();

        }
        public async Task<Result> RequestOtpAsync(string phoneNumber)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();

            var code = RandomNumberGenerator.GetInt32(10000, 99999).ToString();
            int expiryMinutes = int.Parse(_configuration["Otp:ExpirationMinutes"]);
            var otp = new OtpCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                Expiration = DateTime.UtcNow.AddMinutes(expiryMinutes)
            };

            try
            {
                await _otpRepository.AddOtpAsync(otp);
                await _unitOfWork.SaveChangesAsync();

                var smsResult = await _smsService.SendAsync(phoneNumber, string.Empty); //fake sms
                if (!smsResult.IsSuccess)
                    throw new Exception(smsResult.ErrorMessage);

                await transaction.CommitAsync();
                return Result.Success();
            }

            catch(Exception ex) 
            {
                await _unitOfWork.RollbackTransaction(transaction); // remove saved OTP
                //add log 
                return Result.Failure(ex.Message, 500);
            }
        }
        public async Task<Result<string>> VerifyOtpAsync(string phoneNumber, string code)
        {
            var otp = await _otpRepository.GetUserLastOtpAsync(phoneNumber, code);
            if (otp == null || otp.Code != code || otp.Used || otp.Expiration < DateTime.UtcNow)
                    return Result<string>.Failure("Invalid or expired OTP", 400);

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


            var token = GenerateToken(user);
            return Result<string>.Success(token);

        }
        private string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secret = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secret))
                throw new Exception("JWT secret is not configured.");
            var key = Encoding.UTF8.GetBytes(secret);

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
