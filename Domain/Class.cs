using System;
using System.Collections.Generic;

namespace Domain;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public string ClassCode { get; set; } = null!;

    public string? TeacherName { get; set; }

    public string? RoomNumber { get; set; }

    public int? MaxStudents { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
