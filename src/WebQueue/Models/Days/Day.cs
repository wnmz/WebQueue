using System;
using System.ComponentModel.DataAnnotations;
using WebQueue.Interfaces;

namespace WebQueue.Models
{
    public class Day : BaseEntityModel
    {
        [Key]
        public long Id { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? WorkStartTime { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? WorkEndTime { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? ExactDate { get; set; }
        
        public DayOfWeek? DayOfWeek { get; set; } 
        
        public bool IsWorkTime { get; set; }
    }
}