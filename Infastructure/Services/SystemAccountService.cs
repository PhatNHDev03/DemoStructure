using Application.DTOs;
using Application.IRepositories;
using Application.IServices;
using Azure.Core;
using Domain;
using Microsoft.AspNetCore.Identity;
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

namespace Infastructure.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IQueryUnitOfWork _queryUnitOfWork;
        private readonly ICommandUnitOfWork _commandUnitOfWork;
        private readonly string _secretKey;
        public SystemAccountService(IConfiguration configuration, IQueryUnitOfWork queryUnitOfWork, ICommandUnitOfWork commandUnitOfWork  )
        {
            _secretKey = configuration.GetValue<string>("JWT:Secret");

            _queryUnitOfWork = queryUnitOfWork;
            _commandUnitOfWork = commandUnitOfWork;
        }
        public async Task<LoginResponseDTO> Login(LoginRequest login)
        {

            var user = await _queryUnitOfWork.Repository<SystemAccount>().GetByCondition(filter: x => x.Username == login.userName);
            var passwordHasher = new PasswordHasher<SystemAccount>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash,login.password);
            if (result == PasswordVerificationResult.Failed)
            {
                 throw new ArgumentException("Invalid username or password");
            }
            var expiredTime = DateTime.Now.AddMinutes(30);
            var token = await GenerateToken(user.Username, expiredTime);
            return new LoginResponseDTO
            {
                Token = token,
                ExpiresAt = expiredTime.ToString("HH:mm:ss dd/MM/yyyy")
            };
        }
        public async Task<string> GenerateToken(string userName, DateTime expires)
        {
            var user = await _queryUnitOfWork.Repository<SystemAccount>().GetByCondition(filter:x => x.Username == userName);
            if (user == null) return null;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.AccountId.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<SystemAccountDto> Register(RegisterRequest register)
        {
            try
            {
                using var transaction =  _commandUnitOfWork.BeginTransaction();
                SystemAccount user = new SystemAccount
                {
                    Username = register.Username,   
                    FullName = register.FullName,
                    Email = register.Email,
                    Role = register.Role.ToString(),          
                    CreatedAt = DateTime.Now,
                    Status = "Active"
                };
                var passwordHasher = new PasswordHasher<SystemAccount>();
                user.PasswordHash = passwordHasher.HashPassword(user, register.Password);
                await _commandUnitOfWork.Repository<SystemAccount>().AddAsync(user);
                await _commandUnitOfWork.SaveChangesAsync();
                await _commandUnitOfWork.CommitAsync();
                
                return SystemAccountDto.ConvertToDto(user);

            }
            catch (Exception ex)
            {
                await _commandUnitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
