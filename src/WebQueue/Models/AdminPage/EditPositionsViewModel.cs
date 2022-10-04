using System;

namespace WebQueue.Models.AdminPage
{
    public class EditPositionsViewModel
    {
        public long PositionId { get; set; }
        public string Email { get; set; }
        public DateTime Time { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}