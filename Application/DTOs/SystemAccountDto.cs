using Application.Enums;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SystemAccountDto
    {
        public int AccountId { get; set; }

        public string Username { get; set; }


        public string FullName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }


        public string Status { get; set; }
        public static SystemAccountDto ConvertToDto(SystemAccount systemAccount)
        {
            return new SystemAccountDto
            {
                AccountId = systemAccount.AccountId,
                Username = systemAccount.Username,
                Email = systemAccount.Email,
                FullName = systemAccount.FullName,
                Role = systemAccount.Role,
                Status = systemAccount.Status,
            };
        }
    }

    public class RegisterRequest
    {
        public int AccountId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }


        public string Status { get; set; }
    }
    public class LoginResponseDTO
    {
        public string? Token { get; set; }
        public string ExpiresAt { get; set; }
        
    }






    // tao nhanh 1 class DTO thôi tạo thẳng class cũng được chẳng qua class thì có thể thêm function để tùy biến như convertToDto còn cái này
    // thì ko thể tạo được function --> tạo như nào cũng được
    public record LoginRequest(string userName, string password);

}
