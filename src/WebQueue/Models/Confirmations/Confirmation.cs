using System.ComponentModel.DataAnnotations;

namespace WebQueue.Models
{
    public class Confirmation
    {
        [Key]
        public long ConfirmationId { get; set; }
        public string ConfirmationCode { get; set; }
        public virtual QueuePosition Position { get; set; }

    }
}