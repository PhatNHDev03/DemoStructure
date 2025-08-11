using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface ISystemAccountService
    {
        Task<LoginResponseDTO> Login(LoginRequest login);
        Task<SystemAccountDto> Register(RegisterRequest register);
    }
}
