using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ult
{
    // cái này hỗ trỡ cho về controller trả data về cho user nếu cần phân trang 
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public int TotalItems { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((decimal)TotalItems / PageSize) : 1;
    }
    // cái này là data được trả về từ service về cho controller hỗ trợ cho việc phân trang
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
    }
}
