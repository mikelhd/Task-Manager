namespace TaskManager.Models
{
    public class TagModel
    {
        public class GetTagRequest
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }
        public class GetTagResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }
        public class SaveTagRequest
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
