using Application.Ult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Application.DTOs;
namespace Application.IServices
{
    public interface IClassService
    {
       public Task<PagedResult<ClassDto>> GetAll(int pageSize = 0,int pageNumber = 1);
        public Task<ClassDetailDto> GetByIdAsync(int id);
        public Task<ClassDto> Create(ClassRequest item);
        public Task<ClassDto> Update(int id , ClassRequest classDto);
        public Task<bool> DeleteByIdAsync(int id);
    }
}
