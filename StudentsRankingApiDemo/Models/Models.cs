using System.ComponentModel.DataAnnotations;

namespace StudentsRankingApiDemo.Models
{
    public class StudentPointsSummary
    {
        [Required]
        public int StudentId { get; set; }
        [Required] 
        public string StudentName { get; set; }
        [Required]
        public int Points { get; set; }
    }
}
