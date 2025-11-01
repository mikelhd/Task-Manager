namespace TaskManager.Models
{
    public class ResponseBase
    {
        public ResponseBase(bool isSuccess, string message = " ", int? id = null)
        {
            Id = id;
            Message = message;
            IsSuccess = isSuccess;
        }
        public int? Id { get; set; }
        public bool IsSuccess { get; set; }
        private string message;
        public string Message
        {
            get
            {
                if (message == null)
                {
                    if (IsSuccess is true)
                    {
                        return "عملیات با موفقیت انجام شد";
                    }
                    return "عملیات انجام نشد";
                }
                return message;
            }
            set
            { message = value; }
        }
    }
}
