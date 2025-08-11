using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class StudentDto
    {
        public int StudentId { get; set; }

        public string StudentCode { get; set; }

        public string FullName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }


        public DateTime? EnrollmentDate { get; set; }

        public string Status { get; set; }

        public static StudentDto ConverToDto(Student student)
        {
            return new StudentDto
            {
                StudentId = student.StudentId,
                StudentCode = student.StudentCode,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                Phone = student.Phone,
                Address = student.Address,
                EnrollmentDate = student.EnrollmentDate,
                Status = student.Status,
            };
        }

    }
    public class StudentDetailDto
    {
        public int StudentId { get; set; }

        public string StudentCode { get; set; }

        public string FullName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int? ClassId { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public string Status { get; set; }

        public static StudentDetailDto ConverToDto(Student student)
        {
            return new StudentDetailDto
            {
                StudentId = student.StudentId,
                StudentCode = student.StudentCode,
                FullName = student.FullName,
                ClassId = student.ClassId,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                Phone = student.Phone,
                Address = student.Address,
                EnrollmentDate = student.EnrollmentDate,
                Status = student.Status,
            };
        }

    }
}
