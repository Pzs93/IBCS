using System.ComponentModel.DataAnnotations;

namespace TodoDAL
{
    public class TodoItem
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public required string Name { get; set; }

        [MaxLength(256)]
        public required string Description { get; set; }

        public byte Priority { get; set; } 

        public DateTime CreatedAt { get; set; }

        public bool IsDone { get; set; }
    }
}