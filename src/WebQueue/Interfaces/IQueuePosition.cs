using System;
using System.Collections.Generic;
using WebQueue.Models;

namespace WebQueue.Interfaces
{
    public interface IQueuePosition : IBaseEntityModelInterface
    {
        long QueueId { get; set; }
        DateTime Date { get; set; }
        ApplicationUser User { get; set; }
        ICollection<Confirmation> Confirmations { get; set; }
    }
}
