namespace OasisAPI.Models;

public class OasisApiResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    
    public static OasisApiResult<T> SuccessResponse(T data)
    {
        return new OasisApiResult<T>
        {
            Success = true,
            Data = data
        };
    }
    
    public static OasisApiResult<T> ErrorResponse(string message)
    {
        return new OasisApiResult<T>
        {
            Success = false,
            Message = message
        };
    }
}