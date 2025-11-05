namespace TaskManager.Data.Entites
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<TaskItem> TaskItems { get; set; }
    }
}
