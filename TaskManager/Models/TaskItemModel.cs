using TaskManager.Data.Enum;

namespace TaskManager.Models.TaskItem
{
    public class GetTaskItem
    {
        public class GetTaskItemResponse
        {
            public int Id { get; set; }
            public Status State { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime DoneDate { get; set; }
        }
        public class GetTaskItemRequest
        {
            public int Id { get; set; }
            public Status State { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime DoneDate { get; set; }
        }
        public class SaveTaskItemRequest
        {
            public int? Id { get; set; }
            public Status State { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime DoneDate { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
        }
    }
}
