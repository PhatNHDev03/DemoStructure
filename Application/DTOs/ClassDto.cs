
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ClassDto
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public string ClassCode { get; set; }

        public string TeacherName { get; set; }

        public string RoomNumber { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? MaxStudents { get; set; }
        public static ClassDto ConvertToClassDto(Class item)
        {
            return new ClassDto
            {
                ClassId = item.ClassId
                ,
                ClassName = item.ClassName,
                ClassCode = item.ClassCode,
                TeacherName = item.TeacherName,
                RoomNumber = item.RoomNumber,
                MaxStudents = item.MaxStudents
            };
        }
        public static Class ConvertToClass(ClassDto item)
        {
            return new Class
            {
                ClassId = item.ClassId,
                ClassName = item.ClassName,
                ClassCode = item.ClassCode,
                TeacherName = item.TeacherName,
                RoomNumber = item.RoomNumber,
                MaxStudents = item.MaxStudents
            };
        }
    }
    public class ClassDetailDto
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public string ClassCode { get; set; }

        public string TeacherName { get; set; }

        public string RoomNumber { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? MaxStudents { get; set; }
        public ICollection<StudentDto>? Students { get; set; }
        public static ClassDetailDto ConvertToDto(Class item)
        {
            return new ClassDetailDto
            {
                ClassId = item.ClassId
                ,
                ClassName = item.ClassName,
                ClassCode = item.ClassCode,
                TeacherName = item.TeacherName,
                RoomNumber = item.RoomNumber,
                MaxStudents = item.MaxStudents,
                Students = item.Students != null ? (item.Students.Select(x => StudentDto.ConverToDto(x)).ToList()) : null
            };
        }
    }


    public class ClassRequest {

        public string ClassName { get; set; }

        public string ClassCode { get; set; }

        public string TeacherName { get; set; }

        public string RoomNumber { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? MaxStudents { get; set; }
    }
}
