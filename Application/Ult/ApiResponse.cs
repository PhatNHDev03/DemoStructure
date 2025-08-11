using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ult
{
    public class ApiResponse<T>
    {
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }
        public T Data { get; set; }
    }
    public class ApiResponseWithPagination<T>
    {
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }
        public T Data { get; set; }
        public Pagination? pagination { get; set; }
    }
}
