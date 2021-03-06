using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIDevelopmentChallenge.Models
{
    /// <summary>
    /// Model to represent LabResult items
    /// </summary>
    public class LabResult
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string TestType { get; set; }
        [Required]
        [MaxLength(20)]
        public string Result { get; set; }
        [Required]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        [Required]
        public DateTime TimeOfTest { get; set; }
        [Required]
        public DateTime EnteredTime { get; set; }
        [Required]
        [MaxLength(255)]
        public string LabName { get; set; }
        [MaxLength(255)]
        public string OrderedByProvider { get; set; }
        public decimal? Measurement { get; set; }
        public string MeasurementUnit { get; set; }
        public LabResult()
        {
            EnteredTime = DateTime.Now;
        }
    }
}
