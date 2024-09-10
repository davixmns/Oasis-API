namespace OasisAPI.Models;

public class AppResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    
    public static AppResult<T> SuccessResponse(T data)
    {
        return new AppResult<T>
        {
            IsSuccess = true,
            Data = data
        };
    }
    
    public static AppResult<T> ErrorResponse(string message)
    {
        return new AppResult<T>
        {
            IsSuccess = false,
            Message = message
        };
    }
}