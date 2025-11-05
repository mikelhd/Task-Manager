using TaskManager.Data.Enum;

namespace TaskManager.Data.Entites
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime DoneDate { get; set; }
        public DateTime DueDate { get; set; }
        public Status State { get; set; }
        public List<Tag> Tags { get; set; }

    }
}
