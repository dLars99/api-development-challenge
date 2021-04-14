using System;
using System.ComponentModel.DataAnnotations;

namespace APIDevelopmentChallenge.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(10)]
        public string SexAtBirth { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Range(10, 120)]
        public int Height { get; set; }
        [Required]
        [Range(1, 1500)]
        public int Weight { get; set; }
        [MaxLength(100)]
        public string InsuranceCompany { get; set; }
        [MaxLength(255)]
        public string MemberId { get; set; }
        [MaxLength(25)]
        public string GroupId { get; set; }
        public bool? IsPolicyHolder { get; set; }
    }
}
