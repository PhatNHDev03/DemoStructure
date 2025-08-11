using Application.DTOs;
using Application.IServices;
using Application.Ult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceAggregator _serviceAggregator;
        public AuthController(IServiceAggregator serviceAggregator) 
        {
            _serviceAggregator = serviceAggregator;
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody]LoginRequest loginRequest) { 
            var result =await _serviceAggregator.SystemAccountService.Login(loginRequest);
            return Ok(new ApiResponse<LoginResponseDTO> { 
                Status = Constant.SUCCESS_READ_CODE,
                Message = "login successfully",
                Data = result
            });
        }
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<SystemAccountDto>>> Register([FromForm]RegisterRequest request)
        {
            var result = await _serviceAggregator.SystemAccountService.Register(request);
            return Ok(new ApiResponse<SystemAccountDto>
            {
                Status = Constant.SUCCESS_CREATE_CODE,
                Message = "register successfully",
                Data = result
            });
        }
    }
}
