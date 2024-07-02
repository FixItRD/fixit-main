namespace fixit_main.Models
{
    public class CrudOperationResponse
    {
        public CrudOperationResponse(bool success, CrudOperation operation, string message, List<object> items = null)
        {
            Success = success;
            Operation = operation;
            Message = message;
            Items = items;
        }

        public bool Success { get; set; }
        public CrudOperation Operation { get; set; }
        public string Message { get; set; }
        public List<object>? Items { get; set; }
    }

    public enum CrudOperation
    {
        Create,
        Read,
        Update,
        Delete
    }
}
