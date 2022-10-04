using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using WebQueue.Interfaces;

namespace WebQueue.Models
{
    public class QueuePosition : BaseEntityModel, IQueuePosition
    {
        [Key]
        public long QueueId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public bool IsConfirmed { get; set; }
        
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Confirmation> Confirmations { get; set; }
    }
}