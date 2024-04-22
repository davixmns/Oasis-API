namespace OasisAPI.Models;

public class OasisApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    
    public static OasisApiResponse<T> SuccessResponse(T data)
    {
        return new OasisApiResponse<T>
        {
            Success = true,
            Data = data
        };
    }
    
    public static OasisApiResponse<T> ErrorResponse(string errorMessage)
    {
        return new OasisApiResponse<T>
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}