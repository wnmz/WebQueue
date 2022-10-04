using System;
using WebQueue.Interfaces;

namespace WebQueue.Models
{
    public class BaseEntityModel : IBaseEntityModelInterface
    {
        //TODO: Default time
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}