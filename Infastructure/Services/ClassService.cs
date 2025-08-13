using Application.DTOs;
using Application.IEventBus;
using Application.IRepositories;
using Application.IServices;
using Application.Ult;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly IQueryUnitOfWork _queryUnitOfWork;
        private readonly ICommandUnitOfWork _commandUnitOfWork;
        private readonly IMessagePublisher _messagePublisher;
        public ClassService(IQueryUnitOfWork queryUnitOfWork, ICommandUnitOfWork commandUnitOfWork, IMessagePublisher messagePublisher)
        {
            _queryUnitOfWork = queryUnitOfWork;
            _commandUnitOfWork = commandUnitOfWork;
            _messagePublisher = messagePublisher;
        }
        public async Task<ClassDto> Create(ClassRequest item)
        {
            using var transaction = _commandUnitOfWork.BeginTransaction();
            try
            {
                var newClass = new Class
                {
                    ClassName = item.ClassName,
                    ClassCode = item.ClassCode,
                    TeacherName = item.TeacherName,
                    RoomNumber = item.RoomNumber,
                    CreatedDate = item.CreatedDate,
                    MaxStudents = item.MaxStudents
                };
                var result = await _commandUnitOfWork.Repository<Class>().AddAsync(newClass);
                await _commandUnitOfWork.SaveChangesAsync();    
                await _commandUnitOfWork.CommitAsync();
                var dto= ClassDto.ConvertToClassDto(result);
                _= _messagePublisher.PublishAsync("class-created", dto);
                //await _messageConsumer.ConsumeAsync("user-events", "user-service-group", cancellationToken);
                return dto;
            }
            catch (Exception ex) {
                await _commandUnitOfWork.RollbackAsync();
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var existingItem = await _queryUnitOfWork.Repository<Class>().GetByCondition(filter: x => x.ClassId == id,traced:false);
            if (existingItem == null) return false;
           await _commandUnitOfWork.Repository<Class>().Delete(existingItem);
            await _commandUnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<ClassDto>> GetAll(int pageSize = 0, int pageNumber = 1)
        {
          var (items,totaltItems) = await _queryUnitOfWork.Repository<Class>().GetAll(pageSize:pageSize,pageNumber:pageNumber);
            if (items == null || totaltItems == 0) {
                return new PagedResult<ClassDto>
                {
                    Items = null,
                    TotalItems = 0
                };
            }
            return new PagedResult<ClassDto>
            {
                Items = items.Select(x => ClassDto.ConvertToClassDto(x)),
                TotalItems = totaltItems
            };
        }

        public async Task<ClassDetailDto> GetByIdAsync(int id)
        {
            var item = await _queryUnitOfWork.Repository<Class>().GetByCondition(filter: x => x.ClassId == id, includeProperties: "Students");
            return (item == null) ? null : ClassDetailDto.ConvertToDto(item);
        }

        public async Task<ClassDto> Update(int id, ClassRequest item)
        {
            using var transaction = _commandUnitOfWork.BeginTransaction();
            try
            {
                var existingItem = await _queryUnitOfWork.Repository<Class>().GetByCondition(filter: x => x.ClassId == id,traced:false);
                if (existingItem == null) return null;
                existingItem.ClassName = item.ClassName;
                existingItem.ClassCode = item.ClassCode;
                existingItem.TeacherName = item.TeacherName;
                existingItem.RoomNumber = item.RoomNumber;
                existingItem.CreatedDate = item.CreatedDate;
                existingItem.MaxStudents = item.MaxStudents;
           
                var result = await _commandUnitOfWork.Repository<Class>().Update(existingItem);
                await _commandUnitOfWork.SaveChangesAsync();
                await _commandUnitOfWork.CommitAsync();
                return ClassDto.ConvertToClassDto(result);
            }
            catch (Exception ex)
            {
                await _commandUnitOfWork.RollbackAsync();
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
