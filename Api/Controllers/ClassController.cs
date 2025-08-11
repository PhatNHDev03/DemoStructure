using Application.DTOs;
using Application.IServices;
using Application.Ult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IServiceAggregator _serviceAggregator;
        public ClassController(IServiceAggregator serviceAggregator)
        {
            _serviceAggregator = serviceAggregator;
        }

        [HttpGet]

        public async Task<ActionResult<ApiResponseWithPagination<List<ClassDto>>>> GetAll([FromQuery]int pageSize, [FromQuery]int pageNumber)
        {
            var result = await _serviceAggregator.ClassService.GetAll(pageSize: pageSize, pageNumber: pageNumber);
            return Ok(new ApiResponseWithPagination<List<ClassDto>>
            {
                Status = Constant.SUCCESS_READ_CODE,
                Message = Constant.SUCCESS_READ_MSG,
                Data = result.Items.ToList(),
                pagination = new Pagination
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = result.TotalItems
                }
            }
            );
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<ClassDetailDto>>> GetById(int id)
        {
            var result = await _serviceAggregator.ClassService.GetByIdAsync(id);
            return Ok(new ApiResponse<ClassDetailDto>
            {
                Status = Constant.SUCCESS_READ_CODE,
                Message = Constant.SUCCESS_READ_MSG,
                Data = result

            }
            );
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ClassDto>>> Create([FromBody] ClassRequest classRequest)
        {
            var result = await _serviceAggregator.ClassService.Create(classRequest);

            return Ok(new ApiResponse<ClassDto>
            {
                Status = Constant.SUCCESS_CREATE_CODE,
                Message = Constant.SUCCESS_CREATE_MSG,
                Data = result
            }
            );
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<ClassDto>>> Update(int id , [FromBody] ClassRequest classRequest)
        {
            var result = await _serviceAggregator.ClassService.Update(id,classRequest);

            return Ok(new ApiResponse<ClassDto>
            {
                Status = Constant.SUCCESS_UPDATE_CODE,
                Message = Constant.SUCCESS_UPDATE_MSG,
                Data = result
            }
            );
        }
        
    }
}
