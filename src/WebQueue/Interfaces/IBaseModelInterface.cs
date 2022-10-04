using System;
using System.ComponentModel.DataAnnotations;

namespace WebQueue.Interfaces
{
    public interface IBaseEntityModelInterface
    {
        [DataType(DataType.Date)]
        DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        DateTime UpdatedAt { get; set; }

        Boolean IsDeleted { get; set; }
    }
}
