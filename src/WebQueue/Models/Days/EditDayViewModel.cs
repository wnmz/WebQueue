#nullable enable
using System;
namespace WebQueue.Models
{
    public class EditDayViewModel
    {
        public long? Id { get; set; }
        
        public string? WorkStartTime { get; set; }
        
        public string? WorkEndTime { get; set; }
        
        public DateTime? ExactDate { get; set; }
        
        public DayOfWeek? DayOfWeek { get; set; } 
        
        public bool IsWorkTime { get; set; }
    }
}