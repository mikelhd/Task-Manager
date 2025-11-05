using static TaskManager.Models.TaskItem.GetTaskItem;

namespace TaskManager.Models
{
    public class TaskItemViewModel
    {
        public List<GetTaskItemResponse> ActiveTasks { get; set; }
        public List<GetTaskItemResponse> DoneTasks { get; set; }
    }
}
