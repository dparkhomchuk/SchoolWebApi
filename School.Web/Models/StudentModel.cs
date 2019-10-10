using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class StudentModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public int Age { get; set; }
    }
}
